namespace GettTest
{
  partial class SettingsForm
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
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.button_Save = new System.Windows.Forms.Button();
      this.button_Cancel = new System.Windows.Forms.Button();
      this.textBox_Password = new System.Windows.Forms.TextBox();
      this.textBox_Email = new System.Windows.Forms.TextBox();
      this.textBox_ApiKey = new System.Windows.Forms.TextBox();
      this.textBox_LiveAPISessionId = new System.Windows.Forms.TextBox();
      this.label4 = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(12, 9);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(45, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "API Key";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(12, 48);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(32, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "Email";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(12, 87);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(53, 13);
      this.label3.TabIndex = 4;
      this.label3.Text = "Password";
      // 
      // button_Save
      // 
      this.button_Save.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.button_Save.Location = new System.Drawing.Point(281, 12);
      this.button_Save.Name = "button_Save";
      this.button_Save.Size = new System.Drawing.Size(75, 23);
      this.button_Save.TabIndex = 8;
      this.button_Save.Text = "Save";
      this.button_Save.UseVisualStyleBackColor = true;
      this.button_Save.Click += new System.EventHandler(this.button_Save_Click);
      // 
      // button_Cancel
      // 
      this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.button_Cancel.Location = new System.Drawing.Point(281, 41);
      this.button_Cancel.Name = "button_Cancel";
      this.button_Cancel.Size = new System.Drawing.Size(75, 23);
      this.button_Cancel.TabIndex = 9;
      this.button_Cancel.Text = "Cancel";
      this.button_Cancel.UseVisualStyleBackColor = true;
      // 
      // textBox_Password
      // 
      this.textBox_Password.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::GettTest.Properties.Settings.Default, "Password", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.textBox_Password.Location = new System.Drawing.Point(15, 103);
      this.textBox_Password.Name = "textBox_Password";
      this.textBox_Password.Size = new System.Drawing.Size(250, 20);
      this.textBox_Password.TabIndex = 5;
      this.textBox_Password.Text = global::GettTest.Properties.Settings.Default.Password;
      this.textBox_Password.UseSystemPasswordChar = true;
      // 
      // textBox_Email
      // 
      this.textBox_Email.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::GettTest.Properties.Settings.Default, "Email", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.textBox_Email.Location = new System.Drawing.Point(15, 64);
      this.textBox_Email.Name = "textBox_Email";
      this.textBox_Email.Size = new System.Drawing.Size(250, 20);
      this.textBox_Email.TabIndex = 3;
      this.textBox_Email.Text = global::GettTest.Properties.Settings.Default.Email;
      // 
      // textBox_ApiKey
      // 
      this.textBox_ApiKey.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::GettTest.Properties.Settings.Default, "ApiKey", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.textBox_ApiKey.Location = new System.Drawing.Point(15, 25);
      this.textBox_ApiKey.Name = "textBox_ApiKey";
      this.textBox_ApiKey.Size = new System.Drawing.Size(250, 20);
      this.textBox_ApiKey.TabIndex = 1;
      this.textBox_ApiKey.Text = global::GettTest.Properties.Settings.Default.ApiKey;
      // 
      // textBox_LiveAPISessionId
      // 
      this.textBox_LiveAPISessionId.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::GettTest.Properties.Settings.Default, "LiveSessionId", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.textBox_LiveAPISessionId.Location = new System.Drawing.Point(15, 142);
      this.textBox_LiveAPISessionId.Name = "textBox_LiveAPISessionId";
      this.textBox_LiveAPISessionId.Size = new System.Drawing.Size(250, 20);
      this.textBox_LiveAPISessionId.TabIndex = 7;
      this.textBox_LiveAPISessionId.Text = global::GettTest.Properties.Settings.Default.LiveSessionId;
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(12, 126);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(99, 13);
      this.label4.TabIndex = 6;
      this.label4.Text = "Live API Session Id";
      // 
      // SettingsForm
      // 
      this.AcceptButton = this.button_Save;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.button_Cancel;
      this.ClientSize = new System.Drawing.Size(368, 170);
      this.Controls.Add(this.textBox_LiveAPISessionId);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.button_Cancel);
      this.Controls.Add(this.button_Save);
      this.Controls.Add(this.textBox_Password);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.textBox_Email);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.textBox_ApiKey);
      this.Controls.Add(this.label1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Name = "SettingsForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Settings";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox textBox_ApiKey;
    private System.Windows.Forms.TextBox textBox_Email;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox textBox_Password;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Button button_Save;
    private System.Windows.Forms.Button button_Cancel;
    private System.Windows.Forms.TextBox textBox_LiveAPISessionId;
    private System.Windows.Forms.Label label4;
  }
}