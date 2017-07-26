using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleBrowser
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            string url = args[0];
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(url, args.Length == 2));
        }
    }
}
