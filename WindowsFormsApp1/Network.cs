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

namespace WindowsFormsApp1
{
    public class Network
    {
        Socket _client;
        public const int _buffer = 1024;
        string _currentData;
        Form1 form;
        Tank enemyTank;
        LoginForm LoginForm;
        public Tank myTank;
        public List<Socket> _peerList=new List<Socket>();
        public int _identification;
        public Socket client2;
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
            catch (Exception e)
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
            catch (Exception e)
            {
                
            }
        }
        private void Execute(object data)
        {
            _client.Close();
            _client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            

            string s = data as string;
            char[] b = { ';' };
            Int32 count = 2;
            String[] strList = s.Split(b, count, StringSplitOptions.RemoveEmptyEntries);
            if (strList[0].Equals("2")==true)
            {
                IPEndPoint ip = new IPEndPoint(IPAddress.Any, 11001);
                //string IpAdress = GetLocalIP(NetworkInterfaceType.Wireless80211);
                string IpAdress = GetLocalIP(NetworkInterfaceType.Ethernet);
                _client.Bind(ip);

                myTank = this.GetTank(form.tank1);
                enemyTank = this.GetTank(form.tank2);
                _identification = 0;
                
                form.Invoke((MethodInvoker)delegate
                {
                    form.WindowState = FormWindowState.Maximized;
                    form.ShowInTaskbar = true;
                    LoginForm.Hide();
                });
                _client.Listen(2);
                Socket client_peer = _client.Accept();
                client2 = client_peer;
                Thread listenPeer = new Thread(Receive_Peer);
                listenPeer.IsBackground = true;
                listenPeer.Start(client_peer);
                listen.Abort();
            }
            if (strList[0].Equals("1")==true)
            {
                IPEndPoint ip = new IPEndPoint(IPAddress.Parse(strList[1]), 11001);
                try
                {
                    _client.Connect(ip);
                    enemyTank = this.GetTank(form.tank1);
                    myTank = this.GetTank(form.tank2);
                    _identification = 1;
                    form.Invoke((MethodInvoker)delegate
                    {
                        form.WindowState = FormWindowState.Maximized;
                        form.ShowInTaskbar = true;
                        LoginForm.Hide();
                    });
                    if (_client.Connected == true) 
                    {
                        Thread listenPeer = new Thread(Receive_Peer);
                        listenPeer.IsBackground = true;
                        listenPeer.Start(null);
                        listen.Abort();
                    }
                    
                }
                catch(Exception e)
                {
                    listen.Abort();
                    _client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11000);
                    //IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.0.103"), 11000);
                    try
                    {
                        _client.Connect(iPEndPoint);
                        listen = new Thread(Receive);
                        listen.IsBackground = true;
                        listen.Start();
                        LoginForm.CallBackToConnect();
                        LoginForm.CallBackToConnect();
                    }
                    catch (Exception E)
                    {
                        MessageBox.Show(E.ToString());
                    }
                }

            }
        }
        
        public void Receive_Peer(object obj)
        {
            Socket client = obj as Socket;
            if(client == null) { client = _client;}
            _currentData = "";
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
        public void Send(string data_need_to_be_sent)
        {
            try
            {
                _client.Send(Serialize(data_need_to_be_sent));
                completed.Set();
            }
            catch (Exception e)
            {
                return;
            }
        }
        public void Send(string data_need_to_be_sent, Socket client)
        {
            if (client == null) return;
            try 
            { 
                client.Send(Serialize(data_need_to_be_sent)); 
            }
            catch (Exception e)
            {
                return;
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
