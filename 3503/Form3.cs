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

namespace _3503
{
    public partial class Form3 : Form
    {

        Socket client_socket;
        bool isConnected;
        byte[] bytes = new byte[1024];
        string data;
        Thread listen_thread;
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
            string name = tb_name.Text;
            if (tb_name.Text.Replace(" ", "").Equals(""))
            {
                name = tb_ip.Text;
            }
            byte[] msg = Encoding.UTF8.GetBytes(name+" : "+tb_msg.Text + "<eof>");
            int bytesSent = client_socket.Send(msg);
            tb_msg.Clear();
            tb_msg.Text = "";
        }

        private void btn_connect_Click(object sender, EventArgs e)
        {
            //접속 버튼을 눌렀을 때 발생하는 것입니다.
            //여기서 말하는 textBox2는 IP주소가 입력되는 TextBox이니 박스의 이름에 주의하세요.
            if (isConnected == true)
                return;
            client_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            client_socket.Connect(new IPEndPoint(IPAddress.Parse(tb_ip.Text), int.Parse(tb_port.Text)));
           tb_ip_my.Text = client_socket.LocalEndPoint.ToString();
            tb_name.Text =client_socket.LocalEndPoint.ToString();
            //chattingList.Items.Add(String.Format("소켓 연결이 되었습니다 {0}", client_socket.RemoteEndPoint.ToString()));
            chattingList.Items.Add("[채팅방에 참여하였습니다.]");
            isConnected = true;
            //아래 두개는 do_receive 함수를 위한 쓰레드입니다.
            //쓰레드가 있어야만 연결된다고 해야할까요.
            listen_thread = new Thread(do_receive);
            listen_thread.Start();

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
            MessageBox.Show("종료");
            if(listen_thread!=null){
                listen_thread.Abort();
                client_socket.Shutdown(SocketShutdown.Both);

            }
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
       

    
}
}
