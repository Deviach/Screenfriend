using ScreenFriend.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScreenFriend
{
   
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            Application.Run(new TaskbarContext());
        }
    }

    public class TaskbarContext : ApplicationContext
    {
        private NotifyIcon trayIcon;
        Form1 mainForm = new Form1();
        public TaskbarContext()
        {
            // Initialize Tray Icon
            trayIcon = new NotifyIcon()
            {
                Icon = new System.Drawing.Icon(@"./cameraicon.ico", 32, 32),
                ContextMenu = new ContextMenu(new MenuItem[] {
                new MenuItem("Show",Show),
                new MenuItem("Exit", Exit)
            }),
                Visible = true
            };
            trayIcon.ShowBalloonTip(4000, "ScreenFriend", "ScreenFriend is now running.",ToolTipIcon.Info);
        }

        void Exit(object sender, EventArgs e)
        {
            // Hide tray icon, otherwise it will remain shown until user mouses over it
            trayIcon.Visible = false;
            mainForm.Close();
            Application.Exit();
        }
        void Show(object sender,EventArgs e)
        {
            mainForm.Show();
        }
    }
}
