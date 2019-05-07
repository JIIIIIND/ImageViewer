using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace ImageViewer
{
    class ImageControl
    {
        Program MainApp;
        PictureBox Picture;

        Thread Update;
        public Thread GetUpdate
        {
            get
            {
                return Update;
            }
        }

        public static string[] All_Img
        { get; set; }

        public static int Timer
        { get; set; }

        public static bool IsWork = true;
        public static bool ThreadSleep = false;
        private static bool fullsize;
        public static bool FullSize
        {
            get
            {
                return fullsize;
            }
            set
            {
                fullsize = value;
            }
        }

        public ImageControl(Program main)
        {
            MainApp = main;

            Picture = new PictureBox();
            Picture.BackColor = Color.Black;
            Picture.Name = "Picture";
            SizeInitialize();
            Picture.MouseClick += new MouseEventHandler(FullSizePicture);
            MainApp.ClientSizeChanged += new EventHandler(ClientResize);
            MainApp.FormClosing += new FormClosingEventHandler(close);

            Update = new Thread(new ThreadStart(ImgSetting));


            MainApp.Controls.Add(Picture);
        }
        private void close(object sender, FormClosingEventArgs e)
        {
            Update.Interrupt();
        }

        private void ClientResize(object sender, EventArgs e)
        {
            SizeInitialize();
        }

        private void SizeInitialize()
        {
            Picture.Width = (MainApp.Width / 13) * 10;
            Picture.Height = (MainApp.Height / 13) * 10;
            Picture.Top = (int)(MainApp.Height * 0.104);
            Picture.Left = (int)(MainApp.Width * 0.188);
            Picture.BorderStyle = BorderStyle.FixedSingle;
            Picture.SizeMode = PictureBoxSizeMode.Zoom;
        }

        public void ImgSetting()
        {
            Random rand = new Random();
            Timer = 1000;
            Delegate Imagefile = new ImageInsertHandler(ImageInsert);

            while (IsWork)
            {
                while (!ThreadSleep)
                {
                    try
                    {
                        string imgname = All_Img[rand.Next(0, All_Img.Length)];
                        if (Picture.InvokeRequired)
                        {
                            Picture.Invoke(Imagefile, imgname);
                        }
                        else
                        {
                            ImageInsert(imgname);
                        }
                        Thread.Sleep(Timer);
                        Picture.BackgroundImage.Dispose();
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        ThreadSleep = true;
                    }
                    catch (NullReferenceException e)
                    {
                        ThreadSleep = true;
                    }
                    catch (ThreadInterruptedException e)
                    {
                        ThreadSleep = true;
                        IsWork = false;
                    }
                    catch (FileNotFoundException e)
                    {
                        ThreadSleep = true;
                    }
                }
            }
        }
        private void ImageInsert(string img_name)
        {
            Picture.BackgroundImage = Image.FromFile(img_name);
            //Console.WriteLine(img_name);
            Picture.BackgroundImageLayout = ImageLayout.Zoom;
            
        }
        private delegate void ImageInsertHandler(string img_name);

        private void FullSizePicture(object sender, MouseEventArgs e)
        {
            Control[] controls;
            if (fullsize == false)
            {
                Console.WriteLine("Picture : {0}, {1} Main : {2}, {3}", Picture.Width, Picture.Height, MainApp.Width, MainApp.Height);
                MainApp.FormBorderStyle = FormBorderStyle.None;
                MainApp.Size = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                Picture.Width = Screen.PrimaryScreen.Bounds.Width;
                Picture.Height = Screen.PrimaryScreen.Bounds.Height;
                Console.WriteLine("Screen : {0}, {1}", Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                Console.WriteLine("Picture : {0}, {1} Main : {2}, {3}", Picture.Width, Picture.Height, MainApp.Width, MainApp.Height);
                Picture.Location = new Point(0, 0);
                Picture.Left = 0;
                Picture.Top = 0;
                Picture.SizeMode = PictureBoxSizeMode.Zoom;

                MainApp.Controls["DirTree"].Visible = false;
                MainApp.Controls["Group"].Visible = false;
                fullsize = true;
            }
            else
            {
                MainApp.FormBorderStyle = FormBorderStyle.Sizable;
                SizeInitialize();

                MainApp.Controls["DirTree"].Visible = true;
                MainApp.Controls["Group"].Visible = true;

                fullsize = false;
            }
        }
    }
}