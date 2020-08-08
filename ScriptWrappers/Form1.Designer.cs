namespace ScriptWrappers
{
	partial class Form1
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.label1 = new System.Windows.Forms.Label();
			this.TemplatePathTextBox = new System.Windows.Forms.TextBox();
			this.BrowseButton = new System.Windows.Forms.Button();
			this.FilesToConvertListBox = new System.Windows.Forms.ListBox();
			this.label2 = new System.Windows.Forms.Label();
			this.ConvertButton = new System.Windows.Forms.Button();
			this.OpenFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.FolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
			this.RemoveItemButton = new System.Windows.Forms.Button();
			this.gbEmblem = new System.Windows.Forms.GroupBox();
			this.rbNoEditable = new System.Windows.Forms.RadioButton();
			this.rbNone = new System.Windows.Forms.RadioButton();
			this.radioButton1 = new System.Windows.Forms.RadioButton();
			this.lblVersion = new System.Windows.Forms.Label();
			this.gbEmblem.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(79, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Template Path:";
			// 
			// TemplatePathTextBox
			// 
			this.TemplatePathTextBox.Location = new System.Drawing.Point(97, 6);
			this.TemplatePathTextBox.Name = "TemplatePathTextBox";
			this.TemplatePathTextBox.Size = new System.Drawing.Size(509, 20);
			this.TemplatePathTextBox.TabIndex = 1;
			this.TemplatePathTextBox.Text = "D:\\Google Drive\\Nikeaa Design LLC\\Wrappers\\WBG - Web Backgrounds";
			// 
			// BrowseButton
			// 
			this.BrowseButton.Location = new System.Drawing.Point(612, 4);
			this.BrowseButton.Name = "BrowseButton";
			this.BrowseButton.Size = new System.Drawing.Size(75, 23);
			this.BrowseButton.TabIndex = 2;
			this.BrowseButton.Text = "Browse ...";
			this.BrowseButton.UseVisualStyleBackColor = true;
			this.BrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
			// 
			// FilesToConvertListBox
			// 
			this.FilesToConvertListBox.FormattingEnabled = true;
			this.FilesToConvertListBox.Location = new System.Drawing.Point(12, 109);
			this.FilesToConvertListBox.Name = "FilesToConvertListBox";
			this.FilesToConvertListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.FilesToConvertListBox.Size = new System.Drawing.Size(675, 355);
			this.FilesToConvertListBox.TabIndex = 3;
			this.FilesToConvertListBox.SelectedIndexChanged += new System.EventHandler(this.FilesToConvertListBox_SelectedIndexChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 93);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(83, 13);
			this.label2.TabIndex = 4;
			this.label2.Text = "Files to Convert:";
			// 
			// ConvertButton
			// 
			this.ConvertButton.Enabled = false;
			this.ConvertButton.Location = new System.Drawing.Point(12, 470);
			this.ConvertButton.Name = "ConvertButton";
			this.ConvertButton.Size = new System.Drawing.Size(75, 23);
			this.ConvertButton.TabIndex = 5;
			this.ConvertButton.Text = "Convert";
			this.ConvertButton.UseVisualStyleBackColor = true;
			this.ConvertButton.Click += new System.EventHandler(this.ConvertButton_Click);
			// 
			// OpenFileDialog
			// 
			this.OpenFileDialog.FileName = "openFileDialog1";
			// 
			// FolderBrowserDialog
			// 
			this.FolderBrowserDialog.RootFolder = System.Environment.SpecialFolder.MyComputer;
			this.FolderBrowserDialog.SelectedPath = "C:\\Users\\Scott\\Google Drive\\Nikeaa Design LLC\\Wrappers\\WBG - Web Backgrounds";
			// 
			// RemoveItemButton
			// 
			this.RemoveItemButton.Enabled = false;
			this.RemoveItemButton.Location = new System.Drawing.Point(597, 470);
			this.RemoveItemButton.Name = "RemoveItemButton";
			this.RemoveItemButton.Size = new System.Drawing.Size(90, 23);
			this.RemoveItemButton.TabIndex = 6;
			this.RemoveItemButton.Text = "Remove Item(s)";
			this.RemoveItemButton.UseVisualStyleBackColor = true;
			this.RemoveItemButton.Click += new System.EventHandler(this.RemoveItemButton_Click);
			// 
			// gbEmblem
			// 
			this.gbEmblem.Controls.Add(this.rbNoEditable);
			this.gbEmblem.Controls.Add(this.rbNone);
			this.gbEmblem.Controls.Add(this.radioButton1);
			this.gbEmblem.Location = new System.Drawing.Point(15, 33);
			this.gbEmblem.Name = "gbEmblem";
			this.gbEmblem.Size = new System.Drawing.Size(672, 48);
			this.gbEmblem.TabIndex = 11;
			this.gbEmblem.TabStop = false;
			this.gbEmblem.Text = "Emblem?";
			// 
			// rbNoEditable
			// 
			this.rbNoEditable.AutoSize = true;
			this.rbNoEditable.Location = new System.Drawing.Point(82, 19);
			this.rbNoEditable.Name = "rbNoEditable";
			this.rbNoEditable.Size = new System.Drawing.Size(80, 17);
			this.rbNoEditable.TabIndex = 13;
			this.rbNoEditable.TabStop = true;
			this.rbNoEditable.Text = "No Editable";
			this.rbNoEditable.UseVisualStyleBackColor = true;
			// 
			// rbNone
			// 
			this.rbNone.AutoSize = true;
			this.rbNone.Checked = true;
			this.rbNone.Location = new System.Drawing.Point(7, 20);
			this.rbNone.Name = "rbNone";
			this.rbNone.Size = new System.Drawing.Size(51, 17);
			this.rbNone.TabIndex = 12;
			this.rbNone.TabStop = true;
			this.rbNone.Text = "None";
			this.rbNone.UseVisualStyleBackColor = true;
			// 
			// radioButton1
			// 
			this.radioButton1.AutoSize = true;
			this.radioButton1.Location = new System.Drawing.Point(-44, 16);
			this.radioButton1.Name = "radioButton1";
			this.radioButton1.Size = new System.Drawing.Size(51, 17);
			this.radioButton1.TabIndex = 11;
			this.radioButton1.Text = "None";
			this.radioButton1.UseVisualStyleBackColor = true;
			// 
			// lblVersion
			// 
			this.lblVersion.AutoSize = true;
			this.lblVersion.Location = new System.Drawing.Point(599, 93);
			this.lblVersion.Name = "lblVersion";
			this.lblVersion.Size = new System.Drawing.Size(88, 13);
			this.lblVersion.TabIndex = 12;
			this.lblVersion.Text = "100.100.100.100";
			this.lblVersion.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(699, 501);
			this.Controls.Add(this.lblVersion);
			this.Controls.Add(this.gbEmblem);
			this.Controls.Add(this.RemoveItemButton);
			this.Controls.Add(this.ConvertButton);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.FilesToConvertListBox);
			this.Controls.Add(this.BrowseButton);
			this.Controls.Add(this.TemplatePathTextBox);
			this.Controls.Add(this.label1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "Form1";
			this.Text = "Script Wrappers";
			this.gbEmblem.ResumeLayout(false);
			this.gbEmblem.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox TemplatePathTextBox;
		private System.Windows.Forms.Button BrowseButton;
		private System.Windows.Forms.ListBox FilesToConvertListBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button ConvertButton;
		private System.Windows.Forms.OpenFileDialog OpenFileDialog;
		private System.Windows.Forms.FolderBrowserDialog FolderBrowserDialog;
		private System.Windows.Forms.Button RemoveItemButton;
		private System.Windows.Forms.GroupBox gbEmblem;
		private System.Windows.Forms.RadioButton radioButton1;
		private System.Windows.Forms.RadioButton rbNoEditable;
		private System.Windows.Forms.RadioButton rbNone;
		private System.Windows.Forms.Label lblVersion;
	}
}

