using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace FireParted
{
    public partial class fmWarningDialog : Form
    {
        public fmWarningDialog()
        {
            InitializeComponent();

            txtWarningText.Text = "PLEASE NOTE: FireParted modifies the filesystem layout of your " +
                "Kindle Fire's internal memory. Although all attempts have been made to ensure that " +
                "FireParted is as safe as possible, there is always a small chance that an error could occur.\n\n" +
                "Please be sure you understand the risks of altering your device's partition layout before continuing!";
        }

        private void btnOkay_Click(object sender, EventArgs e)
        {
            if (chkSuppressWarning.Checked)
            {
                File.Create("nowarningdialog");
            }

            this.Close();
        }
    }
}
