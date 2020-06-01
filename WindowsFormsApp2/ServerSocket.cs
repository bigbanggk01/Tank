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
using System.Management;
namespace WindowsFormsApp2
{
    class ServerSocket
    {
        
        Socket _server;
        string _currentData;
        public List<Socket> _clientList;
        public const int _buffer = 1024;
        string _ipAdressOfpeer_1 = "";
        string _ipAdressOfpeer_2 = "";
        public bool Start()
        {
            _clientList = new List<Socket>();
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
                    MessageBox.Show("Connect 1 lần thôi thằng ngu!");
                }
            });
            listen.IsBackground = true;
            listen.Start();
            return true;
        }
        public void Send(string data_need_to_be_sent, Socket client)
        {
            try { client.Send(Serialize(data_need_to_be_sent)); }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Problem", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
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
                                _ipAdressOfpeer_1 = _ipAdressOfpeer_2;
                                _ipAdressOfpeer_2 = strList[2];
                                Execute(data);
                            }
                        }
                    }
                    if (strList[0].Equals("update")==true)
                    {
                        SqlConnection sqlcon = new SqlConnection(@"Data Source=DESKTOP-AB6F94G;Initial Catalog=TankDB;Integrated Security=True");
                        SqlDataAdapter sda = new SqlDataAdapter(strList[1], sqlcon);
                    }
                    
                }
            }
            catch
            {
                _clientList.Remove(client);
                client.Close();
            }
        }
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
                    this.Send("2;0", _clientList[b[1]]);
                }
                if (b[1] % 2 != 0)
                {
                    if(_clientList[b[1]-1].Connected== false)
                    {
                        this.Send("2;0", _clientList[b[1]]);
                        _clientList[b[1] - 1] = _clientList[b[1]];
                        _clientList.Remove(_clientList[b[1]]);
                    }
                    this.Send("1;"+_ipAdressOfpeer_1, _clientList[b[1]]);
                }
            }
        }
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
