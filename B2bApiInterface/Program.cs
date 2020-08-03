using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Windows.Forms;

namespace B2bApiInterface
{
    static class Program
    {
        private static System.Threading.Mutex mutex;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            

           
            Application.EnableVisualStyles();

            Application.SetCompatibleTextRenderingDefault(false);
            mutex = new System.Threading.Mutex(true, "优药汇电商平台同步接口");
            if (mutex.WaitOne(0, false))
                Application.Run(new frmMain());
            else
                MessageBox.Show("程序已经在运行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
        }

        
    }
}
