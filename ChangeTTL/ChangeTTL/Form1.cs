using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO.Ports;
namespace ChangeTTL
{

    public partial class Form1 : Form
    {
        string Data;
        Int32 A = 0;
        byte[] s = { 0xe8, 0xff, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x80, 0x0D, 0x00 };
        byte[] q = { 0xe8, 0xff, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x80, 0x24, 0x00 };
        byte[] w = { 0xe8, 0xff, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x80, 0x27, 0x00, 0x15 };
        byte[] e = { 0xe8, 0xff, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x80, 0x13, 0x00 };
        byte[] r = { 0xe8, 0xff, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x80, 0x10, 0x00 };
        public Form1()
        {
            InitializeComponent();

        }
        int intlen = 0;

        private void timer1_Tick(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            if (intlen != ports.Length)
            {
                intlen = ports.Length;
                cbb_COM.Items.Clear();
                for (int j = 0; j < intlen; j++)
                {
                    cbb_COM.Items.Add(ports[j]);
                }
            }

            if (serialPort.IsOpen == true)
            {
                //  int b = Convert.ToInt32(s[8]);
                //  A = A + 2;

                //  int a = Convert.ToInt32(tb_TTL.Text, 16);
                //  s[12] = (byte)a;
                // serialPort.Write(s, 0, 13);
            }

        }

        private void btn_connect_Click(object sender, EventArgs e)
        {
            if (label1.Text == "Disconnect")
            {
                serialPort.PortName = cbb_COM.Text;
                serialPort.Open();
                btn_connect.Text = "Disconnect";
                label1.Text = "Connect";
                // tb_TTL.Enabled = false;
                // cbb_COM.Enabled = false;
            }
            else
            {
                serialPort.Close();
                btn_connect.Text = " Connect";
                label1.Text = "Disconnect";
                tb_TTL.Enabled = true;
                cbb_COM.Enabled = true;
            }

        }
        private void Data_Receive(object sender, SerialDataReceivedEventArgs e)
        {

        }

        private void btn_Send_Click(object sender, EventArgs e)
        {
            try
            {

                if (serialPort.IsOpen == true)
                {
                    if (tb_TTL.Text != null)
                    {
                        int a = Convert.ToInt32(tb_ADR.Text, 16);
                        int b = Convert.ToInt32(tb_TTL.Text, 16);
                        s[11] = (byte)b;
                        s[7] = (byte)a;
                        s[8] = (byte)(a >> 8);
                        foreach (byte x in s)
                        {
                            Data = Data + (x.ToString("X2")) + " ";
                        }
                        serialPort.Write(s, 0, 12);
                        tb_TTL.Text = "";
                    }
                    else if (tb_Transmit != null)
                    {
                        int c = Convert.ToInt32(tb_Transmit.Text, 16);
                        int a = Convert.ToInt32(tb_ADR.Text, 16);
                        
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("Error");
            }
        }
    }
}
