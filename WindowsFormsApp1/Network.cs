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
        Tank enemyTank;
        
        public Tank myTank;
        public List<Socket> _peerList=new List<Socket>();
        public void GetForm(Form1 f)
        {
            form = f;
        }
        
        public void GetMap(Map m)
        {
             map= m;
        }
        public Tank GetTank(Tank t)
        {
            return  t;
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
                myTank = this.GetTank(form.tank1);
                enemyTank = this.GetTank(form.tank2);
                while (true)
                {
                    _client.Listen(2);
                    Socket client_peer = _client.Accept();
                    _peerList.Add(client_peer);
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
                    enemyTank = this.GetTank(form.tank1);
                    myTank = this.GetTank(form.tank2);
                }
                catch (Exception e) { MessageBox.Show(e.ToString()); };
                this.Send("0");
                
                Thread listen = new Thread(Receive_Peer);
                listen.IsBackground = true;
                listen.Start(null);
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
                    if ((object)_currentData != null)
                    {
                        if(int.Parse(data) == 7)
                        {
                            
                        }
                        if (int.Parse(data) == 0)
                        {
                            this.Send("7", _peerList[0]);
                        }
                        if(int.Parse(data) == 1)
                        {
                            enemyTank.Go_Up(form, map);
                        }
                        if (int.Parse(data) == 2)
                        {
                            enemyTank.Go_Down(form, map);
                        }
                        if (int.Parse(data) ==3)
                        {
                            enemyTank.Go_Left(form, map);
                        }
                        if (int.Parse(data) == 4)
                        {
                            enemyTank.Go_Right(form, map);
                        }
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
            try { _client.Send(Serialize(data_need_to_be_sent)); }
            catch (Exception e)
            {
            }
        }
        public void Send(string data_need_to_be_sent, Socket client)
        {
            try { client.Send(Serialize(data_need_to_be_sent)); }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Problem", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        //private void Add_tank(int a)
        //{
        //    //if (a == 1)
        //    //{
        //    //    form.Invoke((MethodInvoker)delegate
        //    //    {
        //    //        tank_peer = new Tank();
        //    //        tank_peer.Draw(form);
        //    //        tank_peer.x[0] = 10 + 60;
        //    //        tank_peer.y[0] = 10 + 30;
        //    //        tank_peer.x[1] = 9 + 60;
        //    //        tank_peer.y[1] = 11 + 30;
        //    //        tank_peer.x[2] = 10 + 60;
        //    //        tank_peer.y[2] = 11 + 30;
        //    //        tank_peer.x[3] = 11 + 60;
        //    //        tank_peer.y[3] = 11 + 30;
        //    //        tank_peer.x[4] = 9 + 60;
        //    //        tank_peer.y[4] = 12 + 30;
        //    //        tank_peer.x[5] = 11 + 60;
        //    //        tank_peer.y[5] = 12 + 30;
        //    //    });
        //    //}
        //    form.Invoke((MethodInvoker)delegate
        //    {
        //        tank_peer = new Tank();
        //        tank_peer.Draw(form);
        //    });

        //}
    }
}
