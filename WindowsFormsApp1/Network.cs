using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Net.PeerToPeer;

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
        Tank enemyTank;
        LoginForm LoginForm;
        public Tank myTank;
        public List<Socket> _peerList=new List<Socket>();
        public int _identification;
        ManualResetEvent completed = new ManualResetEvent(false);
        Thread listen;
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
            //IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.0.103"), 11000);
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
            catch 
            {
                
            }
        }
        private void Execute(object data)
        {
            //_client.Close();
            //_client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _client1 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            string s = data as string;
            char[] b = { ';' };
            Int32 count = 2;
            String[] strList = s.Split(b, count, StringSplitOptions.RemoveEmptyEntries);
            if (strList[0].Equals("2")==true)
            {
                
                //string IpAdress = GetLocalIP(NetworkInterfaceType.Wireless80211);
                string IpAdress = GetLocalIP(NetworkInterfaceType.Ethernet);
                IPEndPoint ip = new IPEndPoint(IPAddress.Parse(IpAdress), 11001);
                _client1.Bind(ip);

                myTank = this.GetTank(form.tank1);
                enemyTank = this.GetTank(form.tank2);
                _identification = 0;
                
                form.Invoke((MethodInvoker)delegate
                {
                    form.WindowState = FormWindowState.Maximized;
                    form.ShowInTaskbar = true;
                    LoginForm.Hide();
                });

                //Bat dau lang nghe nguoi thu 2
                _client1.Listen(1);
                Socket client_peer = _client1.Accept();
                _client2 = client_peer;

                Thread listenPeer = new Thread(Receive_Peer);
                listenPeer.IsBackground = true;
                listenPeer.Start(client_peer);
                //Dong Thread nghe server 
                listen.Abort();
            }
            if (strList[0].Equals("1")==true)
            {
                IPEndPoint ip = new IPEndPoint(IPAddress.Parse(strList[1]), 11001);
                try
                {
                    _client1.Connect(ip);
                    enemyTank = this.GetTank(form.tank1);
                    myTank = this.GetTank(form.tank2);
                    _identification = 1;
                    form.Invoke((MethodInvoker)delegate
                    {
                        form.WindowState = FormWindowState.Maximized;
                        form.ShowInTaskbar = true;
                        LoginForm.Hide();
                    });
                    if (_client1.Connected == true) 
                    {
                        Thread listenPeer = new Thread(Receive_Peer);
                        listenPeer.IsBackground = true;
                        listenPeer.Start(null);
                        listen.Abort();
                    }
                }
                catch
                {
                   // listen.Abort();
                   //// _client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                   // _client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                   // IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11000);
                   // //Dung lenh nay khi choi trong mang Lan khong day, gateway la 192.168.0.1:
                   // //IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.0.103"), 11000);
                   // try
                   // {
                   //     _client.Connect(iPEndPoint);
                   //     listen = new Thread(Receive);
                   //     listen.IsBackground = true;
                   //     listen.Start();
                   //     //Neu khong ket noi duoc thi Connect 2 lan de server quay lai trang thai khoi dau
                   //     LoginForm.CallBackToConnect();
                   //     LoginForm.CallBackToConnect();
                   // }
                   // catch (Exception E)
                   // {
                   //     MessageBox.Show(E.ToString());
                   // }
                }
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
                completed.Set();
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
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, o);
            return ms.ToArray();
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
