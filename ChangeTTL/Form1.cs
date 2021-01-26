using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Threading;

using Newtonsoft.Json.Linq;
using System.IO.Ports;
using System.IO;
using System.Collections;

namespace ChangeTTL
{

    public partial class Form1 : Form
    {
        string adrSelected;
        string Data;
        string Data1;
        string Data2;
        string Data3;
        string Data4;
        int rec_lend;
        int ind;
        byte[] rxbyte;
        byte[] rec_msg;
        byte TSCRIPT_GATWAY_DIR_RSP = 145;
        bool f_datain = false;
        bool st_msg = false;
        byte[] unicast = new byte[4];
        byte[] opcode = new byte[2];
        bool btnclick = false;
        int xx = 0;
        byte[] iset2 = new byte[2];
        
        int demtime = 0;
        byte[] v = { 0xe8, 0xff, 0x00, 0x00, 0x00, 0x00, 0x02, 0x01, 0x00, 0x00, 0x80, 0x0D, 0x00 };
        byte[] h = { 0xe8, 0xff, 0x00, 0x00, 0x00, 0x00, 0x02, 0x01, 0x00, 0x00, 0x80, 0x24, 0x00 };
        byte[] u = { 0xe8, 0xff, 0x00, 0x00, 0x00, 0x00, 0x02, 0x01, 0x00, 0x00, 0x80, 0x27, 0x00, 0x15 };
        byte[] n = { 0xe8, 0xff, 0x00, 0x00, 0x00, 0x00, 0x02, 0x01, 0x00, 0x00, 0x80, 0x13, 0x00 };
        byte[] g = { 0xe8, 0xff, 0x00, 0x00, 0x00, 0x00, 0x02, 0x01, 0x00, 0x00, 0x80, 0x10, 0x00 };
        byte[] on = { 0xe8, 0xff, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0xff, 0xff, 0x82, 0x02, 0x10, 0x00 };
        byte[] off = { 0xe8, 0xff, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0xff, 0xff, 0x82, 0x02, 0x10, 0x00 };

        public Form1()
        {
            InitializeComponent();

        }
        int intlen = 0;
        public class Device
        {
            public int ADR { get; set; }
            public string MAINDEVICE { get; set; }
            public string SUBDEVICE { get; set; }
            public string POWER { get; set; }
        }
        public class JSON
        {
            public int list_device_count { get; set; }
            public List<Device> devices { get; set; }
        }
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
            demtime++;
            req_msg();
            btn_send_click();

        }

        private void btn_connect_Click(object sender, EventArgs e)
        {
            if (label1.Text == "Disconnect")
            {
                serialPort.PortName = cbb_COM.Text;
                serialPort.Open();
                btn_connect.Text = "Disconnect";
                label1.Text = "Connect";

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

        void btn_send_click()
        {
            if (btnclick == true && demtime % 2 == 0)
            {
                if (serialPort.IsOpen == true)
                {
                    if (tb_TTL.Text != "")
                    {
                        
                        if( xx < listView1.Items.Count)
                        {
                            int a = Convert.ToInt32(listView1.Items[xx].Text, 16);
                            int b = Convert.ToInt32(tb_TTL.Text, 16);
                            v[12] = (byte)b;
                            v[8] = (byte)a;
                            v[9] = (byte)(a >> 8);
                            foreach (byte x in v)
                            {
                                Data = Data + (x.ToString("X2")) + " ";
                            }

                            serialPort.Write(v, 0, 13);
                            xx++;
                        }

                       // tb_TTL.Text = null;
             
                        else
                        {
                            btnclick = false;
                            xx = 0;
                        }
                    }
                    if (tb_Transmit.Text != "")
                    {
                        for (int k = 0; k < listView1.Items.Count; k++)
                        {
                            int l = Convert.ToInt32(listView1.Items[k].Text, 16);
                            int p = Convert.ToInt32(tb_Transmit.Text, 16);
                            h[12] = (byte)p;
                            h[8] = (byte)l;
                            h[9] = (byte)(l >> 8);
                            foreach (byte x in h)
                            {
                                Data1 = Data1 + (x.ToString("X2")) + " ";

                            }
                            serialPort.Write(h, 0, 13);

                            tb_Transmit.Text = null;
                        }


                    }
                }
            }
        }
        private void btn_Send_Click(object sender, EventArgs e)
        {
            try
            {
                btnclick = true;

                if (serialPort.IsOpen == true)
                {
                /*    if (tb_TTL.Text != "")
                    {
                        for (int k = 0; k < listView1.Items.Count; k++)
                        { 
                                int a = Convert.ToInt32(listView1.Items[k].Text, 16);
                                int b = Convert.ToInt32(tb_TTL.Text, 16);
                                v[12] = (byte)b;
                                v[8] = (byte)a;
                                v[9] = (byte)(a >> 8);
                                foreach (byte x in v)
                                {
                                    Data = Data + (x.ToString("X2")) + " ";
                                }

                                serialPort.Write(v, 0, 13);
                       
                        }

                        tb_TTL.Text = null;

                    }
                    if (tb_Transmit.Text != "")
                    {
                        for (int k = 0; k < listView1.Items.Count; k++)
                        {
                            int l = Convert.ToInt32(listView1.Items[k].Text, 16);
                            int p = Convert.ToInt32(tb_Transmit.Text, 16);
                            h[12] = (byte)p;
                            h[8] = (byte)l;
                            h[9] = (byte)(l >> 8);
                            foreach (byte x in h)
                            {
                                Data1 = Data1 + (x.ToString("X2")) + " ";

                            }
                            serialPort.Write(h, 0, 13);

                            tb_Transmit.Text = null;
                        }


                    } */
                    if (tb_Relay.Text != "")
                    {
                        int a = Convert.ToInt32(adrSelected, 16);
                        int d = Convert.ToInt32(tb_Relay.Text, 16);
                        u[12] = (byte)d;
                        u[8] = (byte)a;
                        u[9] = (byte)(a >> 8);
                        foreach (byte x in u)
                        {
                            Data2 = Data2 + (x.ToString("X2")) + " ";
                        }
                        serialPort.Write(u, 0, 14);

                     //   tb_Relay.Text = "";

                    }
                    if (tb_Proxy.Text != "")
                    {
                        int a = Convert.ToInt32(adrSelected, 16);
                        int i = Convert.ToInt32(tb_Proxy.Text, 16);

                        n[12] = (byte)i;
                        n[8] = (byte)a;
                        n[9] = (byte)(a >> 8);

                        foreach (byte x in n)
                        {
                            Data3 = Data3 + (x.ToString("X2")) + " ";
                        }
                        serialPort.Write(n, 0, 13);

                     //   tb_Proxy.Text = "";

                    }
                    if (tb_Friend.Text != "")
                    {
                        int a = Convert.ToInt32(adrSelected, 16);
                        int j = Convert.ToInt32(tb_Friend.Text, 16);

                        g[12] = (byte)j;
                        g[8] = (byte)a;
                        g[9] = (byte)(a >> 8);

                        foreach (byte x in g)
                        {
                            Data4 = Data4 + (x.ToString("X2")) + " ";
                        }
                        serialPort.Write(g, 0, 13);

                      //  tb_Friend.Text = "";

                    }

                }
            }
            catch (Exception err)
            {
                MessageBox.Show("Error");
            }

        }
        Queue rspdata = new Queue();
        private void Data_Receive(object sender, SerialDataReceivedEventArgs e)
        {
            rec_lend = serialPort.BytesToRead;
           
            for (int i = 0; i < 2; i++)
            {
                Thread.Sleep(50);
                if (serialPort.BytesToRead == rec_lend) break; // neu rec_lend co gtri se ngat.
                rec_lend = serialPort.BytesToRead; // chua co gtri duoc ghi vao.
            }
            rxbyte = new byte[rec_lend];
            serialPort.Read(rxbyte, 0, rec_lend);
            for (int i = 0; i < rxbyte.Length; i++)
            {
                rspdata.Enqueue(rxbyte[i]);
            }
            f_datain = true;
        }
        private void req_msg()
        {

            byte[] header = new byte[3];
            byte[] buf_msg_lend = new byte[2];
            UInt16 msg_lend = 0;
            string REC_MSG = "";
            if (rspdata.Count != 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    header[i] = (byte)(rspdata.Dequeue());
                }
                for (int i = 0; i < 2; i++)
                {
                    buf_msg_lend[i] = header[i];
                }
                msg_lend = BitConverter.ToUInt16(header, 0);

                if (msg_lend <= rspdata.Count + 1)
                {
                    if (header[2] == TSCRIPT_GATWAY_DIR_RSP)
                    {
                        rec_msg = new byte[msg_lend - 1];
                        for (int i = 0; i < msg_lend - 1; i++)
                        {
                            rec_msg[i] = (byte)(rspdata.Dequeue());
                        }

                        pt_msg(rec_msg);

                        foreach (byte b in rec_msg)
                        {
                            REC_MSG = REC_MSG + (b.ToString("X2")) + " ";
                        }

                    }

                }
            }


        }
        private void pt_msg(byte[] msg)
        {
           // MessageBox.Show("load");
            switch (msg[0])
            {
                
                case 0x80:
                    break;

                case 0x81:
                   // MessageBox.Show("Load2");
                    string UNICAST = "";
                    string OPCODE = "";
                    string ISET1 = "";
                    string ISET2 = "";
                    for (int i = 0; i < 4; i++)
                    {
                        unicast[i] = msg[i + 1];
                    }
                    foreach (byte b in unicast)
                    {
                        UNICAST = UNICAST + b.ToString(("X2")) + " ";
                    }
                    for (int i = 0; i < 2; i++)
                    {
                        opcode[i] = msg[i + 5];
                    }
                    foreach (byte c in opcode)
                    {
                        OPCODE = OPCODE + (c.ToString("X2")) + " ";

                    }
                    this.Invoke((MethodInvoker)delegate
                    {
                        
                        if (OPCODE == "80 25 " || OPCODE == "80 14 " || OPCODE == "80 11 " || OPCODE == "80 0E ")
                        {
                            byte iset1 = 0x00;
                            iset1 = msg[7];
                            ISET1 = iset1.ToString();
                            //  textBox1.Text = textBox1.Text.Insert(0, UNICAST + OPCODE + "0" + ISET1 + Environment.NewLine);
                            textBox1.AppendText(UNICAST + OPCODE + "0"+ ISET1 + "\r\n");
                        }
                        if (OPCODE == "80 28 ")
                        {
                            for (int i = 0; i < 2; i++)
                            {
                                iset2[i] = msg[i + 7];

                            }
                            foreach (byte y in iset2)
                            {
                                ISET2 = ISET2 + (y.ToString("X2")) + " ";
                            }
                          //  textBox1.Text = textBox1.Text.Insert(0, UNICAST + OPCODE + ISET2 + Environment.NewLine);
                            textBox1.AppendText(UNICAST + OPCODE + ISET2 + "\r\n");
                        }
                    });

                    break;
            }
        }
        private void Form1_load(object sender, EventArgs e)
        {

            string obj_lst_device;

            using (StreamReader str_lst_device = File.OpenText("mesh_database.json"))
            {
                obj_lst_device = str_lst_device.ReadToEnd();
            }
            var lst_dv = JsonConvert.DeserializeObject<JSON>(obj_lst_device);
            for (int a = 1; a < lst_dv.devices.Count; a++)
            {
                ListViewItem item1 = new ListViewItem(Convert.ToString(lst_dv.devices[a].ADR, 16));
                item1.SubItems.AddRange(new string[] { lst_dv.devices[a].MAINDEVICE, lst_dv.devices[a].SUBDEVICE, lst_dv.devices[a].POWER });
                listView1.Items.Add(item1);
            }
        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if (listView1.Items[i].Selected)
                {
                    adrSelected = listView1.Items[i].Text;
                }
            }
        }

        private void btn_on_Click(object sender, EventArgs e)
        {
            serialPort.Write(on, 0, 13);
        }

        private void btn_off_Click(object sender, EventArgs e)
        {
            serialPort.Write(off, 0, 13);
        }
    }
}
