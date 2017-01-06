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
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                MessageBox.Show("Usage arguments:\nDopusLayoutSaver.exe <LayoutName> [<MaxBackupCount>]", "Dopus Session Saver", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var dopus = Process.GetProcessesByName("dopus");
            var layout = args[0];
            int backups;
            int maxbackups = 10;
            if (args.Length == 2 && int.TryParse(args[1], out backups))
            {
                maxbackups = backups;
            }


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

                if (!File.Exists(layoutFile))
                {
                    MessageBox.Show("Cannot backup layout. No such layout exists: " + layout, "Dopus Session Saver", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //backup layout before saving
                File.Copy(layoutFile, $"{layoutFile}.bak-{DateTime.Now:yyyy-MM-dd_HH-mm-ss}");

                //save layout
                new Process
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        FileName = Path.Combine(dopusdir, "dopusrt.exe"),
                        Arguments = "/acmd Prefs LAYOUTSAVE=updatecurrent LAYOUTNAME=" + layout,
                        WorkingDirectory = dopusdir
                    }
                }.Start();

                //purge oldest backups
                var di = new DirectoryInfo(Path.GetDirectoryName(layoutFile));
                var files = di.GetFileSystemInfos(layout + ".oll.bak-*");
                var allentries = files.OrderBy(f => f.CreationTime).ToList();

                if (allentries.Count <= maxbackups) return;
                allentries.RemoveRange(allentries.Count - maxbackups, maxbackups);
                foreach (var info in allentries)
                {
                    File.Delete(info.FullName);
                }
            }

        }
    }
}
