using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace VisualFizzler
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        internal static void ShowExceptionDialog(Exception e)
        {
            ShowExceptionDialog(e, null);
        }

        internal static void ShowExceptionDialog(Exception e, string title)
        {
            ShowExceptionDialog(e, title, null);
        }

        internal static void ShowExceptionDialog(Exception e, string title, IWin32Window owner)
        {
            Debug.Assert(e != null);

            Trace.WriteLine(e.ToString());

            if (!SystemInformation.UserInteractive)
                return;

            var message = new StringWriter();
            message.WriteLine(e.GetBaseException().Message);
            WriteTrace(e, message); // DEBUG only

            MessageBox.Show(owner, message.ToString(),
                string.IsNullOrEmpty(title) ? Application.ProductName : title,
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        [Conditional("DEBUG")]
        private static void WriteTrace(Exception e, TextWriter message)
        {
            message.WriteLine();
            message.WriteLine(e.ToString());
        }
    }
}
