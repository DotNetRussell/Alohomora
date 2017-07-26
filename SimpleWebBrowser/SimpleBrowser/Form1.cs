using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace SimpleBrowser
{
    public partial class Form1 : Form
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();
        public Form1(string url, bool doscan = false)
        {
            InitializeComponent();
            try
            {
                AllocConsole();
                this.webBrowser1.Navigate(url);

                while (this.webBrowser1.ReadyState != WebBrowserReadyState.Complete)
                {
                    System.Windows.Forms.Application.DoEvents();
                }
                if (doscan)
                {
                    DoFBScan();
                }
                else
                {
                    Console.WriteLine(this.webBrowser1.Document.Body.InnerHtml);
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            
                //nom nom
            }

            Environment.Exit(0);
        }

        private void DoFBScan()
        {
            DateTime startTime = DateTime.Now;
            while (this.webBrowser1 == null || this.webBrowser1.Document == null
                || this.webBrowser1.Document.GetElementById("BrowseResultsContainer") == null)
            {
                if ((DateTime.Now - startTime) > TimeSpan.FromSeconds(3))
                {
                   
                    break;
                }
                System.Windows.Forms.Application.DoEvents();
                continue;
            }
            Console.WriteLine(this.webBrowser1.Document.Body.InnerHtml);
        }
    }
}
