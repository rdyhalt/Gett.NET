/*

  Copyright (c) 2016 Togocoder
 
  This program is to illustrate how to use Gett.NET library that uses the Ge.tt API Web Service, http://ge.tt/developers
  Please see copyright text in the source code for Gett.NET.

*/
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace GettTest
{
    public partial class UploadDownloadProgress : Form
    {
        private readonly bool _upload;
        private readonly string _fileName;
        private long _fileSize;
        private readonly Gett.Sharing.GettFile _file;
        private DateTime _started, _ended;

        public UploadDownloadProgress(bool upload, string fileName, Gett.Sharing.GettFile file)
        {
            _upload = upload;
            _fileName = fileName;
            _file = file;

            InitializeComponent();
        }

        private void UploadDownloadProgress_Load(object sender, EventArgs e)
        {
            Text = (_upload ? "Upload file " : "Download file ") + _file.Info.FileName;
            label_FileName.Text = _file.Info.FileName;

            if (_upload)
            {
                button_OpenFolder.Visible = false;
            }
        }

        private void UploadDownloadProgress_Shown(object sender, EventArgs e)
        {
            if (_upload)
            {
                try
                {
                    _file.UploadProgressChanged += File_UploadProgressChanged;
                    _file.UploadFileCompleted += File_UploadFileCompleted;

                    _started = DateTime.Now;
                    _fileSize = new System.IO.FileInfo(_fileName).Length;
                    _file.UploadFileEvents(_fileName);
                }
                catch (Exception ex)
                {
                    _file.UploadProgressChanged -= File_UploadProgressChanged;
                    _file.UploadFileCompleted -= File_UploadFileCompleted;

                    MessageBox.Show(@"Failed to upload new file '" + _fileName + @"' to Ge.tt" + Environment.NewLine + @"Exception of type " + ex.GetType() + Environment.NewLine + ex.Message);
                    Close();
                }
            }
            else
            {
                try
                {
                    _file.DownloadAsyncProgressChanged += File_DownloadAsyncProgressChanged;
                    _file.DownloadAsyncFileCompleted += File_DownloadAsyncFileCompleted;

                    _started = DateTime.Now;
                    _fileSize = _file.Info.Size;
                    _file.DownloadFileEvents(_fileName);
                }
                catch (Exception ex)
                {
                    _file.DownloadAsyncProgressChanged -= File_DownloadAsyncProgressChanged;
                    _file.DownloadAsyncFileCompleted -= File_DownloadAsyncFileCompleted;

                    MessageBox.Show(@"Failed to download '" + _file.Info.FileName + @"' from Ge.tt" + Environment.NewLine + @"Exception of type " + ex.GetType() + Environment.NewLine + ex.Message);
                    Close();
                }
            }
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            try
            {
                _file.CancelEvents();

                button_Cancel.Text = @"Close";
                button_Cancel.Click -= button_Cancel_Click;
                button_Cancel.Click += (obj, evt) => Close();
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch { }
        }

        private void UploadDownloadProgress_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                _file.CancelEvents();
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch { }

            if (_upload)
            {
                _file.UploadProgressChanged -= File_UploadProgressChanged;
                _file.UploadFileCompleted -= File_UploadFileCompleted;

            }
            else
            {
                _file.DownloadAsyncProgressChanged -= File_DownloadAsyncProgressChanged;
                _file.DownloadAsyncFileCompleted -= File_DownloadAsyncFileCompleted;
            }
        }

        #region Upload
        private void File_UploadFileCompleted(object sender, System.Net.UploadFileCompletedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => File_UploadFileCompleted(sender, e)));
                return;
            }

            _file.UploadProgressChanged -= File_UploadProgressChanged;
            _file.UploadFileCompleted -= File_UploadFileCompleted;

            _ended = DateTime.Now;
            TimeSpan totalTime = _ended - _started;
            long speed = (long)totalTime.TotalSeconds > 0 ? _fileSize / (long)totalTime.TotalSeconds : 0;
            progressBar_UploadDownload.Value = progressBar_UploadDownload.Maximum;

            label_Speed.Text = string.Format("Total time: {0}, speed was {1}/s", (DateTime.Now - _started).ToString(@"hh\:mm\:ss"), FormatSizeDisplay(speed));

            button_Cancel.Text = @"Close";
            button_Cancel.Click -= button_Cancel_Click;
            button_Cancel.Click += (obj, evt) => Close();

            if (e.Error != null)
            {
                MessageBox.Show(@"Failed to upload new file '" + _fileName + @"' to Ge.tt" + Environment.NewLine + @"Exception of type " + e.Error.GetType() + Environment.NewLine + e.Error.Message);
            }
        }

        private void File_UploadProgressChanged(object sender, System.Net.UploadProgressChangedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => File_UploadProgressChanged(sender, e)));
                return;
            }

            long percentage = (100 * e.BytesSent) / e.TotalBytesToSend;
            label_Size.Text = string.Format("{2}% - Total: {1} - Uploaded: {0}", FormatSizeDisplay(e.BytesSent), FormatSizeDisplay(e.TotalBytesToSend), percentage);

            TimeSpan elapsedTime = DateTime.Now - _started;
            TimeSpan estimatedTime = TimeSpan.FromSeconds((e.TotalBytesToSend - e.BytesSent) / (e.BytesSent / elapsedTime.TotalSeconds));
            if (estimatedTime.Seconds > 0 && estimatedTime.Seconds % 2 == 0)
            {
                label_Speed.Text = string.Format("Estimated time:{0}", estimatedTime.ToString(@"hh\:mm\:ss"));
            }

            try
            {
                progressBar_UploadDownload.Value = (int)percentage;
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch { }
        }
        #endregion

        #region Download
        private void File_DownloadAsyncFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => File_DownloadAsyncFileCompleted(sender, e)));
                return;
            }

            _file.DownloadAsyncProgressChanged -= File_DownloadAsyncProgressChanged;
            _file.DownloadAsyncFileCompleted -= File_DownloadAsyncFileCompleted;

            _ended = DateTime.Now;
            TimeSpan totalTime = _ended - _started;
            long speed = (long)totalTime.TotalSeconds > 0 ? _fileSize / (long)totalTime.TotalSeconds : 0;
            progressBar_UploadDownload.Value = progressBar_UploadDownload.Maximum;

            label_Speed.Text = string.Format("Total time: {0}, speed was {1}/s", (DateTime.Now - _started).ToString(@"hh\:mm\:ss"), FormatSizeDisplay(speed));

            button_Cancel.Text = @"Close";
            button_Cancel.Click -= button_Cancel_Click;
            button_Cancel.Click += (obj, evt) => Close();

            if (e.Error != null)
            {
                MessageBox.Show(@"Failed to download '" + _file.Info.FileName + @"' from Ge.tt" + Environment.NewLine + @"Exception of type " + e.Error.GetType() + Environment.NewLine + e.Error.Message);
            }
        }

        private void File_DownloadAsyncProgressChanged(object sender, System.Net.DownloadProgressChangedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => File_DownloadAsyncProgressChanged(sender, e)));
                return;
            }

            long percentage = (100 * e.BytesReceived) / e.TotalBytesToReceive;
            label_Size.Text = string.Format("{2}% - Total: {1} - Download: {0}", FormatSizeDisplay(e.BytesReceived), FormatSizeDisplay(e.TotalBytesToReceive), percentage);

            TimeSpan elapsedTime = DateTime.Now - _started;
            TimeSpan estimatedTime = TimeSpan.FromSeconds((e.TotalBytesToReceive - e.BytesReceived) / (e.BytesReceived / elapsedTime.TotalSeconds));
            if (estimatedTime.Seconds > 0 && estimatedTime.Seconds % 2 == 0)
            {
                label_Speed.Text = string.Format("Estimated time:{0}", estimatedTime.ToString(@"hh\:mm\:ss"));
            }

            try
            {
                progressBar_UploadDownload.Value = (int)percentage;
            }
// ReSharper disable once EmptyGeneralCatchClause
            catch { }
        }

        private void button_OpenFolder_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("explorer.exe", "/select, " + _fileName);
            }
// ReSharper disable once EmptyGeneralCatchClause
            catch { }
        }
        #endregion

        public static string FormatSizeDisplay(long bytes)
        {
            const int scale = 1024;
            string[] orders = { "PB", "TB", "GB", "MB", "KB", "B" };
            var max = (long)Math.Pow(scale, orders.Length - 1);
            foreach (string order in orders)
            {
                if (bytes > max)
                    return string.Format("{0:##.##} {1}", decimal.Divide(bytes, max), order);
                max /= scale;
            }
            return "0 B";
        }
    }
}
