﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text.RegularExpressions;

namespace _3503
{
    public partial class Form2 : Form
    {
        Socket listen_socket;
        List<Socket> connectedClients = new List<Socket>();
        List<Thread> receive_threads = new List<Thread>();


        bool isConnected;  //bool로 서버와 클라이언트가 연결되었는지 true /false로 판단

        byte[] bytes = new byte[1024];
        string data;  //채팅서버이기 때문에 글자 데이터를 읽고 불러오는데에 쓰이는 선언.

        //Thread receive_thread;
        Thread listen_thread;

        string user_name;

        public Form2()
        {
            InitializeComponent();
            isConnected = false;

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            user_name = Client_IP;
            tb_ip.Text = user_name;

        }

        private void btn_Listen_Click(object sender, EventArgs e)
        {

            if (CheckIP(tb_ip.Text) && CheckPort(tb_port.Text))
            {
                start(tb_ip.Text, int.Parse(tb_port.Text), 10);

            }

        }


        private bool CheckIP(string ip)//ip 검사
        {
            if (ip.Replace(" ", "") == "" || ip == null)
            {
                MessageBox.Show("IP를 입력해주세요.");
                return false;
            }
            string ValidIpAddressRegex = "^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\\.){3}([0-9]|[1-9][0-9]|1[0-9‌​]{2}|2[0-4][0-9]|25[0-5])$";    // IP validation 

            Regex r = new Regex(ValidIpAddressRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Match m = r.Match(ip);

            if (!m.Success)
            {
                MessageBox.Show("유효한 IP를 입력해주세요.");
            }


            return m.Success;




        }

        private bool CheckPort(string port)//port 검사
        {
            if (port.Replace(" ", "") == "" || port == null)
            {
                MessageBox.Show("port를 입력해주세요.");
                return false;
            }
            int port_num;

            try
            {
                port_num = int.Parse(port);
            }
            catch
            {
                port_num = -1;
            }

            if (port_num < 0 || port_num > 65535)
            {
                MessageBox.Show("유효한 port를 입력해주세요.");
                return false;

            }
            return true;

        }

        private void btn_send_Click(object sender, EventArgs e)
        {
            if (isConnected == false)
                return;



            SendMessage(user_name + " : " + tb_msg.Text);



            tb_msg.Clear();
            tb_msg.Text = "";
        }

        void SendMessage(string data)
        {


            chattingList.Items.Add(data);

            // for을 통해 "역순"으로 클라이언트에게 데이터를 보낸다.
            for (int i = connectedClients.Count - 1; i >= 0; i--)
            {
                Socket socket = connectedClients[i];
                if (socket != this.listen_socket)
                {
                    try
                    {
                        byte[] msg = Encoding.UTF8.GetBytes(data + "<eof>");
                        socket.Send(msg);
                    }
                    catch
                    {
                        //MessageBox.Show("에러");
                        // 오류 발생하면 전송 취소하고 리스트에서 삭제한다.
                        try { socket.Dispose(); }
                        catch { }
                        connectedClients.RemoveAt(i);
                        receive_threads.RemoveAt(i);
                    }
                }
            }
        }

        public void start(string host, int port, int backlog)
        {

            chattingList.Items.Add("[-- 참여자를 기다리는 중입니다. --]");


            this.listen_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress address;
            if (host == "0.0.0.0")
            {
                address = IPAddress.Any;
            }
            else
            {
                address = IPAddress.Parse(host);
            }
            IPEndPoint endpoint = new IPEndPoint(address, port);

            try
            {
                listen_socket.Bind(endpoint);
                listen_socket.Listen(backlog);

                listen_thread = new Thread(WaitSocket);
                listen_thread.Start();

            }
            catch
            {
                MessageBox.Show("에러가 발생하였습니다.\n port를 변경하거나 프로그램을 재시작하면 해결됩니다.");
            }

        }

        void WaitSocket()//클라이언트를 기다리는 함수를 쓰레드로. 비동기
        {
            while (true)
            {
                Socket client_socket = listen_socket.Accept();

                connectedClients.Add(client_socket);

                string client_address = client_socket.RemoteEndPoint.ToString();


                isConnected = true;
                Thread receive_thread = new Thread(delegate()
                {
                    do_receive(client_socket);
                });
                receive_threads.Add(receive_thread);

                //문자받는 쓰레드 시작
                receive_thread.Start();
            }

        }


        void do_receive(Socket client_socket)
        {
            while (isConnected)
            {
                while (true)
                {
                    try
                    {
                        byte[] bytes = new byte[1024];
                        int bytesRec = client_socket.Receive(bytes);


                        data += Encoding.UTF8.GetString(bytes, 0, bytesRec);
                        if (data.IndexOf("<eof>") > -1)
                            break;
                    }
                    catch
                    {
                        
                    }
                    if (data.IndexOf("q[--") == 0)
                    {


                        int index = connectedClients.IndexOf(client_socket);
                        connectedClients.RemoveAt(index);
                        receive_threads.RemoveAt(index);
                        client_socket.Dispose();
                    }
                }
                data = data.Replace("q[--", "[--");
                data = data.Substring(0, data.Length - 5);
                try
                {
                    Invoke((MethodInvoker)delegate
                    {

                        SendMessage(data);
                    }
                    );
                }
                catch
                {
                    MessageBox.Show("채팅을 시작할 수 없습니다.");
                }

                data = "";
            }
        }

        private void btn_close_Click(object sender, EventArgs e)//종료함수
        {
            try
            {

                SendMessage("[-- 채팅이 종료되었습니다. --]");

                for (int i = connectedClients.Count - 1; i >= 0; i--)
                {
                    Socket socket = connectedClients[i];
                    
                            try { socket.Dispose(); }
                            catch { }
                            connectedClients.RemoveAt(i);
                            receive_threads.RemoveAt(i);
                        
                }

                if (receive_threads != null)
                {
                    foreach (Thread receive_thread in receive_threads)
                    {
                        receive_thread.Abort();
                    }
                }

                if (this.listen_socket != null)
                {

                    listen_socket.Shutdown(SocketShutdown.Both);
                    listen_socket.Close();

                }
            }
            catch
            {
            }
            this.Close();
        }

        private void chattingList_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void tb_ip_TextChanged(object sender, EventArgs e)
        {
            tb_name.Text = tb_ip.Text;
        }


        public static string Client_IP//내 ip가져오기
        {
            get
            {


                IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
                string ClientIP = string.Empty;
                for (int i = 0; i < host.AddressList.Length; i++)
                {
                    if (host.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        ClientIP = host.AddressList[i].ToString();
                    }
                }
                return ClientIP;
            }

        }

        private void btn_change_Click(object sender, EventArgs e)
        {
            string name = tb_name.Text;
            if (tb_name.Text.Replace(" ", "").Equals(""))
            {
                MessageBox.Show("이름을 입력해주세요.");
                tb_name.Text = user_name;
            }
            else
            {
                SendMessage("[-- '" + user_name + "'님이 '" + tb_name.Text + "' (으)로 이름을 변경하였습니다. --]");
                user_name = tb_name.Text;

            }
        }
    }
}
