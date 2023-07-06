using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoWin.Plugin
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var info = new ProcessStartInfo
            {
                FileName = AppDomain.CurrentDomain.BaseDirectory + "/update.exe",
                Arguments = $"adl"
            };
            Process.Start(info);

            Environment.Exit(0);
        }
    }
}
