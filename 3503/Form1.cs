using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _3503
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_server_Click(object sender, EventArgs e)
        {
            Form2 serverForm = new Form2();
            serverForm.Show();

        }

        private void btn_client_Click(object sender, EventArgs e)
        {
            Form3 clientForm = new Form3();
            clientForm.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
              Application.Exit();

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
