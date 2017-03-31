using ScreenFriend.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScreenFriend
{
   
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;
        private const int PRT_SCN = 44;
        static Form1 reference;
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form1 mainForm = new Form1();
            reference = mainForm;
            _hookID = SetHook(_proc);
           
            Application.Run(new TaskbarContext(mainForm));
            UnhookWindowsHookEx(_hookID);
        }
        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                if (vkCode == PRT_SCN)
                {
                    reference.addClipboard();
                }
                //Console.WriteLine((Keys)vkCode);
                //MessageBox.Show(vkCode.ToString());
            }

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

    }

    public class TaskbarContext : ApplicationContext
        {
            private NotifyIcon trayIcon;
            Form1 mainForm;
            public TaskbarContext(Form1 mainform)
            {
                mainForm = mainform;
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
