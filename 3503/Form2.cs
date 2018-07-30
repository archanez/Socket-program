using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;       ////이 이하 3가지는 네트워크 소켓 쓰레딩을 하기 위해서 선언해줍니다.
using System.Net.Sockets;
using System.Threading;


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
        public Form2()
        {
            InitializeComponent();
            isConnected = false;   //초기화부문

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            tb_ip.Text = Client_IP;

        }

        private void btn_Listen_Click(object sender, EventArgs e)
        {

            //TODO: 예외처리하기 (ip,port범위 , port 숫자만)
            start(tb_ip.Text, int.Parse(tb_port.Text), 10);   //버튼1 을 눌렀을 때 실행될 것 127.0.0.1 은 Local입니다
            //여러분도 위와 같은 것으로 해보세요 연습할때는 ㅎㅎ
        }

        private void btn_send_Click(object sender, EventArgs e)
        {
            if (isConnected == false)
                return;

            string name = tb_name.Text;
            if (tb_name.Text.Replace(" ", "").Equals(""))
            {
                name = tb_ip.Text;
            }
            //int bytesSent = client_socket.Send(msg);
            SendMessage(name + " : " + tb_msg.Text);



            tb_msg.Clear();
            tb_msg.Text = "";
        }

        void SendMessage(string data)
        {

            try
            {
                chattingList.Items.Add(data);
            }
            catch
            {

            }
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

                listen_thread = new Thread(Listen);
                listen_thread.Start();

            }
            catch (Exception e)
            {
                MessageBox.Show("에러가 발생하였습니다. : " + e.ToString());
            }
            //c++하셨다면 try / catch 구문이야 잘 아시겠죠 전 잘모름.
        }

        void Listen()
        {
            while (true)
            {
                Socket client_socket = listen_socket.Accept();
                //클라이언트가 접속하지 않으면 서버는 이곳에서 디버깅이 멈춰져 있습니다. 클라이언트가 접속할 시에
                //아래 문단부터 다시 돌아가게 되지요 ㅎㅎ
                connectedClients.Add(client_socket);

                string client_address = client_socket.RemoteEndPoint.ToString();

                //IPEndPoint ip = (IPEndPoint)client_socket.LocalEndPoint;
                //string client_address = ip.Address.ToString();


                SendMessage("[" + client_address + "] 님이 참여하셨습니다.");

                isConnected = true;
                Thread receive_thread = new Thread(delegate(){
                    do_receive(client_socket);
                });
                receive_threads.Add(receive_thread);

 //이 줄과 밑에 줄은 쓰레드입니다. 즉
                //글자를 받는 것이라고 보시면 되겠습니다. 클라이언트로부터.
                receive_thread.Start();
            }

        }


        void do_receive(Socket client_socket)//do receive 함수입니다. 이 함수가 채팅서버의 결정적인 함수입니다.
        {
            while (isConnected)
            {

                // string client_address = client_socket.RemoteEndPoint.ToString();
                //string client_address = "";
                while (true)
                {
                    byte[] bytes = new byte[1024];  //바이트 배열 선언
                    int bytesRec = client_socket.Receive(bytes);


                    data += Encoding.UTF8.GetString(bytes, 0, bytesRec);  //인코딩과 GetString
                    if (data.IndexOf("<eof>") > -1)  //Index Of  의 그것입니다 숫자 0 아닙니다.
                        break;
                }
                data = data.Substring(0, data.Length - 5);
                Invoke((MethodInvoker)delegate
                {
                    //chattingList.Items.Add(data);  //이 부문은 listBox로 우리가 위에서 만든UI에 클라이언트로부터
                    //받아온 글자 데이터를 뿌려주는 역할을 합니다.
                    // for을 통해 "역순"으로 클라이언트에게 데이터를 보낸다.
                    SendMessage(data);
                }
                );
                data = "";  //한 번 채팅을 치고 보내기를 누르면 그 글자는 사라져야겠죠? 그 부분입니다.
            }
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            if (receive_threads != null)
            {
                foreach(Thread receive_thread in receive_threads){
                    receive_thread.Abort();
                }
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


        public static string Client_IP
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
    }
}
