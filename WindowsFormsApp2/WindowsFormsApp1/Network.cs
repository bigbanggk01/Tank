using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography.X509Certificates;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.Reflection.Emit;

namespace WindowsFormsApp1
{
    class Network
    {
        Socket _client;
        public object _currentData;
        
        public const int _buffer = 1024;
        public void Start()
        {
            _client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipAdress = IPAddress.Parse("127.0.0.1");
            IPEndPoint iPEndPoint = new IPEndPoint(ipAdress, 11000);
            try
            {
                _client.Connect(iPEndPoint);
                Thread listen = new Thread(Receive);
                listen.IsBackground = true;
                listen.Start();
            }
            catch (Exception )
            {
                MessageBox.Show("Bảo trì máy chủ, mời các vị cút khỏi trò chơi! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                form.Invoke((MethodInvoker)delegate
                {
                    form.Close();
                });

            }
        }

        public void Send(string data_need_to_be_sent)
        {
            try { _client.Send(Serialize(data_need_to_be_sent)); }
            catch 
            {
                MessageBox.Show("Bảo trì máy chủ, mời các vị cút khỏi trò chơi! ");
            }
        }
        Form1 form;
        public void GetForm(Form1 f)
        {
            form = f;
        }

        public void Receive()
        {
            _currentData = new object();
            try
            {
                while (true)
                {
                    byte[] buffer = new byte[_buffer * 5];
                    _client.Receive(buffer);
                    _currentData = Deserialize(buffer);
                    string data = _currentData as string;
                    
                    Thread Executor = new Thread(form.Execute);
                    Executor.IsBackground = true;
                    Executor.Start((object)data);
                }
            }
            catch
            {
                MessageBox.Show("Bảo trì máy chủ, mời các vị cút khỏi trò chơi!");
                form.Invoke((MethodInvoker)delegate 
                {  
                    form.Close();
                });
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
