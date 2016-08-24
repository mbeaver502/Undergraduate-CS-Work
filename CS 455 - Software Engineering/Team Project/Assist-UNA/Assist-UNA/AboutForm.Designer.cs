namespace Assist_UNA
{
    partial class AboutForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this.btnAboutClose = new System.Windows.Forms.Button();
            this.txtAbout = new System.Windows.Forms.TextBox();
            this.lblVersion = new System.Windows.Forms.Label();
            this.txtInfo = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAboutClose
            // 
            this.btnAboutClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(100)))), ((int)(((byte)(255)))));
            this.btnAboutClose.Location = new System.Drawing.Point(364, 294);
            this.btnAboutClose.Name = "btnAboutClose";
            this.btnAboutClose.Size = new System.Drawing.Size(75, 23);
            this.btnAboutClose.TabIndex = 3;
            this.btnAboutClose.Text = "Ok";
            this.btnAboutClose.UseVisualStyleBackColor = false;
            this.btnAboutClose.Click += new System.EventHandler(this.BtnAboutCloseClick);
            // 
            // txtAbout
            // 
            this.txtAbout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(162)))));
            this.txtAbout.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAbout.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAbout.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(0)))), ((int)(((byte)(120)))));
            this.txtAbout.Location = new System.Drawing.Point(12, 96);
            this.txtAbout.Multiline = true;
            this.txtAbout.Name = "txtAbout";
            this.txtAbout.ReadOnly = true;
            this.txtAbout.Size = new System.Drawing.Size(427, 192);
            this.txtAbout.TabIndex = 1;
            this.txtAbout.Text = resources.GetString("txtAbout.Text");
            this.txtAbout.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TxtInfoKeyUp);
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersion.ForeColor = System.Drawing.SystemColors.Control;
            this.lblVersion.Location = new System.Drawing.Point(12, 298);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(204, 16);
            this.lblVersion.TabIndex = 5;
            this.lblVersion.Text = "ASSIST/UNA v0.9 — April 2014";
            // 
            // txtInfo
            // 
            this.txtInfo.Location = new System.Drawing.Point(458, 188);
            this.txtInfo.Multiline = true;
            this.txtInfo.Name = "txtInfo";
            this.txtInfo.Size = new System.Drawing.Size(10, 10);
            this.txtInfo.TabIndex = 0;
            this.txtInfo.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TxtInfoKeyUp);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(427, 78);
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(152)))), ((int)(((byte)(0)))), ((int)(((byte)(230)))));
            this.ClientSize = new System.Drawing.Size(450, 329);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.txtInfo);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.btnAboutClose);
            this.Controls.Add(this.txtAbout);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About ASSIST/UNA";
            this.TopMost = true;
            this.Activated += new System.EventHandler(this.AboutFormActivated);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAboutClose;
        private System.Windows.Forms.TextBox txtAbout;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.TextBox txtInfo;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}