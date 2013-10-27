using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WordSearch
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        //[STAThread]
        static void Main()
        {
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //WordMatrixEngine wordme = new WordMatrixEngine();
            //wordme.Debug(6);
            EngineControl.Run();
            Application.Run(new Form1(EngineControl.resultMatrix));
        }
    }
}
