namespace GettTest
{
  partial class GettSharingForm
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
            this.button_GettLogin = new System.Windows.Forms.Button();
            this.button_Download = new System.Windows.Forms.Button();
            this.button_RefreshShares = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.listBox_Shares = new System.Windows.Forms.ListBox();
            this.listBox_Files = new System.Windows.Forms.ListBox();
            this.button_RefreshFiles = new System.Windows.Forms.Button();
            this.label_GettMe = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button_Upload = new System.Windows.Forms.Button();
            this.button_Delete = new System.Windows.Forms.Button();
            this.label_UserStorage = new System.Windows.Forms.Label();
            this.button_DeleteShare = new System.Windows.Forms.Button();
            this.button_CreateShare = new System.Windows.Forms.Button();
            this.button_Setup = new System.Windows.Forms.Button();
            this.button_ChangeTitle = new System.Windows.Forms.Button();
            this.listBox_LiveEvents = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button_GettLogin
            // 
            this.button_GettLogin.Location = new System.Drawing.Point(12, 7);
            this.button_GettLogin.Name = "button_GettLogin";
            this.button_GettLogin.Size = new System.Drawing.Size(75, 23);
            this.button_GettLogin.TabIndex = 0;
            this.button_GettLogin.Text = "Login";
            this.button_GettLogin.UseVisualStyleBackColor = true;
            this.button_GettLogin.Click += new System.EventHandler(this.button_GettLogin_Click);
            // 
            // button_Download
            // 
            this.button_Download.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Download.Location = new System.Drawing.Point(592, 62);
            this.button_Download.Name = "button_Download";
            this.button_Download.Size = new System.Drawing.Size(75, 23);
            this.button_Download.TabIndex = 13;
            this.button_Download.Text = "Download";
            this.button_Download.UseVisualStyleBackColor = true;
            this.button_Download.Click += new System.EventHandler(this.button_Download_Click);
            // 
            // button_RefreshShares
            // 
            this.button_RefreshShares.Location = new System.Drawing.Point(357, 62);
            this.button_RefreshShares.Name = "button_RefreshShares";
            this.button_RefreshShares.Size = new System.Drawing.Size(75, 23);
            this.button_RefreshShares.TabIndex = 9;
            this.button_RefreshShares.Text = "Refresh";
            this.button_RefreshShares.UseVisualStyleBackColor = true;
            this.button_RefreshShares.Click += new System.EventHandler(this.button_RefreshShares_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Shares";
            // 
            // listBox_Shares
            // 
            this.listBox_Shares.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox_Shares.Location = new System.Drawing.Point(12, 91);
            this.listBox_Shares.Name = "listBox_Shares";
            this.listBox_Shares.Size = new System.Drawing.Size(420, 303);
            this.listBox_Shares.TabIndex = 5;
            this.listBox_Shares.SelectedIndexChanged += new System.EventHandler(this.listBox_Shares_SelectedIndexChanged);
            // 
            // listBox_Files
            // 
            this.listBox_Files.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox_Files.Location = new System.Drawing.Point(480, 91);
            this.listBox_Files.Name = "listBox_Files";
            this.listBox_Files.Size = new System.Drawing.Size(420, 485);
            this.listBox_Files.TabIndex = 11;
            // 
            // button_RefreshFiles
            // 
            this.button_RefreshFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_RefreshFiles.Location = new System.Drawing.Point(825, 62);
            this.button_RefreshFiles.Name = "button_RefreshFiles";
            this.button_RefreshFiles.Size = new System.Drawing.Size(75, 23);
            this.button_RefreshFiles.TabIndex = 15;
            this.button_RefreshFiles.Text = "Refresh";
            this.button_RefreshFiles.UseVisualStyleBackColor = true;
            this.button_RefreshFiles.Click += new System.EventHandler(this.button_RefreshFiles_Click);
            // 
            // label_GettMe
            // 
            this.label_GettMe.AutoSize = true;
            this.label_GettMe.Location = new System.Drawing.Point(93, 12);
            this.label_GettMe.Name = "label_GettMe";
            this.label_GettMe.Size = new System.Drawing.Size(70, 13);
            this.label_GettMe.TabIndex = 1;
            this.label_GettMe.Text = "Not loggedin.";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(477, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Files";
            // 
            // button_Upload
            // 
            this.button_Upload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Upload.Location = new System.Drawing.Point(511, 62);
            this.button_Upload.Name = "button_Upload";
            this.button_Upload.Size = new System.Drawing.Size(75, 23);
            this.button_Upload.TabIndex = 12;
            this.button_Upload.Text = "Upload";
            this.button_Upload.UseVisualStyleBackColor = true;
            this.button_Upload.Click += new System.EventHandler(this.button_Upload_Click);
            // 
            // button_Delete
            // 
            this.button_Delete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Delete.Location = new System.Drawing.Point(744, 62);
            this.button_Delete.Name = "button_Delete";
            this.button_Delete.Size = new System.Drawing.Size(75, 23);
            this.button_Delete.TabIndex = 14;
            this.button_Delete.Text = "Delete";
            this.button_Delete.UseVisualStyleBackColor = true;
            this.button_Delete.Click += new System.EventHandler(this.button_Delete_Click);
            // 
            // label_UserStorage
            // 
            this.label_UserStorage.AutoSize = true;
            this.label_UserStorage.Location = new System.Drawing.Point(93, 33);
            this.label_UserStorage.Name = "label_UserStorage";
            this.label_UserStorage.Size = new System.Drawing.Size(53, 13);
            this.label_UserStorage.TabIndex = 2;
            this.label_UserStorage.Text = "Storage...";
            // 
            // button_DeleteShare
            // 
            this.button_DeleteShare.Location = new System.Drawing.Point(276, 62);
            this.button_DeleteShare.Name = "button_DeleteShare";
            this.button_DeleteShare.Size = new System.Drawing.Size(75, 23);
            this.button_DeleteShare.TabIndex = 8;
            this.button_DeleteShare.Text = "Delete";
            this.button_DeleteShare.UseVisualStyleBackColor = true;
            this.button_DeleteShare.Click += new System.EventHandler(this.button_DeleteShare_Click);
            // 
            // button_CreateShare
            // 
            this.button_CreateShare.Location = new System.Drawing.Point(58, 62);
            this.button_CreateShare.Name = "button_CreateShare";
            this.button_CreateShare.Size = new System.Drawing.Size(75, 23);
            this.button_CreateShare.TabIndex = 6;
            this.button_CreateShare.Text = "Create";
            this.button_CreateShare.UseVisualStyleBackColor = true;
            this.button_CreateShare.Click += new System.EventHandler(this.button_CreateShare_Click);
            // 
            // button_Setup
            // 
            this.button_Setup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Setup.Location = new System.Drawing.Point(825, 7);
            this.button_Setup.Name = "button_Setup";
            this.button_Setup.Size = new System.Drawing.Size(75, 23);
            this.button_Setup.TabIndex = 3;
            this.button_Setup.Text = "Setup";
            this.button_Setup.UseVisualStyleBackColor = true;
            this.button_Setup.Click += new System.EventHandler(this.button_Setup_Click);
            // 
            // button_ChangeTitle
            // 
            this.button_ChangeTitle.Location = new System.Drawing.Point(139, 62);
            this.button_ChangeTitle.Name = "button_ChangeTitle";
            this.button_ChangeTitle.Size = new System.Drawing.Size(75, 23);
            this.button_ChangeTitle.TabIndex = 7;
            this.button_ChangeTitle.Text = "Change title";
            this.button_ChangeTitle.UseVisualStyleBackColor = true;
            this.button_ChangeTitle.Click += new System.EventHandler(this.button_ChangeTitle_Click);
            // 
            // listBox_LiveEvents
            // 
            this.listBox_LiveEvents.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox_LiveEvents.FormattingEnabled = true;
            this.listBox_LiveEvents.Location = new System.Drawing.Point(12, 413);
            this.listBox_LiveEvents.Name = "listBox_LiveEvents";
            this.listBox_LiveEvents.Size = new System.Drawing.Size(420, 160);
            this.listBox_LiveEvents.TabIndex = 16;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 397);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "Live events";
            // 
            // GettSharingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(912, 582);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.listBox_LiveEvents);
            this.Controls.Add(this.button_ChangeTitle);
            this.Controls.Add(this.button_Setup);
            this.Controls.Add(this.button_CreateShare);
            this.Controls.Add(this.button_DeleteShare);
            this.Controls.Add(this.label_UserStorage);
            this.Controls.Add(this.button_Delete);
            this.Controls.Add(this.button_Upload);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label_GettMe);
            this.Controls.Add(this.button_RefreshFiles);
            this.Controls.Add(this.listBox_Files);
            this.Controls.Add(this.listBox_Shares);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_RefreshShares);
            this.Controls.Add(this.button_Download);
            this.Controls.Add(this.button_GettLogin);
            this.MinimumSize = new System.Drawing.Size(928, 620);
            this.Name = "GettSharingForm";
            this.Text = "List all shares and files";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GettSharingForm_FormClosing);
            this.Shown += new System.EventHandler(this.GettSharingForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button button_GettLogin;
    private System.Windows.Forms.Button button_Download;
    private System.Windows.Forms.Button button_RefreshShares;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ListBox listBox_Shares;
    private System.Windows.Forms.ListBox listBox_Files;
    private System.Windows.Forms.Button button_RefreshFiles;
    private System.Windows.Forms.Label label_GettMe;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Button button_Upload;
    private System.Windows.Forms.Button button_Delete;
    private System.Windows.Forms.Label label_UserStorage;
    private System.Windows.Forms.Button button_DeleteShare;
    private System.Windows.Forms.Button button_CreateShare;
    private System.Windows.Forms.Button button_Setup;
    private System.Windows.Forms.Button button_ChangeTitle;
    private System.Windows.Forms.ListBox listBox_LiveEvents;
    private System.Windows.Forms.Label label3;
  }
}

