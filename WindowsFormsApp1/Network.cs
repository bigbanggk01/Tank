using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    class Network
    {
        Socket _client;
        public const int _buffer = 1024;
        string _currentData;
        Form1 form;
        Map map;
        Tank tank_peer;
        public void GetForm(Form1 f)
        {
            form = f;
        }
        public void GetMap(Map m)
        {
             map= m;
        }
        public bool Start()
        {
            _client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11000);
            try
            {
                _client.Connect(iPEndPoint);
                Thread listen = new Thread(Receive);
                listen.IsBackground = true;
                listen.Start();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
            
        }

        public void Send(string data_need_to_be_sent)
        {
            try { _client.Send(Serialize(data_need_to_be_sent)); }
            catch (Exception e)
            {
                
            }
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
                //MessageBox.Show(data);
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
            int a = int.Parse(s);
            if (a == 2)
            {
                IPEndPoint ip = new IPEndPoint(IPAddress.Any, 6666);
                _client.Bind(ip);
                while (true)
                {
                    _client.Listen(2);
                    Socket client_peer = _client.Accept();

                    Thread listen = new Thread(Receive_Peer);
                    listen.IsBackground = true;
                    listen.Start(client_peer);
                }
            }
            if (a == 1)
            {
                IPEndPoint ip = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6666);
                try 
                {
                    _client.Connect(IPAddress.Parse("127.0.0.1"), 6666);
                }
                catch (Exception e) { MessageBox.Show(e.ToString()); };
                this.Send("0");
            }
        }
        public void Receive_Peer(object obj)
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
                    if ((object)_currentData != null)
                    {
                        if(int.Parse(data) == 0)
                        {
                            Add_tank();
                        }
                        if(int.Parse(data) == 1)
                        {
                            tank_peer.Go_Up(form,map);
                        }
                        if (int.Parse(data) == 2)
                        {
                            tank_peer.Go_Down(form, map);
                        }
                        if (int.Parse(data) ==3)
                        {
                            tank_peer.Go_Left(form, map);
                        }
                        if (int.Parse(data) == 4)
                        {
                            tank_peer.Go_Right(form, map);
                        }
                    }
                }
            }
            catch
            {
                client.Close();
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

        private void Add_tank()
        {
            form.Invoke((MethodInvoker)delegate 
            {
                tank_peer = new Tank();
                tank_peer.Draw(form);
            });
        }
    }
}
