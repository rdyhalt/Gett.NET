/*

  Copyright (c) 2016 Togocoder
 
  This program is to illustrate how to use Gett.NET library that uses the Ge.tt API Web Service, http://ge.tt/developers
  Please see copyright text in the source code for Gett.NET.

*/
using System;
using System.Windows.Forms;

namespace GettTest
{
  static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new GettSharingForm());
    }
  }
}
