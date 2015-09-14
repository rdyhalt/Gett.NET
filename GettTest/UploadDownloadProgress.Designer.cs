namespace GettTest
{
  partial class UploadDownloadProgress
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.progressBar_UploadDownload = new System.Windows.Forms.ProgressBar();
      this.label_FileName = new System.Windows.Forms.Label();
      this.label_Size = new System.Windows.Forms.Label();
      this.button_Cancel = new System.Windows.Forms.Button();
      this.label_Speed = new System.Windows.Forms.Label();
      this.button_OpenFolder = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // progressBar_UploadDownload
      // 
      this.progressBar_UploadDownload.Location = new System.Drawing.Point(12, 50);
      this.progressBar_UploadDownload.Name = "progressBar_UploadDownload";
      this.progressBar_UploadDownload.Size = new System.Drawing.Size(460, 23);
      this.progressBar_UploadDownload.Step = 1;
      this.progressBar_UploadDownload.TabIndex = 2;
      // 
      // label_FileName
      // 
      this.label_FileName.AutoSize = true;
      this.label_FileName.Location = new System.Drawing.Point(40, 10);
      this.label_FileName.Name = "label_FileName";
      this.label_FileName.Size = new System.Drawing.Size(49, 13);
      this.label_FileName.TabIndex = 0;
      this.label_FileName.Text = "Filename";
      // 
      // label_Size
      // 
      this.label_Size.AutoSize = true;
      this.label_Size.Location = new System.Drawing.Point(12, 34);
      this.label_Size.Name = "label_Size";
      this.label_Size.Size = new System.Drawing.Size(16, 13);
      this.label_Size.TabIndex = 1;
      this.label_Size.Text = "...";
      // 
      // button_Cancel
      // 
      this.button_Cancel.Location = new System.Drawing.Point(402, 95);
      this.button_Cancel.Name = "button_Cancel";
      this.button_Cancel.Size = new System.Drawing.Size(75, 23);
      this.button_Cancel.TabIndex = 5;
      this.button_Cancel.Text = "Cancel";
      this.button_Cancel.UseVisualStyleBackColor = true;
      this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
      // 
      // label_Speed
      // 
      this.label_Speed.AutoSize = true;
      this.label_Speed.Location = new System.Drawing.Point(9, 105);
      this.label_Speed.Name = "label_Speed";
      this.label_Speed.Size = new System.Drawing.Size(16, 13);
      this.label_Speed.TabIndex = 3;
      this.label_Speed.Text = "...";
      // 
      // button_OpenFolder
      // 
      this.button_OpenFolder.Location = new System.Drawing.Point(321, 95);
      this.button_OpenFolder.Name = "button_OpenFolder";
      this.button_OpenFolder.Size = new System.Drawing.Size(75, 23);
      this.button_OpenFolder.TabIndex = 4;
      this.button_OpenFolder.Text = "Open folder";
      this.button_OpenFolder.UseVisualStyleBackColor = true;
      this.button_OpenFolder.Click += new System.EventHandler(this.button_OpenFolder_Click);
      // 
      // UploadDownloadProgress
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(484, 124);
      this.Controls.Add(this.button_OpenFolder);
      this.Controls.Add(this.label_Speed);
      this.Controls.Add(this.button_Cancel);
      this.Controls.Add(this.label_Size);
      this.Controls.Add(this.label_FileName);
      this.Controls.Add(this.progressBar_UploadDownload);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.Name = "UploadDownloadProgress";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "UploadDownloadProgress";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UploadDownloadProgress_FormClosing);
      this.Load += new System.EventHandler(this.UploadDownloadProgress_Load);
      this.Shown += new System.EventHandler(this.UploadDownloadProgress_Shown);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ProgressBar progressBar_UploadDownload;
    private System.Windows.Forms.Label label_FileName;
    private System.Windows.Forms.Label label_Size;
    private System.Windows.Forms.Button button_Cancel;
    private System.Windows.Forms.Label label_Speed;
    private System.Windows.Forms.Button button_OpenFolder;
  }
}