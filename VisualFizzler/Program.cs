#region Copyright and License
// 
// Fizzler - CSS Selector Engine for Microsoft .NET Framework
// Copyright (c) 2009 Atif Aziz, Colin Ramsay. All rights reserved.
// 
// This library is free software; you can redistribute it and/or modify it under 
// the terms of the GNU Lesser General Public License as published by the Free 
// Software Foundation; either version 3 of the License, or (at your option) 
// any later version.
// 
// This library is distributed in the hope that it will be useful, but WITHOUT 
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS 
// FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more 
// details.
// 
// You should have received a copy of the GNU Lesser General Public License 
// along with this library; if not, write to the Free Software Foundation, Inc., 
// 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA 
// 
#endregion

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
