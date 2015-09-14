/*

  Copyright (c) 2014 Togocoder
 
  This program is to illustrate how to use Gett.NET library that uses the Ge.tt API Web Service, http://ge.tt/developers
  Please see copyright text in the source code for Gett.NET.

*/
using System;
using System.Windows.Forms;

namespace GettTest
{
    public partial class NewShareForm : Form
    {
        public Gett.Sharing.GettShare Share = null;
        public string Title;

        public NewShareForm()
        {
            InitializeComponent();
        }

        private void NewShareForm_Load(object sender, EventArgs e)
        {
            if (Share != null)
            {
                Text = @"Change title";
                button_Create.Text = @"Change";
                textBox_Title.Text = Share.Info.Title;
            }
        }

        private void button_Create_Click(object sender, EventArgs e)
        {
            Title = textBox_Title.Text.Trim();
        }

    }
}
