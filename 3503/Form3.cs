using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;   //아래 3개 역시 마찬가지.
using System.Net.Sockets;
using System.Threading;
using System.Text.RegularExpressions;


namespace _3503
{
    public partial class Form3 : Form
    {

        Socket client_socket;
        bool isConnected;
        byte[] bytes = new byte[1024];
        string data;
        Thread listen_thread;

        string user_name;


        public Form3()
        {
            InitializeComponent();
            isConnected = false;

        }

        private void Form3_Load(object sender, EventArgs e)
        {
            
        }

        private void btn_send_Click(object sender, EventArgs e)
        {
            //send 부분입니다. 보내기 버튼을 눌렀을 때 글자 데이터를 보내는 역할입니다. 
            //연결이 안되어있을때는 그냥 return으로 보내졌을 때는 Clear함수와 함께 채팅창을 다시 원위치시키는것으로 마무리.
            if (isConnected == false)
                return;
            
            SendMessage(user_name + " : " + tb_msg.Text);
            tb_msg.Clear();
            tb_msg.Text = "";
        }

        private void btn_connect_Click(object sender, EventArgs e)
        {
            //접속 버튼을 눌렀을 때 발생하는 것입니다.
            //여기서 말하는 textBox2는 IP주소가 입력되는 TextBox이니 박스의 이름에 주의하세요.
            if (isConnected == true)
                return;
            if (CheckIP(tb_ip.Text) && CheckPort(tb_port.Text))
            {

                client_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                client_socket.Connect(new IPEndPoint(IPAddress.Parse(tb_ip.Text), int.Parse(tb_port.Text)));
                tb_ip_my.Text = client_socket.LocalEndPoint.ToString();
                user_name = tb_ip_my.Text;
                tb_name.Text = user_name;
                //chattingList.Items.Add(String.Format("소켓 연결이 되었습니다 {0}", client_socket.RemoteEndPoint.ToString()));

                chattingList.Items.Add("[-- 채팅방에 참여하였습니다. --]");

                SendMessage("[-- '" + user_name + "' 님이 참여하셨습니다. --]");


                isConnected = true;
                //아래 두개는 do_receive 함수를 위한 쓰레드입니다.
                //쓰레드가 있어야만 연결된다고 해야할까요.
                listen_thread = new Thread(do_receive);
                listen_thread.Start();
            }
        }

        private void SendMessage(string message)
        {
           
            byte[] msg = Encoding.UTF8.GetBytes(message+" <eof>");
            int bytesSent = client_socket.Send(msg);
        }

        private bool CheckIP(string ip)
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

        private bool CheckPort(string port)
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

        void do_receive()
        {
            while (isConnected)
            {
                while (true)
                {
                    byte[] bytes = new byte[1024];
                    int bytesRec = client_socket.Receive(bytes);
                    data += Encoding.UTF8.GetString(bytes, 0, bytesRec);
                    if (data.IndexOf("<eof>") > -1)
                        break;
                }
                data = data.Substring(0, data.Length - 5);
                Invoke((MethodInvoker)delegate
                {
                    chattingList.Items.Add(data);
                }
                );
                data = "";
            }
        }

        private void btn_close_Click(object sender, EventArgs e)
        {

            SendMessage("[-- '"+user_name+"'님이 퇴장하셨습니다. --]");

            try
            {
                //  MessageBox.Show("종료");
                if (listen_thread != null)
                {
                    listen_thread.Abort();
                    client_socket.Shutdown(SocketShutdown.Both);

                }
            }
            catch { }
            this.Close();


        }

        private void tb_name_TextChanged(object sender, EventArgs e)
        {

        }

        private void tb_ip_TextChanged(object sender, EventArgs e)
        {

        }

        private void tb_msg_TextChanged(object sender, EventArgs e)
        {

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
                SendMessage("[" + user_name + "]님이 [" + tb_name.Text + "] 로 이름을 변경하였습니다.");
                user_name = tb_name.Text;

            }
        }
       

    
}
}
