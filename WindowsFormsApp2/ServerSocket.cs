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

namespace WindowsFormsApp2
{
    class ServerSocket
    {
        
        Socket _server;
        string _currentData;
        public List<Socket> _clientList;
        private List<string> _ipList;
        public const int _buffer = 1024;
        string _ipAdressOfpeer_1 = "";
        string _ipAdressOfpeer_2 = "";
        /// <summary>
        /// Run server 
        /// </summary>
        /// <returns></returns>
        public bool Start()
        {
            _clientList = new List<Socket>();
            _ipList = new List<string>();
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
                        _clientList.Add(client);
                        Thread receive = new Thread(Receive);
                        receive.IsBackground = true;
                        receive.Start(client);
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
            Socket client = obj as Socket;
            _currentData = "";
            try
            {
                while (true)
                {
                    byte[] buffer = new byte[_buffer * 5];
                    client.Receive(buffer);

                    _currentData = (string)Deserialize(buffer);
                    string data = _currentData as string;
                    char[] b = { ';' };
                    Int32 count = 3;
                    String[] strList = data.Split(b, count, StringSplitOptions.RemoveEmptyEntries);

                    if (strList[0].Equals("Login"))
                    {
                        SqlConnection sqlcon = new SqlConnection(@"Data Source=DESKTOP-AB6F94G;Initial Catalog=TankDB;Integrated Security=True");
                        SqlDataAdapter sda = new SqlDataAdapter(strList[1], sqlcon);
                        DataTable dtbl = new DataTable();
                        sda.Fill(dtbl);
                        if (dtbl.Rows.Count == 1)
                        {
                            data = "0;" + _clientList.IndexOf(client);
                            if ((object)_currentData != null)
                            {
                                _ipList.Add(strList[2]);
                                Execute(data);
                            }
                        }
                    }
                    if (strList[0].Equals("update")==true)
                    {
                        SqlConnection sqlcon = new SqlConnection(@"Data Source=DESKTOP-AB6F94G;Initial Catalog=TankDB;Integrated Security=True");
                        SqlDataAdapter sda = new SqlDataAdapter(strList[1], sqlcon);
                    }

                    if (strList[0].Equals("disconnect") == true)
                    {

                    }
                    
                }
            }
            catch
            {
                _clientList.Remove(client);
                client.Close();
            }
        }
        /// <summary>
        /// Action after data receiving 
        /// </summary>
        /// <param name="obj"></param>
        private void Execute(object obj)
        {
            string s = "";
            try { s= (string)obj; }
            catch { };
            int[] b = s.Split(';').Select(int.Parse).ToArray();
            
            if (b[0] == 0)
            {   
                if (b[1] % 2 == 0)
                {
                    string currentClient= Get_ipList(_ipList);
                    Send("2;"+currentClient, _clientList[b[1]]);
                }
                if (b[1] % 2 != 0)
                {
                    string currentClient = Get_ipList(_ipList);
                    if (_clientList[b[1]-1].Connected== false)
                    {
                        this.Send("2;" + currentClient, _clientList[b[1]]);
                        _clientList[b[1] - 1] = _clientList[b[1]];
                        _clientList.Remove(_clientList[b[1]]);
                    }
                    this.Send("1;" + currentClient, _clientList[b[1]]);
                }
            }
        }
        private string Get_ipList(List<string> ipList)
        {
            string output = "";
            foreach(string item in ipList)
            {
                output = item + "?";
            }
            return output;
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
}
