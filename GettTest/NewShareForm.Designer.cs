namespace GettTest
{
  partial class NewShareForm
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
      this.button_Create = new System.Windows.Forms.Button();
      this.button_Cancel = new System.Windows.Forms.Button();
      this.textBox_Title = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // button_Create
      // 
      this.button_Create.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.button_Create.Location = new System.Drawing.Point(12, 83);
      this.button_Create.Name = "button_Create";
      this.button_Create.Size = new System.Drawing.Size(75, 23);
      this.button_Create.TabIndex = 2;
      this.button_Create.Text = "Create";
      this.button_Create.UseVisualStyleBackColor = true;
      this.button_Create.Click += new System.EventHandler(this.button_Create_Click);
      // 
      // button_Cancel
      // 
      this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.button_Cancel.Location = new System.Drawing.Point(138, 83);
      this.button_Cancel.Name = "button_Cancel";
      this.button_Cancel.Size = new System.Drawing.Size(75, 23);
      this.button_Cancel.TabIndex = 3;
      this.button_Cancel.Text = "Cancel";
      this.button_Cancel.UseVisualStyleBackColor = true;
      // 
      // textBox_Title
      // 
      this.textBox_Title.Location = new System.Drawing.Point(12, 45);
      this.textBox_Title.Name = "textBox_Title";
      this.textBox_Title.Size = new System.Drawing.Size(201, 20);
      this.textBox_Title.TabIndex = 1;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(12, 29);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(75, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Title (Optional)";
      // 
      // NewShareForm
      // 
      this.AcceptButton = this.button_Create;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.button_Cancel;
      this.ClientSize = new System.Drawing.Size(225, 118);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.textBox_Title);
      this.Controls.Add(this.button_Cancel);
      this.Controls.Add(this.button_Create);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Name = "NewShareForm";
      this.Text = "Create new share";
      this.Load += new System.EventHandler(this.NewShareForm_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button button_Create;
    private System.Windows.Forms.Button button_Cancel;
    private System.Windows.Forms.TextBox textBox_Title;
    private System.Windows.Forms.Label label1;
  }
}