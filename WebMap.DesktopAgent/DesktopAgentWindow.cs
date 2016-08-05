using System;
using System.Configuration;
using System.Windows.Forms;

namespace Mobilize
{
    public partial class DesktopAgentWindow : Form
    {


        public DesktopAgentWindow()
        {
            InitializeComponent();
            this.Text = ConfigurationManager.AppSettings["ManagerTitle"] ?? "WebMap Desktop Agent";
            var listeningMessage = ConfigurationManager.AppSettings["ManagerContent"] ?? "Listening on port {0}";
            this.label1.Text = string.Format(listeningMessage, Mobilize.DesktopAgent.agent_listening_port);
            this.Resize += DesktopAgentWindow_Resize;
            //Load all current plugins
            foreach (var plugin in DesktopAgent.Plugins)
            {
                var legend = new PluginLegend();
                legend.Legend = plugin.Key;
                this.flowLayoutPanel1.Controls.Add(legend);
            }
        }

        private void DesktopAgentWindow_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                systemTrayIcon.Visible = true;
                systemTrayIcon.BalloonTipText = "WebMap Client Interation minimized";
                systemTrayIcon.ShowBalloonTip(500);
                this.Hide();
            }

            else if (FormWindowState.Normal == this.WindowState)
            {
                //mynotifyicon.Visible = false;
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            Application.Exit();
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
           
        }
    }
}
