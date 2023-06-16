using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace WindowsFromAppDownload
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        WebClient webClient;

        private void btnDownload_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(this.txtUrl.Text))
                {

                    pbDownload.Value = 0;
                    lbStatus.Text = "Downloaded 0%";
                    lbStatus.Update();

                    Thread thread = new Thread(() =>
                    {
                        Uri uri = new Uri(this.txtUrl.Text);

                        string fileName = System.IO.Path.GetFileName(uri.AbsolutePath);
                        webClient.DownloadFileAsync(uri, Application.StartupPath + "/mm/" + fileName);
                    });

                    thread.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            webClient = new WebClient();
            webClient.DownloadProgressChanged += webClient_DownloadProgressChanged;
            webClient.DownloadFileCompleted += webClient_DownloadFileCompleted;
        }

        void webClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            MessageBox.Show("Downloaded complete!");
        }

        void webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Invoke(new MethodInvoker(delegate(){
                pbDownload.Minimum = 0;
                double receive = double.Parse(e.BytesReceived.ToString());
                double total = double.Parse(e.TotalBytesToReceive.ToString());

                double persentage = receive / total * 100;

                pbDownload.Value = int.Parse(Math.Truncate(persentage).ToString());

                lbStatus.Text = "Downloaded " + persentage.ToString() + " %";
                lbStatus.Update();

            }));
        }

    }
}
