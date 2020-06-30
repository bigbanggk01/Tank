using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Linq;

namespace WindowsFormsApp1
{
    public class Network
    {
        public Socket _client;
        public Socket _client1;
        public Socket _client2;
        public const int _buffer = 1024;
        string _currentData;
        Form1 form;
        public Tank enemyTank;
        LoginForm LoginForm;
        public Tank myTank;
        public List<Socket> _peerList=new List<Socket>();
        public int _identification;
        Thread listen;
        public List<string> _ipList = new List<string>();
        public List<string> _roomList = new List<string>();
        RegistorForm registorFrom;
        public void GetRegisterForm(RegistorForm rf)
        {
            registorFrom = rf;
        }
        public void GetForm(Form1 f)
        {
            form = f;
        }
        public Tank GetTank(Tank t)
        {
            return  t;
        }
        public void GetLogin(LoginForm lg)
        {
            LoginForm = lg;
        }
        public bool Start()
        {
            _client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11000);
            try
            {
                _client.Connect(iPEndPoint);
                listen = new Thread(Receive);
                listen.IsBackground = true;
                listen.Start();
                return true;
            }
            catch 
            {
                return false;
            }
        }
        public void Stop()
        {
            _client.Close();
        }
        public void Receive()
        {
            _currentData = "";
            try
            {
                while (true) 
                {
                    byte[] buffer = new byte[_buffer * 5];
                    _client.Receive(buffer);
                    _currentData = (string)Deserialize(buffer);
                    string data = _currentData as string;
                    if ((object)_currentData != null)
                    {
                        Thread Executor = new Thread(Execute);
                        Executor.IsBackground = true;
                        Executor.Start((object)data);
                    }
                }
            }
            catch 
            {
                
            }
        }
        private void Execute(object data)
        {
            _client1 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            string s = data as string;
            char[] b = { ';' };
            Int32 count = 100;
            String[] strList = s.Split(b, count, StringSplitOptions.RemoveEmptyEntries);
            int port = 11001;
            if (strList[0].Equals("newplayerok")==true )
            {
                string IpAdress = GetLocalIP(NetworkInterfaceType.Ethernet);
                while (true) {
                    try
                    {
                        IPEndPoint ip = new IPEndPoint(IPAddress.Parse(IpAdress), port);
                        _client1.Bind(ip);
                        break;
                    }
                    catch
                    {
                        
                    }
                    port++;
                }

                Send("binded;" + port);
                form.Invoke((MethodInvoker)delegate
                {
                    form.WindowState = FormWindowState.Maximized;
                    form.ShowInTaskbar = true;
                    LoginForm.Hide();
                });
                foreach (string item in strList)
                {
                    if (item.Equals(strList[0]) == true) continue;
                    form.Invoke((MethodInvoker)delegate
                    {
                        form.Room.Items.Add(item);
                    });
                }
                _client1.Listen(1);
                Socket client_peer = _client1.Accept();
                _client2 = client_peer;
                Thread listenPeer = new Thread(Receive_Peer);
                listenPeer.IsBackground = true;
                listenPeer.Start(client_peer);
            }

            if (strList[0].Equals("createok") == true)
            {
                form.Invoke((MethodInvoker)delegate
                {
                    form.RoomName.Dispose();
                    form.Create.Dispose();
                    form.Title.Dispose();
                    form.label1.Dispose();
                    form.label2.Dispose();
                });
                form.StartGame();
            }

            if (strList[0].Equals("joint") == true)
            {
                IPEndPoint ip = new IPEndPoint(IPAddress.Parse(strList[1]), int.Parse(strList[2]));
                _client1 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _client1.Connect(ip);
                form.Invoke((MethodInvoker)delegate
                {
                    form.Room.Dispose();
                });
                form.StartGame2();
                Thread listenPeer = new Thread(Receive_Peer);
                listenPeer.IsBackground = true;
                listenPeer.Start(null);
            }
            if (strList[0].Equals("registerok") == true)
            {
                registorFrom.Invoke((MethodInvoker)delegate
                {
                    registorFrom.Close();
                });
                MessageBox.Show("Đăng ký thành công, vui chơi thỏa thích :))", "Success", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
        }

        /// <summary>
        /// Ham receive khi tro choi bat dau
        /// </summary>
        /// <param name="obj"></param>
        public void Receive_Peer(object obj)
        {
            Socket client = obj as Socket;
            _currentData = "";

            if (client == null) 
            { 
                client = _client1;
            }

            try
            {
                while (true)
                {
                    byte[] buffer = new byte[_buffer * 5];
                    client.Receive(buffer);

                    _currentData = (string)Deserialize(buffer);
                    string data = _currentData as string;
                    int instruction = int.Parse(data);
                    if ((object)_currentData != null)
                    {
                        form.Enemy_Control(instruction);
                    }
                }
            }
            catch
            {
                client.Close();
            }
        }
        /// <summary>
        /// Ham send cua nguoi ket noi sau
        /// </summary>
        /// <param name="data_need_to_be_sent"></param>
        public void Send(string data_need_to_be_sent)
        {
            try
            {
                _client.Send(Serialize(data_need_to_be_sent));
            }
            catch 
            {
                return;
            }
        }
        /// <summary>
        /// ham send cua truong phong
        /// </summary>
        /// <param name="data_need_to_be_sent"></param>
        /// <param name="client"></param>
        public void Send(string data_need_to_be_sent, Socket client)
        {
            if (client == null) return;
            try 
            { 
                client.Send(Serialize(data_need_to_be_sent)); 
            }
            catch
            {
                return;
            }
        }
        /// <summary>
        /// Ham dong goi du lieu thanh mang byte
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        byte[] Serialize(object o)
        {
            try {
                MemoryStream ms = new MemoryStream();
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, o);
                return ms.ToArray();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return null;
            }
            
        }
        /// <summary>
        /// Ham dich du lieu tu mang byte thanh 1 object
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        object Deserialize(byte[] data)
        {
            MemoryStream ms = new MemoryStream(data);
            BinaryFormatter bf = new BinaryFormatter();
            return bf.Deserialize(ms);
        }
        /// <summary>
        /// Lay dia chi IP tu 1 card mang tuy chon
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
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
