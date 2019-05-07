using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Threading;

namespace ImageViewer
{
    public class Program : Form
    {

        public static void Main(string[] args)
        {
            Program MainApp = new Program();
            MainApp.Size = new Size(Screen.PrimaryScreen.Bounds.Width,Screen.PrimaryScreen.Bounds.Height);
            MainApp.Location = new Point(0, 0);
            MainApp.MaximizeBox = false;
            MainApp.Text = "ImageRandomViewer";
            MainApp.FormClosing += new FormClosingEventHandler(MainApp.close);

            TreeControl Tree = new TreeControl(MainApp);
            ButtonControl Button = new ButtonControl(MainApp);
            ImageControl Image = new ImageControl(MainApp);

            Image.GetUpdate.Start();
            Application.Run(MainApp);
            Image.GetUpdate.Join();
        }

        private void close(object sender, FormClosingEventArgs e)
        {
            ImageControl.ThreadSleep = true;
            ImageControl.IsWork = false;
        }
    }
}
