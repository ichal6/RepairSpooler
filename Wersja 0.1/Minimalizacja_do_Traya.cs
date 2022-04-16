using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Drawing;

namespace Repair_Spooler
{


    public class TrayClass
    {
        private MainWindow mainWindow;

        private ContextMenu podręczne_menu;
        private NotifyIcon ni;
        

        public TrayClass(MainWindow mainWindow, string opis)
        {
            this.mainWindow = mainWindow;
            /* */
            ni = new NotifyIcon();

           

            Stream iconStream = System.Windows.Application.GetResourceStream(new Uri("/ikona1.ico", UriKind.Relative)).Stream;
            ni.Icon = new System.Drawing.Icon(iconStream);
            ni.Text = opis;
            ni.Visible = true;

            ni.DoubleClick += TrayOpen_Click;

            /* menu */
            podręczne_menu = new ContextMenu();
            podręczne_menu.MenuItems.Add(0,
                new MenuItem("Otwórz", new System.EventHandler(TrayOpen_Click)));
            podręczne_menu.MenuItems.Add(1,
                new MenuItem("Zakończ", new System.EventHandler(TrayExit_Click)));
            ni.ContextMenu = podręczne_menu;
        }

        public void ShowTrayInformation(string Title, string Content)
        {
            ni.BalloonTipTitle = Title;
            ni.BalloonTipText = Content;
            ni.BalloonTipIcon = ToolTipIcon.None;
            ni.Visible = true;
            ni.ShowBalloonTip(30000);

            ni.BalloonTipClicked += delegate(object sender, EventArgs args)
            {
                mainWindow.Show();
                mainWindow.Activate();
            };
        }

        private void TrayExit_Click(object sender, EventArgs e)
        {
            ZniszczTray();
            Environment.Exit(0);
        }

        private void TrayOpen_Click(object sender, EventArgs e)
        {
            mainWindow.Show();
            mainWindow.Activate();
            mainWindow.WindowState = WindowState.Normal;
        }

        public void ZniszczTray()
        {
            
            ni.Icon = null;
            ni.Dispose();
        }
    }
}
