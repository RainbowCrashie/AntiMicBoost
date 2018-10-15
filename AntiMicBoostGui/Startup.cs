using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IWshRuntimeLibrary;
using Shell32;

namespace AntiMicBoostGui
{
    public static class Startup
    {
        public static void CreateStartupFolderShortcut()
        {
            var wshShell = new WshShellClass();
            IWshShortcut shortcut;
            var startUpFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);

            // Create the shortcut
            shortcut = (IWshShortcut)wshShell.CreateShortcut(startUpFolderPath + "\\" + Application.ProductName + ".lnk");

            shortcut.TargetPath = Application.ExecutablePath;
            shortcut.WorkingDirectory = Application.StartupPath;
            shortcut.Description = "Launch AntiMicBoost";
            shortcut.Save();
        }

        public static void DeleteStartupFolderShortcuts()
        {
            var startUpFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);

            var di = new DirectoryInfo(startUpFolderPath);
            var files = di.GetFiles("*.lnk");

            foreach (FileInfo fi in files)
            {
                var shortcutTargetFile = GetShortcutTargetFile(fi.FullName);
                
                if (shortcutTargetFile.EndsWith(Path.GetFileNameWithoutExtension(Application.ExecutablePath) + ".exe",
                      StringComparison.InvariantCultureIgnoreCase))
                {
                    System.IO.File.Delete(fi.FullName);
                }
            }
        }

        public static string GetShortcutTargetFile(string shortcutFilename)
        {
            var pathOnly = Path.GetDirectoryName(shortcutFilename);
            var filenameOnly = Path.GetFileName(shortcutFilename);

            var shell = new ShellClass();
            Shell32.Folder folder = shell.NameSpace(pathOnly);
            var folderItem = folder.ParseName(filenameOnly);
            if (folderItem != null)
            {
                ShellLinkObject link =
                  (ShellLinkObject)folderItem.GetLink;
                return link.Path;
            }

            return string.Empty; // Not found
        }

    }
}
