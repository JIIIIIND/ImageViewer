using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace ImageViewer
{
    class ButtonControl
    {
        Program MainApp;

        GroupBox Group;
        RadioButton Time_1;
        RadioButton Time_2;
        RadioButton Time_3;
        RadioButton Time_4;
        TextBox Self_Time;
        TableLayoutPanel tlp;

        GroupBox Dither;
        RadioButton Dither_1;
        RadioButton Dither_2;
        RadioButton Dither_3;
        RadioButton Dither_4;



        public ButtonControl(Program main)
        {
            MainApp = main;

            Group = new GroupBox();
            Group.Name = "Group";

            tlp = new TableLayoutPanel();
            tlp.ColumnCount = 2;
            tlp.RowCount = 4;
            tlp.Dock = DockStyle.Fill;

            Time_1 = new RadioButton();
            Time_1.Text = "1초";
            Time_1.Checked = true;
            Time_2 = new RadioButton();
            Time_2.Text = "2초";
            Time_3 = new RadioButton();
            Time_3.Text = "3초";
            Time_4 = new RadioButton();
            Time_4.Text = "시간 : ";
            Self_Time = new TextBox();
            Self_Time.Width = 50;
            Self_Time.MaxLength = 10000000;
            Self_Time.Dock = DockStyle.Right;
            Time_4.Controls.Add(Self_Time);

            tlp.Controls.Add(Time_1, 0, 0);
            tlp.Controls.Add(Time_2, 0, 1);
            tlp.Controls.Add(Time_3, 0, 2);
            tlp.Controls.Add(Time_4, 0, 3);

            Group.Text = "시간 조절";

            Group.Controls.Add(tlp);

            Self_Time.KeyPress += new KeyPressEventHandler(Keypress_Number);
            Time_1.CheckedChanged += new EventHandler(RadioButton1_Click);
            Time_2.CheckedChanged += new EventHandler(RadioButton2_Click);
            Time_3.CheckedChanged += new EventHandler(RadioButton3_Click);
            Time_4.CheckedChanged += new EventHandler(RadioButton4_Click);
            Self_Time.TextChanged += new EventHandler(TextChange);
            MainApp.ClientSizeChanged += new EventHandler(ClientResize);

            SizeInitialize();

            MainApp.Controls.Add(Group);
        }

        private void ClientResize(object sender, EventArgs e)
        {
            SizeInitialize();
        }

        private void SizeInitialize()
        {
            Group.Width = (int)(MainApp.Width * 0.104);
            Group.Height = (int)(MainApp.Height * 0.14);
            Group.Top = (int)(MainApp.Height * 0.604);
            Group.Left = (int)(MainApp.Width * 0.042);
        }

        private void Keypress_Number(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
                e.Handled = true;
        }
        private void RadioButton1_Click(object sender, EventArgs e)
        {
            if (Time_1.Checked == true)
                ImageControl.Timer = 1000;
        }
        private void RadioButton2_Click(object sender, EventArgs e)
        {
            if (Time_2.Checked == true)
                ImageControl.Timer = 2000;
        }
        private void RadioButton3_Click(object sender, EventArgs e)
        {
            if (Time_3.Checked == true)
                ImageControl.Timer = 3000;
        }
        private void RadioButton4_Click(object sender, EventArgs e)
        {
            if (Time_4.Checked == true)
            {
                try
                {
                    int time = Int32.Parse(Self_Time.Text);
                    if (time < 200 || time > 10000000)
                    {
                        MessageBox.Show("텍스트창에 200~10000000사이의 숫자를 입력하세요.(1초 = 1000)");
                        Time_1.Checked = true;
                    }
                    else
                        ImageControl.Timer = time;
                }
                catch (FormatException error)
                {
                    MessageBox.Show("텍스트창에 200~10000000사이의 숫자를 입력하세요.(1초 = 1000)");
                    Time_1.Checked = true;
                }
            }
        }
        private void TextChange(object sender, EventArgs e)
        {
            if (Time_4.Checked == true && Self_Time.TextLength > 0)
            {
                int time = Int32.Parse(Self_Time.Text);
                if (time < 200 || time > 10000000)
                {
                    MessageBox.Show("텍스트창에 200~10000000사이의 숫자를 입력하세요.(1초 = 1000)");
                    Time_1.Checked = true;
                }
                else
                    ImageControl.Timer = time;
            }
            else
            {
                Time_1.Checked = true;
            }
        }
    }
}
