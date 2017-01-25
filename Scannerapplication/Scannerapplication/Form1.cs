using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using WIATest;

namespace Scannerapplication
{
    public partial class Form1 : Form
    {
        const string Prefix = "http://+:8008/";
        static HttpListener Listener = null;
        int RequestNumber = 0;
        static byte[] GlobalImgFile = null;
        static HttpListenerContext contexts = null;
        //readonly DateTime StartupDate = DateTime.UtcNow;
        public Form1()
        {
            
            StartBrowserEventListener();
            InitializeComponent();
        }
        public void StartBrowserEventListener()
        {
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("HttpListener is not supported on this platform.");
                return;
            }
            using (Listener = new HttpListener())
            {
                Listener.Prefixes.Add(Prefix);
                Listener.Start();
                var context = Listener.GetContext();
                // Begin waiting for requests.
                Listener.BeginGetContext(GetContextCallback, context);
                Console.WriteLine("Listening. Press Enter to stop.");
                Console.ReadLine();
                Listener.Stop();
            }
        }
        static void GetContextCallback(IAsyncResult ar)
        {
            // HttpListener listenOn = (HttpListener)ar.AsyncState;
            contexts = (HttpListenerContext)ar.AsyncState;
            HttpListenerRequest request = contexts.Request;
            var a = contexts.Request.Url.Query;
           
            // contexts.Request.QueryString[""];
            
            
            
            //HttpWebResponse responseda = (HttpWebResponse)request.GetResponse();
            //int req = ++RequestNumber;

            // Get the context
            //var context = Listener.EndGetContext(ar);

            // listen for the next request
            //Listener.BeginGetContext(GetContextCallback, null);

            // get the request
            //var NowTime = DateTime.UtcNow;

            //Console.WriteLine("{0}: {1}", NowTime.ToString("R"), context.Request.RawUrl);

            //var responseString = string.Format("<html><body>Your request, \"{0}\", was received at {1}.<br/>It is request #{2:N0} since {3}.",
            //    context.Request.RawUrl, NowTime.ToString("R"), req, StartupDate.ToString("R"));

            //byte[] buffer = Encoding.UTF8.GetBytes(responseString);
            // and send it
            //var response = context.Response;
            //response.ContentType = "text/html";
            //response.ContentLength64 = buffer.Length;
            //response.StatusCode = 200;
            //response.OutputStream.Write(buffer, 0, buffer.Length);
            //response.OutputStream.Close();
        }
        //button click event
        private void btn_scan_Click(object sender, EventArgs e)
        {
            try
            {
                //get list of devices available
                List<string> devices = WIAScanner.GetDevices();

                foreach (string device in devices)
                {
                    lbDevices.Items.Add(device);
                }
                //check if device is not available
                if (lbDevices.Items.Count == 0)
                {
                    MessageBox.Show("You do not have any WIA devices.");
                    this.Close();
                }
                else
                {
                    lbDevices.SelectedIndex = 0;
                }
                //get images from scanner
                List<Image> images = WIAScanner.Scan((string)lbDevices.SelectedItem);
                foreach (Image image in images)
                {
                    pic_scan.Image = image;
                    pic_scan.Show();
                    pic_scan.SizeMode = PictureBoxSizeMode.StretchImage;
                    string name = @"D:\" + DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ".jpeg";
                    //save scanned image into specific folder
                    image.Save(name,ImageFormat.Jpeg);
                    //GlobalImgFile = File.ReadAllBytes(name);
                   // contexts.Response.Close(GlobalImgFile, true);
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }
        private void Home_SizeChanged(object sender, EventArgs e)
        {
            int pheight = this.Size.Height - 153;
            pic_scan.Size = new Size(pheight - 150, pheight);
        }

    }
}
