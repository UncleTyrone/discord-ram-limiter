using DiscordRamLimiter.CControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Management;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiscordRamLimiter
{
    public partial class Form1 : Form
    {
        [DllImport("kernel32.dll")]
        static extern bool SetProcessWorkingSetSize(IntPtr proc, int min, int max);
        static int min = -1;
        static int max = -1;
        static bool active = false;

        private void Form1_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(500);
                Hide();
            }
            else if (this.WindowState == FormWindowState.Normal)
            {
                notifyIcon1.Visible = false;
            }
        }

        private static void RamLimiter()
        {
            int DiscordId = -1;
            long workingSet = 0;
            foreach (Process discord in Process.GetProcessesByName("Discord"))
            {
                if (discord.WorkingSet64 > workingSet)
                {
                    workingSet = discord.WorkingSet64;
                    DiscordId = discord.Id;
                }
            }
            while (DiscordId != -1 && active == true)
            {
                if (DiscordId != -1 && active == true)
                {
                    GC.Collect(); // Force garbage collection
                    GC.WaitForPendingFinalizers(); // Wait for all finalizers to complete before continuing. 
                    if (Environment.OSVersion.Platform == PlatformID.Win32NT) // Check OS version platform 
                    {
                        SetProcessWorkingSetSize(Process.GetProcessById(DiscordId).Handle, min, max);
                    }
                    var wmiObject = new ManagementObjectSearcher("select * from Win32_OperatingSystem");
                    var memoryValues = wmiObject.Get().Cast<ManagementObject>().Select(mo => new {
                        FreePhysicalMemory = Double.Parse(mo["FreePhysicalMemory"].ToString()),
                        TotalVisibleMemorySize = Double.Parse(mo["TotalVisibleMemorySize"].ToString())
                    }).FirstOrDefault();
                    if (memoryValues != null)
                    {
                        var percent = ((memoryValues.TotalVisibleMemorySize - memoryValues.FreePhysicalMemory) / memoryValues.TotalVisibleMemorySize) * 100;
                        Thread.Sleep(300);
                    }
                    Thread.Sleep(1);
                }
            }
        }

        private void toggleButton1_CheckedChange(object sender, EventArgs e)
        {
            if (this.toggleButton1.Checked == true)
            {
                active = true;
                Thread RamLimiterThread = new Thread(RamLimiter);
                RamLimiterThread.Name = "RamLimiterThread";
                RamLimiterThread.IsBackground = true;
                RamLimiterThread.Start();
            }
            else
            {
                active = false;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }

        public Form1()
        {
            InitializeComponent();
        }

        void Exit(object sender, EventArgs e)
        {
            notifyIcon1.Visible = false;
            Application.Exit();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
