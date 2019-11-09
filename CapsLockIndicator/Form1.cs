using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.Management.Instrumentation;
using System.Collections.Specialized;
using System.Threading;

namespace CapsLockIndicator
{
    public partial class Form1 : Form
    {

        NotifyIcon capsLockIcon;
        Icon activeIcon;
        Icon idleIcon;
        Thread capsLockWorker;

        public Form1()
        {
            InitializeComponent();

            //Load icons from files into objects
            activeIcon = new Icon("Caps_Active.ico");
            idleIcon = new Icon("Caps_Idle.ico");

            //Create notify icons and assign idle icon and show it
            capsLockIcon = new NotifyIcon();
            capsLockIcon.Icon = idleIcon;
            capsLockIcon.Visible = true;

            //Create all context menu items and add them to notification tray icon
            MenuItem progNameMenuItem = new MenuItem("Caps Lock Indicator v1.2 by Ben Hawthorn");
            MenuItem quitMenuItem = new MenuItem("Quit");
            ContextMenu contextMenu = new ContextMenu();
            contextMenu.MenuItems.Add(progNameMenuItem);
            contextMenu.MenuItems.Add(quitMenuItem);
            capsLockIcon.ContextMenu = contextMenu;

            //Wire up quit button to close application
            quitMenuItem.Click += QuitMenuItem_Click;

            //Hide the form because we don't need it, this is a notification tray application
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;

            //Start worker thread that pulls caps lock status
            capsLockWorker = new Thread(new ThreadStart(CapsLockThread));
            capsLockWorker.Start();

        }

        private void QuitMenuItem_Click(object sender, EventArgs e)
        {
            //Close the application on click of 'Quit' button on context menu
            capsLockIcon.Dispose();
            this.Close();
        }

        //This is the thread that pulls the caps lock status and updates the notification icon
        public void CapsLockThread()
        {
            try
            {
                //Main loop where all the magic happens
                while(true)
                {
                    if (Control.IsKeyLocked(Keys.CapsLock))
                        capsLockIcon.Icon = activeIcon;
                    else
                        capsLockIcon.Icon = idleIcon;

                    Thread.Sleep(100);
                }
            } catch(ThreadAbortException tbe)
            {

            }
            

        }
    }
}
