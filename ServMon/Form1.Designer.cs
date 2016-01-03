namespace ServMon {
	partial class SettingsForm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.LGroupBox = new System.Windows.Forms.GroupBox();
			this.LAutoStart = new System.Windows.Forms.CheckBox();
			this.LBtnRestart = new System.Windows.Forms.Button();
			this.LBtnStop = new System.Windows.Forms.Button();
			this.LBtnStart = new System.Windows.Forms.Button();
			this.LServiceLabel = new System.Windows.Forms.Label();
			this.LServiceSel = new System.Windows.Forms.ComboBox();
			this.RGroupBox = new System.Windows.Forms.GroupBox();
			this.RAutoStart = new System.Windows.Forms.CheckBox();
			this.RBtnRestart = new System.Windows.Forms.Button();
			this.RBtnStop = new System.Windows.Forms.Button();
			this.RServiceLabel = new System.Windows.Forms.Label();
			this.RBtnStart = new System.Windows.Forms.Button();
			this.RServiceSel = new System.Windows.Forms.ComboBox();
			this.BtnSave = new System.Windows.Forms.Button();
			this.Exit = new System.Windows.Forms.Button();
			this.LGroupBox.SuspendLayout();
			this.RGroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// LGroupBox
			// 
			this.LGroupBox.Controls.Add(this.LAutoStart);
			this.LGroupBox.Controls.Add(this.LBtnRestart);
			this.LGroupBox.Controls.Add(this.LBtnStop);
			this.LGroupBox.Controls.Add(this.LBtnStart);
			this.LGroupBox.Controls.Add(this.LServiceLabel);
			this.LGroupBox.Controls.Add(this.LServiceSel);
			this.LGroupBox.Location = new System.Drawing.Point(10, 10);
			this.LGroupBox.Margin = new System.Windows.Forms.Padding(1);
			this.LGroupBox.Name = "LGroupBox";
			this.LGroupBox.Padding = new System.Windows.Forms.Padding(5);
			this.LGroupBox.Size = new System.Drawing.Size(275, 98);
			this.LGroupBox.TabIndex = 0;
			this.LGroupBox.TabStop = false;
			this.LGroupBox.Text = "Left Side";
			// 
			// LAutoStart
			// 
			this.LAutoStart.BackColor = System.Drawing.Color.Transparent;
			this.LAutoStart.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.LAutoStart.Location = new System.Drawing.Point(209, 20);
			this.LAutoStart.Name = "LAutoStart";
			this.LAutoStart.Size = new System.Drawing.Size(60, 17);
			this.LAutoStart.TabIndex = 7;
			this.LAutoStart.Text = "Autostart";
			this.LAutoStart.UseVisualStyleBackColor = false;
			this.LAutoStart.CheckedChanged += new System.EventHandler(this.LAutoStart_CheckedChanged);
			// 
			// LBtnRestart
			// 
			this.LBtnRestart.BackColor = System.Drawing.Color.Transparent;
			this.LBtnRestart.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.LBtnRestart.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.LBtnRestart.Location = new System.Drawing.Point(184, 65);
			this.LBtnRestart.Name = "LBtnRestart";
			this.LBtnRestart.Size = new System.Drawing.Size(83, 23);
			this.LBtnRestart.TabIndex = 4;
			this.LBtnRestart.Text = "Restart";
			this.LBtnRestart.UseVisualStyleBackColor = false;
			this.LBtnRestart.Click += new System.EventHandler(this.LBtnRestart_Click);
			// 
			// LBtnStop
			// 
			this.LBtnStop.BackColor = System.Drawing.Color.Transparent;
			this.LBtnStop.FlatAppearance.BorderColor = System.Drawing.Color.DarkRed;
			this.LBtnStop.FlatAppearance.BorderSize = 2;
			this.LBtnStop.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Brown;
			this.LBtnStop.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Firebrick;
			this.LBtnStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.LBtnStop.ForeColor = System.Drawing.Color.DarkRed;
			this.LBtnStop.Location = new System.Drawing.Point(96, 65);
			this.LBtnStop.Name = "LBtnStop";
			this.LBtnStop.Size = new System.Drawing.Size(82, 23);
			this.LBtnStop.TabIndex = 3;
			this.LBtnStop.Text = "Stop";
			this.LBtnStop.UseVisualStyleBackColor = false;
			this.LBtnStop.Click += new System.EventHandler(this.LBtnStop_Click);
			// 
			// LBtnStart
			// 
			this.LBtnStart.BackColor = System.Drawing.Color.Transparent;
			this.LBtnStart.FlatAppearance.BorderColor = System.Drawing.Color.DarkGreen;
			this.LBtnStart.FlatAppearance.BorderSize = 2;
			this.LBtnStart.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LimeGreen;
			this.LBtnStart.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen;
			this.LBtnStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.LBtnStart.ForeColor = System.Drawing.Color.Green;
			this.LBtnStart.Location = new System.Drawing.Point(8, 65);
			this.LBtnStart.Name = "LBtnStart";
			this.LBtnStart.Size = new System.Drawing.Size(82, 23);
			this.LBtnStart.TabIndex = 2;
			this.LBtnStart.Text = "Start";
			this.LBtnStart.UseVisualStyleBackColor = false;
			this.LBtnStart.Click += new System.EventHandler(this.LBtnStart_Click);
			// 
			// LServiceLabel
			// 
			this.LServiceLabel.AutoSize = true;
			this.LServiceLabel.Location = new System.Drawing.Point(9, 22);
			this.LServiceLabel.Name = "LServiceLabel";
			this.LServiceLabel.Size = new System.Drawing.Size(141, 13);
			this.LServiceLabel.TabIndex = 1;
			this.LServiceLabel.Text = "Choose a service to monitor:";
			// 
			// LServiceSel
			// 
			this.LServiceSel.FormattingEnabled = true;
			this.LServiceSel.Location = new System.Drawing.Point(8, 38);
			this.LServiceSel.Name = "LServiceSel";
			this.LServiceSel.Size = new System.Drawing.Size(259, 21);
			this.LServiceSel.TabIndex = 0;
			// 
			// RGroupBox
			// 
			this.RGroupBox.Controls.Add(this.RAutoStart);
			this.RGroupBox.Controls.Add(this.RBtnRestart);
			this.RGroupBox.Controls.Add(this.RBtnStop);
			this.RGroupBox.Controls.Add(this.RServiceLabel);
			this.RGroupBox.Controls.Add(this.RBtnStart);
			this.RGroupBox.Controls.Add(this.RServiceSel);
			this.RGroupBox.Location = new System.Drawing.Point(299, 10);
			this.RGroupBox.Margin = new System.Windows.Forms.Padding(1);
			this.RGroupBox.Name = "RGroupBox";
			this.RGroupBox.Padding = new System.Windows.Forms.Padding(5);
			this.RGroupBox.Size = new System.Drawing.Size(275, 98);
			this.RGroupBox.TabIndex = 1;
			this.RGroupBox.TabStop = false;
			this.RGroupBox.Text = "Right Side";
			// 
			// RAutoStart
			// 
			this.RAutoStart.BackColor = System.Drawing.Color.Transparent;
			this.RAutoStart.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.RAutoStart.Location = new System.Drawing.Point(209, 20);
			this.RAutoStart.Name = "RAutoStart";
			this.RAutoStart.Size = new System.Drawing.Size(60, 17);
			this.RAutoStart.TabIndex = 6;
			this.RAutoStart.Text = "Autostart";
			this.RAutoStart.UseVisualStyleBackColor = false;
			this.RAutoStart.CheckedChanged += new System.EventHandler(this.RAutoStart_CheckedChanged);
			// 
			// RBtnRestart
			// 
			this.RBtnRestart.BackColor = System.Drawing.Color.Transparent;
			this.RBtnRestart.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.RBtnRestart.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.RBtnRestart.ForeColor = System.Drawing.SystemColors.ControlText;
			this.RBtnRestart.Location = new System.Drawing.Point(184, 65);
			this.RBtnRestart.Name = "RBtnRestart";
			this.RBtnRestart.Size = new System.Drawing.Size(83, 23);
			this.RBtnRestart.TabIndex = 5;
			this.RBtnRestart.Text = "Restart";
			this.RBtnRestart.UseVisualStyleBackColor = false;
			this.RBtnRestart.Click += new System.EventHandler(this.RBtnRestart_Click);
			// 
			// RBtnStop
			// 
			this.RBtnStop.BackColor = System.Drawing.Color.Transparent;
			this.RBtnStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.RBtnStop.ForeColor = System.Drawing.Color.DarkRed;
			this.RBtnStop.Location = new System.Drawing.Point(96, 65);
			this.RBtnStop.Name = "RBtnStop";
			this.RBtnStop.Size = new System.Drawing.Size(82, 23);
			this.RBtnStop.TabIndex = 5;
			this.RBtnStop.Text = "Stop";
			this.RBtnStop.UseVisualStyleBackColor = false;
			this.RBtnStop.Click += new System.EventHandler(this.RBtnStop_Click);
			// 
			// RServiceLabel
			// 
			this.RServiceLabel.AutoSize = true;
			this.RServiceLabel.Location = new System.Drawing.Point(8, 22);
			this.RServiceLabel.Name = "RServiceLabel";
			this.RServiceLabel.Size = new System.Drawing.Size(141, 13);
			this.RServiceLabel.TabIndex = 3;
			this.RServiceLabel.Text = "Choose a service to monitor:";
			// 
			// RBtnStart
			// 
			this.RBtnStart.BackColor = System.Drawing.Color.Transparent;
			this.RBtnStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.RBtnStart.ForeColor = System.Drawing.Color.Green;
			this.RBtnStart.Location = new System.Drawing.Point(8, 65);
			this.RBtnStart.Name = "RBtnStart";
			this.RBtnStart.Size = new System.Drawing.Size(82, 23);
			this.RBtnStart.TabIndex = 4;
			this.RBtnStart.Text = "Start";
			this.RBtnStart.UseVisualStyleBackColor = false;
			this.RBtnStart.Click += new System.EventHandler(this.RBtnStart_Click);
			// 
			// RServiceSel
			// 
			this.RServiceSel.FormattingEnabled = true;
			this.RServiceSel.Location = new System.Drawing.Point(8, 38);
			this.RServiceSel.Name = "RServiceSel";
			this.RServiceSel.Size = new System.Drawing.Size(259, 21);
			this.RServiceSel.TabIndex = 2;
			// 
			// BtnSave
			// 
			this.BtnSave.Location = new System.Drawing.Point(169, 112);
			this.BtnSave.Name = "BtnSave";
			this.BtnSave.Size = new System.Drawing.Size(116, 28);
			this.BtnSave.TabIndex = 2;
			this.BtnSave.Text = "Save Settings";
			this.BtnSave.UseVisualStyleBackColor = true;
			this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
			// 
			// Exit
			// 
			this.Exit.Location = new System.Drawing.Point(299, 112);
			this.Exit.Name = "Exit";
			this.Exit.Size = new System.Drawing.Size(116, 28);
			this.Exit.TabIndex = 3;
			this.Exit.Text = "Exit Application";
			this.Exit.UseVisualStyleBackColor = true;
			this.Exit.Click += new System.EventHandler(this.Exit_Click);
			// 
			// SettingsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(584, 152);
			this.Controls.Add(this.Exit);
			this.Controls.Add(this.BtnSave);
			this.Controls.Add(this.RGroupBox);
			this.Controls.Add(this.LGroupBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "SettingsForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "ServMon";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingsForm_FormClosing);
			this.LGroupBox.ResumeLayout(false);
			this.LGroupBox.PerformLayout();
			this.RGroupBox.ResumeLayout(false);
			this.RGroupBox.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox LGroupBox;
		private System.Windows.Forms.Label LServiceLabel;
		private System.Windows.Forms.ComboBox LServiceSel;
		private System.Windows.Forms.GroupBox RGroupBox;
		private System.Windows.Forms.Label RServiceLabel;
		private System.Windows.Forms.ComboBox RServiceSel;
		private System.Windows.Forms.Button LBtnStop;
		private System.Windows.Forms.Button LBtnStart;
		private System.Windows.Forms.Button BtnSave;
		private System.Windows.Forms.Button RBtnStop;
		private System.Windows.Forms.Button RBtnStart;
		private System.Windows.Forms.Button LBtnRestart;
		private System.Windows.Forms.Button RBtnRestart;
		private System.Windows.Forms.Button Exit;
		private System.Windows.Forms.CheckBox LAutoStart;
		private System.Windows.Forms.CheckBox RAutoStart;
	}
}

