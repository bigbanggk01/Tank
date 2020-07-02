using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using System.Threading;
using System.Data.SqlClient;
using System.Data;
using System.Runtime.InteropServices.WindowsRuntime;

namespace WindowsFormsApp2
{
    class ServerSocket
    {
        Socket _server;
        string _currentData;
        List<Player> _player;
        public const int _buffer = 1024;
        SqlConnection sqlcon = new SqlConnection(@"Data Source=DESKTOP-AB6F94G;Initial Catalog=TankDB;Integrated Security=True");
        /// <summary>
        /// Run server 
        /// </summary>
        /// <returns></returns>
        public bool Start()
        {
            _player= new List<Player>();
            _server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, 11000);
            try 
            { 
                _server.Bind(iPEndPoint);
            }
            catch { }
            Thread listen = new Thread(() =>
            {
                try
                {
                    while (true)
                    {
                        _server.Listen(2);
                        Socket client = _server.Accept();
                        Player player = new Player();
                        player.client = client;
                        _player.Add(player);
                        Thread receive = new Thread(Receive);
                        receive.IsBackground = true;
                        receive.Start(player);
                    }
                }
                catch
                {
                }
            });
            listen.IsBackground = true;
            listen.Start();
            return true;
        }
        /// <summary>
        /// Send func
        /// </summary>
        /// <param name="data_need_to_be_sent"></param>
        /// <param name="client"></param>
        public void Send(string data_need_to_be_sent, Socket client)
        {
            try { client.Send(Serialize(data_need_to_be_sent)); }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Problem", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// Reveive function
        /// </summary>
        /// <param name="obj"></param>
        public void Receive(object obj)
        {
            Player player = obj as Player;
            _currentData = "";
            try
            {
                while (true)
                {
                    byte[] buffer = new byte[_buffer * 5];
                    player.client.Receive(buffer);
                    _currentData = (string)Deserialize(buffer);
                    string data = _currentData as string;
                    char[] b = { ';' };
                    Int32 count = 4;
                    String[] strList = data.Split(b, count, StringSplitOptions.RemoveEmptyEntries);

                    if (strList[0].Equals("Login"))
                    {
                        Make_Player_Online(strList[1],strList[2],strList[3], player);   
                    }

                    if (strList[0].Equals("gameover")==true)
                    {
                        Update_Match_Result(strList[1],player);
                    }

                    if (strList[0].Equals("disconnect") == true)
                    {
                        Handling_Disconnections(strList[1], player);
                    }

                    if (strList[0].Equals("binded") == true)
                    {
                        Store_Player_Port(int.Parse(strList[1]), player);
                    }

                    if (strList[0].Equals("joint") == true)
                    {
                        Response_Client_Join_Event(strList[1], player);
                    }

                    if(strList[0].Equals("create") == true)
                    {
                        Add_Room_Made_By_Player(strList[1], strList[2], player);
                    }

                    if (strList[0].Equals("register") == true)
                    {
                        Add_New_User(strList[1], strList[2], player);
                    }
                }
            }
            catch
            {
                _player.Remove(player);
                player.client.Close();
            }
        }

        /// <summary>
        /// Trả lời client khi nó muốn đăng nhập
        /// </summary>
        /// <param name="str"></param>
        /// <param name="player"></param>
        private void Make_Player_Online(string strUser,string strPass,string ipUser,Player player) 
        {
            string query = "Select * from Usertable Where username = '" + strUser
                + "' and passwo = '" + strPass + "'";
            sqlcon.Open();
            using (SqlDataAdapter sda = new SqlDataAdapter(query, sqlcon))
            {
                DataTable dtbl = new DataTable();
                sda.Fill(dtbl);
                if (dtbl.Rows.Count == 1)
                {
                    player.username = strUser;
                    if ((object)_currentData != null)
                    {
                        _player[_player.IndexOf(player)].ip = ipUser;
                        string room = "";
                        foreach (Player item in _player)
                        {
                            if (item.Room == null) continue;
                            if (item.Room.isEmpty == true) 
                            { 
                                room += item.Room.name + "....................." + item.Room.title 
                                    + ".................." + "Waiting enemy;"; 
                            }
                            else if(item.Room.isEmpty == false)
                            {
                                room += item.Room.name + "....................." + item.Room.title
                                    + ".................." + "Full;";
                            }
                        }
                        Send("loginok;" + room, player.client);
                    }
                }
            }
            sqlcon.Close();
        }

        /// <summary>
        /// Lưu port của client để chuyển tới enemy của nó
        /// </summary>
        /// <param name="port"></par
        private void Store_Player_Port(int port,Player player) 
        {
            player.port = port;
        }

        /// <summary>
        /// Thêm phòng được tạo ra bởi player vào thuộc tính của nó 
        /// </summary>
        /// <param name="strRoom"></param>
        /// <param name="strTit"></param>
        /// <param name="player"></param>
        private void Add_Room_Made_By_Player(string strRoom,string strTit, Player player)
        {
            foreach (Player item in _player)
            {
                if (item.Room == null) continue;
                if (item.Room.name.Equals(strRoom) == true) return;
            }
            player.Room = new Room();
            player.Room.name = strRoom;
            player.Room.title = strTit;
            player.Room.isEmpty = true;
            Send("createok", player.client);
        }

        /// <summary>
        /// Trả lời client khi client yêu cầu tham gia một phòng
        /// </summary>
        /// <param name="str"></param>
        /// <param name="player"></param>
        private void Response_Client_Join_Event(string str, Player player)
        {
            int i = 0;
            string response = "";
            foreach (Player item in _player)
            {
                if (item.Room == null) continue;
                if (item.Room.name.Equals(str) == true)
                {
                    if (item.Room.isEmpty == true) 
                    {
                        player.Room = new Room();
                        player.Room = item.Room;
                        item.Room.isEmpty = false;
                        response += "jointok;" + item.ip + ";" + item.port;
                        Send(response, player.client);
                        return;
                    }
                    else
                    {
                        response += "joint0ok";
                        return;
                    }
                }
                i++;
            }
        }

        /// <summary>
        /// Đăng ký tài khoản
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="player"></param>
        private void Add_New_User(string username, string password, Player player)
        {
            if (isExist(username) == true) 
            {
                Send("register0ok", player.client);
                return;
            }
            else if (isExist(username) == false) 
            {
                int id;
                string query = "Select * from Usertable";
                using (SqlDataAdapter sda = new SqlDataAdapter(query, sqlcon))
                {
                    DataTable dtbl = new DataTable();
                    sda.Fill(dtbl);
                    id = dtbl.Rows.Count + 1;
                }
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    using (SqlCommand command = new SqlCommand("insert into Usertable(id,username, passwo, tank)" +
                        "values(@id,@username,@passwo,@tank)", sqlcon))
                    {
                        command.Parameters.Add("@id", SqlDbType.Int, id);
                        command.Parameters["@id"].Value = id;
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@passwo", password);
                        command.Parameters.AddWithValue("@tank", 1);
                        sda.InsertCommand = command;
                        sqlcon.Open();
                        sda.InsertCommand.ExecuteNonQuery();
                        sqlcon.Close();
                    }
                }
                Send("registerok", player.client);
                return;
            }
            
        }

        /// <summary>
        /// Cập nhật kẻ thằng người thua
        /// </summary>        
        private void Update_Match_Result(string result, Player player)
        {
            if (result.Equals("lose") == true) 
            {
                try 
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        using (SqlCommand command = sqlcon.CreateCommand())
                        {
                            command.CommandText = "update Usertable set tank = 0 where username = @username";
                            command.Parameters.AddWithValue("@username", player.username);
                            sda.UpdateCommand = command;
                            sqlcon.Open();
                            sda.UpdateCommand.ExecuteNonQuery();
                            sqlcon.Close();
                        }
                    }
                }
                catch(Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
            
        }

        /// <summary>
        /// Xử lý khi một hoặc nhiều player mất kết nối
        /// </summary>
        private void Handling_Disconnections(string str, Player player)
        {
            
        }

        /// <summary>
        /// Kiểm tra username đã được sử dụng chưa
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
        private bool isExist(string Username)
        {
            string query = "Select * from Usertable Where username = '" + Username + "'";
            sqlcon.Open();
            using (SqlDataAdapter sda = new SqlDataAdapter(query, sqlcon))
            {
                DataTable dtbl = new DataTable();
                sda.Fill(dtbl);

                if (dtbl.Rows.Count == 0)
                {
                    sqlcon.Close();
                    return false;
                }
                else 
                {
                    sqlcon.Close();
                    return true;
                }
            }
            
        }

        /// <summary>
        /// Serialize and Deserialize
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        byte[] Serialize(object o)
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, o);
            return ms.ToArray();
        }

        object Deserialize(byte[] data)
        {
            MemoryStream ms = new MemoryStream(data);
            BinaryFormatter bf = new BinaryFormatter();
            return bf.Deserialize(ms);
        }
        public string GetLocalIP(NetworkInterfaceType type)
        {
            string output = "";
            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (item.NetworkInterfaceType == type && item.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            output = ip.Address.ToString();
                        }
                    }
                }
            }
            return output;
        }
    }
    class Player
    {
        public string username;
        public Socket client;
        public int port;
        public string ip;
        public Room Room;
    }
    class Room
    {
        public string name;
        public string title;
        public bool isEmpty;
    }
}
