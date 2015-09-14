/*

  Copyright (c) 2014 Togocoder
 
  This program is to illustrate how to use Gett.NET library that uses the Ge.tt API Web Service, http://ge.tt/developers
  Please see copyright text in the source code for Gett.NET.

*/
using System;
using System.Linq;
using System.Windows.Forms;

namespace GettTest
{
    public partial class GettSharingForm : Form
    {
        /// <summary>
        /// Illustrate of work with the async/await i .NET 4.5
        /// </summary>
        public async void OtherWorkAsync()
        {
            try
            {
                // Login to Ge.tt
                var user = new Gett.Sharing.GettUser();
                await user.LoginAsync("myapikey", "mymail@local", "mypassword");

                // Create new share
                Gett.Sharing.GettShare share1 = await user.Shares.CreateShareAsync(DateTime.Now.ToString("s"));
                Gett.Sharing.GettFile file1 = await share1.CreateFileAsync("MyDataFile." + DateTime.Now.ToString("s") + ".txt");

                // Upload own string as a file to Ge.tt
                const string myDataString = "Hello from Gett.NET library with async/await";
                await file1.UploadDataAsync(System.Text.Encoding.UTF8.GetBytes(myDataString));

                // Download file as a string or ...
                byte[] content = await file1.DownloadDataAsync();
                string contentString = System.Text.Encoding.UTF8.GetString(content);

                if (myDataString == contentString)
                {
                    MessageBox.Show(@"It is a match!");
                }

                // Download content to a local file
                await file1.DownloadFileAsync(@"C:\Workspace\MyFileFromGett.txt");

                // Delete file or ...
                await file1.DestroyAsync();

                // Delete share and all files
                await share1.DestroyAsync();

            }
            catch (Exception ex)
            {
                MessageBox.Show(@"Exception" + ex);
            }
        }


        /// <summary>
        /// Illustrate of work...
        /// </summary>
        public void OtherWork()
        {
            // Login to Ge.tt
            var user = new Gett.Sharing.GettUser();
            user.Login("myapikey", "mymail@local", "mypassword");

            // Find specefic share
            Gett.Sharing.GettShare share1 = user.Shares.GetShare("9XwUVrB");

            // Create new file on share.
            Gett.Sharing.GettFile file1 = share1.CreateFile("mybackup.sql");

            // Start upload new file
            file1.UploadFile(@"d:\mysqlbackupdump\today.sql");

            // Get all shares with most files count in it ordered
            var shareWithMostfiles = from share in user.Shares.GetShares()
                                     where share.FilesInfo.Length > 0
                                     orderby share.FilesInfo.Length descending
                                     select share;

            // Get all shares with the largeste files
            var shareWithTheLargestFileSize = from share in user.Shares.GetShares()
                                              where share.FilesInfo.Length > 0
                                              orderby share.FilesInfo.Max(f => f.Size) descending
                                              select share;

            // Begin async upload of file
            Gett.Sharing.GettFile file2 = share1.CreateFile("contacts.sql");
            file2.BeginUploadFile(@"d:\mysqlbackupdump\contacts.sql", UploadFileCompleted, file2);

            // Begin async download of file
            file1.BeginDownloadFile(@"d:\download\today.sql", file1.EndDownloadFile, null);
        }

        /// <summary>
        /// Illustrate of more work...
        /// </summary>
        public void OtherWork2()
        {
            // Login to Ge.tt
            var user = new Gett.Sharing.GettUser();
            user.Login("myapikey", "mymail@local", "mypassword");

            // Create new share
            Gett.Sharing.GettShare share1 = user.Shares.CreateShare(DateTime.Now.ToString("s"));
            Gett.Sharing.GettFile file1 = share1.CreateFile("MyDataFile." + DateTime.Now.ToString("s") + ".txt");

            // Upload own string as a file to Ge.tt
            const string myDataString = "Hello from Gett.NET library";
            file1.UploadData(System.Text.Encoding.UTF8.GetBytes(myDataString));

            // Download file as a string or ...
            byte[] content = file1.DownloadData();
            string contentString = System.Text.Encoding.UTF8.GetString(content);

            if (myDataString == contentString)
            {
                MessageBox.Show(@"It is a match!");
            }

            // Download content to a local file
            file1.DownloadFile(@"C:\TEMP\MyFileFromGett.txt");

            // Delete file or ...
            file1.Destroy();

            // Delete share and all files
            share1.Destroy();
        }

        /// <summary>
        /// Example on Begin/End method
        /// </summary>
        private void UploadFileCompleted(IAsyncResult ar)
        {
            var file2 = (Gett.Sharing.GettFile)ar.AsyncState;
            bool success = file2.EndUploadFile(ar);
        }

        /// <summary>
        /// Main class, 1 instance pr. user login.
        /// </summary>
        readonly Gett.Sharing.GettUser _gettUser = new Gett.Sharing.GettUser(10);

        /// <summary>
        /// For use with Ge.tt Live API
        /// </summary>
        Gett.Sharing.Live.GettLive _gettLive;

        public GettSharingForm()
        {
            InitializeComponent();
        }

        private void GettSharingForm_Shown(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Properties.Settings.Default.ApiKey) || string.IsNullOrEmpty(Properties.Settings.Default.Email) || string.IsNullOrEmpty(Properties.Settings.Default.Password))
            {
                button_Setup.PerformClick();
            }
        }

        private void GettSharingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_gettLive != null)
            {
                _gettLive.Stop();
            }
        }

        /// <summary>
        /// Settings, apikey, email and password
        /// </summary>
        private void button_Setup_Click(object sender, EventArgs e)
        {
            var settings = new SettingsForm();

            if (settings.ShowDialog() == DialogResult.OK)
            {
                button_GettLogin.PerformClick();
            }
        }

        /// <summary>
        /// Login to Ge.tt.
        /// </summary>
        private async void button_GettLogin_Click(object sender, EventArgs e)
        {
            try
            {
                UseWaitCursor = true;

                await _gettUser.LoginAsync(Properties.Settings.Default.ApiKey, Properties.Settings.Default.Email, Properties.Settings.Default.Password);

                // Setup Live API ?
                if (!string.IsNullOrEmpty(Properties.Settings.Default.LiveSessionId))
                {
                    try
                    {
                        // Startup Live API
                        _gettLive = _gettUser.GetLive();
                        _gettLive.LiveFileEvent += lfe => Invoke(new Action(() => listBox_LiveEvents.Items.Add(string.Format("{0}: Type:{1} - Share:{2} - FileName:{3}", lfe.Timestamp, lfe.Type, lfe.ShareName, lfe.FileName))));
                        _gettLive.Start(Properties.Settings.Default.LiveSessionId, true);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(@"Failed to connect to Live API on Ge.tt" + Environment.NewLine + @"Exception of type " + ex.GetType() + Environment.NewLine + ex.Message);
                    }
                }
                else
                {
                    label3.Visible = false;
                    listBox_LiveEvents.Visible = false;
                    listBox_Shares.Size = listBox_Files.Size;
                }

                button_RefreshShares.PerformClick();
            }
            catch (Exception ex)
            {
                label_GettMe.Text = @"Not loggedin.";
                label_UserStorage.Text = @"Storage...";
                listBox_Shares.Items.Clear();
                listBox_Files.Items.Clear();

                MessageBox.Show(@"Failed to login on Ge.tt, check apiKey, email or password" + Environment.NewLine + @"Exception of type " + ex.GetType() + Environment.NewLine + ex.Message);
            }
            finally
            {
                UseWaitCursor = false;
            }
        }

        #region Shares
        /// <summary>
        /// Create new share
        /// </summary>
        private async void button_CreateShare_Click(object sender, EventArgs e)
        {
            var newShare = new NewShareForm();

            if (newShare.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    UseWaitCursor = true;
                    Gett.Sharing.GettShare share = await _gettUser.Shares.CreateShareAsync(newShare.Title);
                    listBox_Shares.Items.Add(share);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(@"Failed to create new share at Ge.tt" + Environment.NewLine + @"Exception of type " + ex.GetType() + Environment.NewLine + ex.Message);
                }
                finally
                {
                    UseWaitCursor = false;
                }
            }
        }

        /// <summary>
        /// Change title
        /// </summary>
        private async void button_ChangeTitle_Click(object sender, EventArgs e)
        {
            if (listBox_Shares.SelectedItem is Gett.Sharing.GettShare)
            {
                var share = (Gett.Sharing.GettShare)listBox_Shares.SelectedItem;

                var changeTitleShare = new NewShareForm { Share = share };

                if (changeTitleShare.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        UseWaitCursor = true;
                        await share.SetTitleAsync(changeTitleShare.Title);
                        button_RefreshShares.PerformClick();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(@"Failed to change title for share at Ge.tt" + Environment.NewLine + @"Exception of type " + ex.GetType() + Environment.NewLine + ex.Message);
                    }
                    finally
                    {
                        UseWaitCursor = false;
                    }
                }
            }
        }

        /// <summary>
        /// Delete Share
        /// </summary>
        private async void button_DeleteShare_Click(object sender, EventArgs e)
        {
            if (listBox_Shares.SelectedItem is Gett.Sharing.GettShare)
            {
                var share = (Gett.Sharing.GettShare)listBox_Shares.SelectedItem;

                if (MessageBox.Show(@"Delete share '" + (string.IsNullOrEmpty(share.Info.Title) ? share.Info.ShareName : share.Info.Title) + @"' ?", @"Delete share", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    try
                    {
                        UseWaitCursor = true;
                        await share.DestroyAsync();
                        listBox_Shares.Items.Remove(share);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(@"Failed to delete '" + (string.IsNullOrEmpty(share.Info.Title) ? share.Info.ShareName : share.Info.Title) + @"' from Ge.tt" + Environment.NewLine + @"Exception of type " + ex.GetType() + Environment.NewLine + ex.Message);
                    }
                    finally
                    {
                        UseWaitCursor = false;
                    }
                }
            }
        }

        private async void button_RefreshShares_Click(object sender, EventArgs e)
        {
            try
            {
                UseWaitCursor = true;

                await _gettUser.RefreshMeAsync();
                label_GettMe.Text = string.Format("Email:{0} ({1})", _gettUser.Me.Email, _gettUser.Me.FullName);
                label_UserStorage.Text = string.Format("You are using {0} of {1}, free {2}.", UploadDownloadProgress.FormatSizeDisplay(_gettUser.Me.Storage.Used), UploadDownloadProgress.FormatSizeDisplay(_gettUser.Me.Storage.Total), UploadDownloadProgress.FormatSizeDisplay(_gettUser.Me.Storage.Free));

                listBox_Files.Items.Clear();
                listBox_Shares.Items.Clear();
                // ReSharper disable once CoVariantArrayConversion
                listBox_Shares.Items.AddRange(_gettUser.Shares.GetShares());
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"Failed to refresh shares from Ge.tt" + Environment.NewLine + @"Exception of type " + ex.GetType() + Environment.NewLine + ex.Message);
            }
            finally
            {
                UseWaitCursor = false;
            }
        }

        private void listBox_Shares_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox_Files.Items.Clear();
            if (listBox_Shares.SelectedItem is Gett.Sharing.GettShare)
            {
                var share = (Gett.Sharing.GettShare)listBox_Shares.SelectedItem;
                // ReSharper disable once CoVariantArrayConversion
                listBox_Files.Items.AddRange(share.Files);
            }
        }
        #endregion

        #region Files
        private void button_RefreshFiles_Click(object sender, EventArgs e)
        {
            listBox_Files.Items.Clear();
            if (listBox_Shares.SelectedItem is Gett.Sharing.GettShare)
            {
                var share = (Gett.Sharing.GettShare)listBox_Shares.SelectedItem;
                share.Refresh();
                // ReSharper disable once CoVariantArrayConversion
                listBox_Files.Items.AddRange(share.Files);
            }
        }

        private void button_Download_Click(object sender, EventArgs e)
        {
            if (listBox_Files.SelectedItem is Gett.Sharing.GettFile)
            {
                var file = (Gett.Sharing.GettFile)listBox_Files.SelectedItem;

                var saveDialog = new SaveFileDialog { OverwritePrompt = true, FileName = file.Info.FileName };

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    var download = new UploadDownloadProgress(false, saveDialog.FileName, file);
                    download.Show();
                }
            }
        }

        private async void button_Upload_Click(object sender, EventArgs e)
        {
            if (listBox_Shares.SelectedItem is Gett.Sharing.GettShare)
            {
                var openDialog = new OpenFileDialog { CheckFileExists = true };

                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    var share = (Gett.Sharing.GettShare)listBox_Shares.SelectedItem;

                    try
                    {
                        Gett.Sharing.GettFile file = await share.CreateFileAsync(System.IO.Path.GetFileName(openDialog.FileName), Properties.Settings.Default.LiveSessionId);
                        var upload = new UploadDownloadProgress(true, openDialog.FileName, file);
                        upload.Show();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(@"Failed to upload new file '" + openDialog.FileName + @"' to Ge.tt" + Environment.NewLine + @"Exception of type " + ex.GetType() + Environment.NewLine + ex.Message);
                    }
                }
            }
        }

        private async void button_Delete_Click(object sender, EventArgs e)
        {
            if (listBox_Files.SelectedItem is Gett.Sharing.GettFile)
            {
                var file = (Gett.Sharing.GettFile)listBox_Files.SelectedItem;

                if (MessageBox.Show(@"Delete file '" + file.Info.FileName + @"' ?", @"Delete file", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    try
                    {
                        await file.DestroyAsync();
                        listBox_Files.Items.Remove(file);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(@"Failed to delete '" + file.Info.FileName + @"' from Ge.tt" + Environment.NewLine + @"Exception of type " + ex.GetType() + Environment.NewLine + ex.Message);
                    }
                    finally
                    {
                        UseWaitCursor = false;
                    }
                }
            }
        }
        #endregion
    }
}
