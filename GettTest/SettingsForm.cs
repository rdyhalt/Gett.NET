using System;
using System.Windows.Forms;

namespace GettTest
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        private void button_Save_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
        }
    }
}
