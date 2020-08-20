using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using YeelightAPI;

namespace LimeLight
{
    public partial class Form1 : Form
    {
        // Lamp handler
        public LampHandler lampHandler;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Set up lamp handler
            lampHandler = new LampHandler();
        }

        // Button changes lamp handler status
        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "START")
            {
                textBox1.Enabled = false;
                lampHandler.ConnectLampAndStartUpdateLoop(textBox1.Text);
                button1.Text = "STOP";
            }

            else
            {
                textBox1.Enabled = true;
                lampHandler.DisconnectLampAndStopUpdateLoop();
                button1.Text = "START";
            }
        }
    }
}