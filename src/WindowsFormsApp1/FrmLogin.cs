﻿using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace WindowsFormsApp1
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
