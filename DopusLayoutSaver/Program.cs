using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DopusLayoutSaver
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var dopus = Process.GetProcessesByName("dopus");
            var layout = "Dev";

            if (dopus.Any())
            {
                //get paths
                var dopuspath = string.Empty;
                try
                {
                    dopuspath = dopus.First().MainModule.FileName;
                }
                catch(Win32Exception ex)
                {
                    MessageBox.Show(ex.Message, "Dopus Session Saver", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                } 
                var dopusdir = Path.GetDirectoryName(dopuspath);
                var layoutFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), $@"GPSoftware\Directory Opus\Layouts\{layout}.oll");
                
                //backup
                File.Copy(layoutFile, $"{layoutFile}.bak-{DateTime.Now:yyyy-MM-dd_hh-mm-ss-tt}");
                //save
                new Process
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        FileName = Path.Combine(dopusdir, "dopusrt.exe"),
                        Arguments = "/acmd Prefs LAYOUTSAVE=updatecurrent LAYOUTNAME=" + layout,
                        WorkingDirectory = dopusdir
                    }
                }.Start();
            }

        }
    }
}
