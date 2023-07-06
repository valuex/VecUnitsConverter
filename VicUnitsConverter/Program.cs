using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VecUnitsConverter
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // convert multiple args into one
            string sFullArgs = "";
            for (int i = 0; i < args.Length; i++)
            {
                sFullArgs= sFullArgs+args[i]+" ";
            }
            sFullArgs= sFullArgs.Trim();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(sFullArgs));
        }
    }
}
