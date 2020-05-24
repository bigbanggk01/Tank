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
    public class Network
    {
        Socket _client;
        public const int _buffer = 1024;
        string _currentData;
        Form1 form;
        Tank enemyTank;
        public Tank myTank;
        public List<Socket> _peerList=new List<Socket>();
        public int _identification;
        public Socket client2;
        public void GetForm(Form1 f)
        {
            form = f;
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
            
            int[] b = s.Split(';').Select(int.Parse).ToArray();
            int a = b[0];
            if (a == 2)
            {
                IPEndPoint ip = new IPEndPoint(IPAddress.Any, b[1]);
                _client.Bind(ip);
                myTank = this.GetTank(form.tank1);
                enemyTank = this.GetTank(form.tank2);
                _identification = 0;
                while (true)
                {
                    _client.Listen(2);
                    Socket client_peer = _client.Accept();
                    client2=client_peer;
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
                    _client.Connect(IPAddress.Parse("127.0.0.1"), b[1]);
                    enemyTank = this.GetTank(form.tank1);
                    myTank = this.GetTank(form.tank2);
                    _identification = 1;
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
            try { _client.Send(Serialize(data_need_to_be_sent)); }
            catch (Exception e)
            {
                return;
            }
        }
        public void Send(string data_need_to_be_sent, Socket client)
        {
            try { client.Send(Serialize(data_need_to_be_sent)); }
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
    }
}
