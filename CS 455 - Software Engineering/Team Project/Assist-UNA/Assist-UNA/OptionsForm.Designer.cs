namespace Assist_UNA
{
    partial class OptionsForm
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
            this.gbASSISTIOptions = new System.Windows.Forms.GroupBox();
            this.txtMaxSize = new System.Windows.Forms.MaskedTextBox();
            this.txtMaxPages = new System.Windows.Forms.MaskedTextBox();
            this.txtMaxInstructions = new System.Windows.Forms.MaskedTextBox();
            this.txtMaxLines = new System.Windows.Forms.MaskedTextBox();
            this.lblMaxSize = new System.Windows.Forms.Label();
            this.lblMaxPages = new System.Windows.Forms.Label();
            this.lblMaxInstructions = new System.Windows.Forms.Label();
            this.lblMaxLines = new System.Windows.Forms.Label();
            this.btnOptionsApply = new System.Windows.Forms.Button();
            this.gbASSISTUNAOptions = new System.Windows.Forms.GroupBox();
            this.txtLabelColor = new System.Windows.Forms.TextBox();
            this.txtFormText = new System.Windows.Forms.TextBox();
            this.txtBrowsePRT = new System.Windows.Forms.TextBox();
            this.lblPRT = new System.Windows.Forms.Label();
            this.btnLabelColor = new System.Windows.Forms.Button();
            this.txtCommentColor = new Assist_UNA.CustomSourceEditor();
            this.btnFormText = new System.Windows.Forms.Button();
            this.btnAccent = new System.Windows.Forms.Button();
            this.btnBrowsePRT = new System.Windows.Forms.Button();
            this.btnBackColor2 = new System.Windows.Forms.Button();
            this.btnCommentColor = new System.Windows.Forms.Button();
            this.btnBackColor = new System.Windows.Forms.Button();
            this.lblCustomLabels = new System.Windows.Forms.Label();
            this.lblFormText = new System.Windows.Forms.Label();
            this.lblAccent = new System.Windows.Forms.Label();
            this.lblBackColor2 = new System.Windows.Forms.Label();
            this.txtAccent = new System.Windows.Forms.RichTextBox();
            this.txtBackColor2 = new System.Windows.Forms.RichTextBox();
            this.lblBackColor = new System.Windows.Forms.Label();
            this.txtBackColor = new System.Windows.Forms.RichTextBox();
            this.radCustom = new System.Windows.Forms.RadioButton();
            this.radSystem = new System.Windows.Forms.RadioButton();
            this.radUNA = new System.Windows.Forms.RadioButton();
            this.lblTheme = new System.Windows.Forms.Label();
            this.lblCommentColor = new System.Windows.Forms.Label();
            this.btnOptionsClose = new System.Windows.Forms.Button();
            this.gbASSISTIOptions.SuspendLayout();
            this.gbASSISTUNAOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbASSISTIOptions
            // 
            this.gbASSISTIOptions.Controls.Add(this.txtMaxSize);
            this.gbASSISTIOptions.Controls.Add(this.txtMaxPages);
            this.gbASSISTIOptions.Controls.Add(this.txtMaxInstructions);
            this.gbASSISTIOptions.Controls.Add(this.txtMaxLines);
            this.gbASSISTIOptions.Controls.Add(this.lblMaxSize);
            this.gbASSISTIOptions.Controls.Add(this.lblMaxPages);
            this.gbASSISTIOptions.Controls.Add(this.lblMaxInstructions);
            this.gbASSISTIOptions.Controls.Add(this.lblMaxLines);
            this.gbASSISTIOptions.Location = new System.Drawing.Point(12, 2);
            this.gbASSISTIOptions.Name = "gbASSISTIOptions";
            this.gbASSISTIOptions.Size = new System.Drawing.Size(300, 126);
            this.gbASSISTIOptions.TabIndex = 3;
            this.gbASSISTIOptions.TabStop = false;
            this.gbASSISTIOptions.Text = " ASSIST/I Options ";
            // 
            // txtMaxSize
            // 
            this.txtMaxSize.HidePromptOnLeave = true;
            this.txtMaxSize.Location = new System.Drawing.Point(234, 99);
            this.txtMaxSize.Mask = "0000";
            this.txtMaxSize.Name = "txtMaxSize";
            this.txtMaxSize.PromptChar = ' ';
            this.txtMaxSize.Size = new System.Drawing.Size(47, 20);
            this.txtMaxSize.TabIndex = 3;
            this.txtMaxSize.Text = "2700";
            this.txtMaxSize.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;
            this.txtMaxSize.ValidatingType = typeof(int);
            this.txtMaxSize.TextChanged += new System.EventHandler(this.TxtMaxSizeTextChanged);
            // 
            // txtMaxPages
            // 
            this.txtMaxPages.HidePromptOnLeave = true;
            this.txtMaxPages.Location = new System.Drawing.Point(234, 73);
            this.txtMaxPages.Mask = "0000";
            this.txtMaxPages.Name = "txtMaxPages";
            this.txtMaxPages.PromptChar = ' ';
            this.txtMaxPages.Size = new System.Drawing.Size(47, 20);
            this.txtMaxPages.TabIndex = 2;
            this.txtMaxPages.Text = "900";
            this.txtMaxPages.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;
            this.txtMaxPages.ValidatingType = typeof(int);
            this.txtMaxPages.TextChanged += new System.EventHandler(this.TxtMaxPagesTextChanged);
            // 
            // txtMaxInstructions
            // 
            this.txtMaxInstructions.HidePromptOnLeave = true;
            this.txtMaxInstructions.Location = new System.Drawing.Point(234, 47);
            this.txtMaxInstructions.Mask = "0000";
            this.txtMaxInstructions.Name = "txtMaxInstructions";
            this.txtMaxInstructions.PromptChar = ' ';
            this.txtMaxInstructions.Size = new System.Drawing.Size(47, 20);
            this.txtMaxInstructions.TabIndex = 1;
            this.txtMaxInstructions.Text = "9000";
            this.txtMaxInstructions.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;
            this.txtMaxInstructions.ValidatingType = typeof(int);
            this.txtMaxInstructions.TextChanged += new System.EventHandler(this.TxtMaxInstructionsTextChanged);
            // 
            // txtMaxLines
            // 
            this.txtMaxLines.HidePromptOnLeave = true;
            this.txtMaxLines.Location = new System.Drawing.Point(234, 21);
            this.txtMaxLines.Mask = "0000";
            this.txtMaxLines.Name = "txtMaxLines";
            this.txtMaxLines.PromptChar = ' ';
            this.txtMaxLines.Size = new System.Drawing.Size(47, 20);
            this.txtMaxLines.TabIndex = 0;
            this.txtMaxLines.Text = "500";
            this.txtMaxLines.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;
            this.txtMaxLines.ValidatingType = typeof(int);
            this.txtMaxLines.TextChanged += new System.EventHandler(this.TxtMaxLinesTextChanged);
            // 
            // lblMaxSize
            // 
            this.lblMaxSize.AutoSize = true;
            this.lblMaxSize.Location = new System.Drawing.Point(17, 102);
            this.lblMaxSize.Name = "lblMaxSize";
            this.lblMaxSize.Size = new System.Drawing.Size(119, 13);
            this.lblMaxSize.TabIndex = 7;
            this.lblMaxSize.Text = "Maximum Size (in bytes)";
            // 
            // lblMaxPages
            // 
            this.lblMaxPages.AutoSize = true;
            this.lblMaxPages.Location = new System.Drawing.Point(17, 76);
            this.lblMaxPages.Name = "lblMaxPages";
            this.lblMaxPages.Size = new System.Drawing.Size(175, 13);
            this.lblMaxPages.TabIndex = 5;
            this.lblMaxPages.Text = "Maximum Number of Printout Pages";
            // 
            // lblMaxInstructions
            // 
            this.lblMaxInstructions.AutoSize = true;
            this.lblMaxInstructions.Location = new System.Drawing.Point(17, 50);
            this.lblMaxInstructions.Name = "lblMaxInstructions";
            this.lblMaxInstructions.Size = new System.Drawing.Size(214, 13);
            this.lblMaxInstructions.TabIndex = 3;
            this.lblMaxInstructions.Text = "Maximum Number of Instructions to Execute";
            // 
            // lblMaxLines
            // 
            this.lblMaxLines.AutoSize = true;
            this.lblMaxLines.Location = new System.Drawing.Point(17, 24);
            this.lblMaxLines.Name = "lblMaxLines";
            this.lblMaxLines.Size = new System.Drawing.Size(131, 13);
            this.lblMaxLines.TabIndex = 1;
            this.lblMaxLines.Text = "Maximum Number of Lines";
            // 
            // btnOptionsApply
            // 
            this.btnOptionsApply.Enabled = false;
            this.btnOptionsApply.Location = new System.Drawing.Point(145, 386);
            this.btnOptionsApply.Name = "btnOptionsApply";
            this.btnOptionsApply.Size = new System.Drawing.Size(75, 23);
            this.btnOptionsApply.TabIndex = 0;
            this.btnOptionsApply.Text = "Apply";
            this.btnOptionsApply.UseVisualStyleBackColor = true;
            this.btnOptionsApply.Click += new System.EventHandler(this.BtnOptionsApplyClick);
            // 
            // gbASSISTUNAOptions
            // 
            this.gbASSISTUNAOptions.Controls.Add(this.txtLabelColor);
            this.gbASSISTUNAOptions.Controls.Add(this.txtFormText);
            this.gbASSISTUNAOptions.Controls.Add(this.txtBrowsePRT);
            this.gbASSISTUNAOptions.Controls.Add(this.lblPRT);
            this.gbASSISTUNAOptions.Controls.Add(this.btnLabelColor);
            this.gbASSISTUNAOptions.Controls.Add(this.txtCommentColor);
            this.gbASSISTUNAOptions.Controls.Add(this.btnFormText);
            this.gbASSISTUNAOptions.Controls.Add(this.btnAccent);
            this.gbASSISTUNAOptions.Controls.Add(this.btnBrowsePRT);
            this.gbASSISTUNAOptions.Controls.Add(this.btnBackColor2);
            this.gbASSISTUNAOptions.Controls.Add(this.btnCommentColor);
            this.gbASSISTUNAOptions.Controls.Add(this.btnBackColor);
            this.gbASSISTUNAOptions.Controls.Add(this.lblCustomLabels);
            this.gbASSISTUNAOptions.Controls.Add(this.lblFormText);
            this.gbASSISTUNAOptions.Controls.Add(this.lblAccent);
            this.gbASSISTUNAOptions.Controls.Add(this.lblBackColor2);
            this.gbASSISTUNAOptions.Controls.Add(this.txtAccent);
            this.gbASSISTUNAOptions.Controls.Add(this.txtBackColor2);
            this.gbASSISTUNAOptions.Controls.Add(this.lblBackColor);
            this.gbASSISTUNAOptions.Controls.Add(this.txtBackColor);
            this.gbASSISTUNAOptions.Controls.Add(this.radCustom);
            this.gbASSISTUNAOptions.Controls.Add(this.radSystem);
            this.gbASSISTUNAOptions.Controls.Add(this.radUNA);
            this.gbASSISTUNAOptions.Controls.Add(this.lblTheme);
            this.gbASSISTUNAOptions.Controls.Add(this.lblCommentColor);
            this.gbASSISTUNAOptions.Location = new System.Drawing.Point(12, 134);
            this.gbASSISTUNAOptions.Name = "gbASSISTUNAOptions";
            this.gbASSISTUNAOptions.Size = new System.Drawing.Size(300, 235);
            this.gbASSISTUNAOptions.TabIndex = 4;
            this.gbASSISTUNAOptions.TabStop = false;
            this.gbASSISTUNAOptions.Text = " ASSIST/UNA Options ";
            // 
            // txtLabelColor
            // 
            this.txtLabelColor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(100)))), ((int)(((byte)(255)))));
            this.txtLabelColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLabelColor.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtLabelColor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLabelColor.ForeColor = System.Drawing.SystemColors.Control;
            this.txtLabelColor.Location = new System.Drawing.Point(105, 130);
            this.txtLabelColor.MaxLength = 16;
            this.txtLabelColor.Name = "txtLabelColor";
            this.txtLabelColor.ReadOnly = true;
            this.txtLabelColor.Size = new System.Drawing.Size(153, 20);
            this.txtLabelColor.TabIndex = 33;
            this.txtLabelColor.TabStop = false;
            this.txtLabelColor.Text = "R 00       R 01       R 02";
            this.txtLabelColor.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtFormText
            // 
            this.txtFormText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(162)))));
            this.txtFormText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFormText.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtFormText.Font = new System.Drawing.Font("Courier New", 9F);
            this.txtFormText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(0)))), ((int)(((byte)(120)))));
            this.txtFormText.Location = new System.Drawing.Point(105, 94);
            this.txtFormText.MaxLength = 16;
            this.txtFormText.Name = "txtFormText";
            this.txtFormText.ReadOnly = true;
            this.txtFormText.Size = new System.Drawing.Size(153, 21);
            this.txtFormText.TabIndex = 33;
            this.txtFormText.TabStop = false;
            this.txtFormText.Text = "F4F4F4F4 F4F4F4F4";
            this.txtFormText.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtBrowsePRT
            // 
            this.txtBrowsePRT.BackColor = System.Drawing.SystemColors.Window;
            this.txtBrowsePRT.Location = new System.Drawing.Point(78, 205);
            this.txtBrowsePRT.Name = "txtBrowsePRT";
            this.txtBrowsePRT.ReadOnly = true;
            this.txtBrowsePRT.Size = new System.Drawing.Size(140, 20);
            this.txtBrowsePRT.TabIndex = 7;
            this.txtBrowsePRT.TabStop = false;
            this.txtBrowsePRT.Text = "c:\\";
            this.txtBrowsePRT.WordWrap = false;
            // 
            // lblPRT
            // 
            this.lblPRT.AutoSize = true;
            this.lblPRT.Location = new System.Drawing.Point(10, 200);
            this.lblPRT.Name = "lblPRT";
            this.lblPRT.Size = new System.Drawing.Size(62, 26);
            this.lblPRT.TabIndex = 6;
            this.lblPRT.Text = "Associated \r\nPRT:";
            this.lblPRT.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnLabelColor
            // 
            this.btnLabelColor.Enabled = false;
            this.btnLabelColor.Location = new System.Drawing.Point(258, 129);
            this.btnLabelColor.Name = "btnLabelColor";
            this.btnLabelColor.Size = new System.Drawing.Size(26, 23);
            this.btnLabelColor.TabIndex = 4;
            this.btnLabelColor.Text = "...";
            this.btnLabelColor.UseVisualStyleBackColor = true;
            this.btnLabelColor.Click += new System.EventHandler(this.BtnLabelColorClick);
            // 
            // txtCommentColor
            // 
            this.txtCommentColor.BackColor = System.Drawing.SystemColors.Window;
            this.txtCommentColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCommentColor.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCommentColor.ForeColor = System.Drawing.Color.DeepPink;
            this.txtCommentColor.Location = new System.Drawing.Point(104, 165);
            this.txtCommentColor.Name = "txtCommentColor";
            this.txtCommentColor.ReadOnly = true;
            this.txtCommentColor.Size = new System.Drawing.Size(154, 23);
            this.txtCommentColor.TabIndex = 5;
            this.txtCommentColor.TabStop = false;
            this.txtCommentColor.Text = "COMMENT COLOR TEST";
            // 
            // btnFormText
            // 
            this.btnFormText.Enabled = false;
            this.btnFormText.Location = new System.Drawing.Point(258, 93);
            this.btnFormText.Name = "btnFormText";
            this.btnFormText.Size = new System.Drawing.Size(26, 23);
            this.btnFormText.TabIndex = 3;
            this.btnFormText.Text = "...";
            this.btnFormText.UseVisualStyleBackColor = true;
            this.btnFormText.Click += new System.EventHandler(this.BtnFormTextClick);
            // 
            // btnAccent
            // 
            this.btnAccent.Enabled = false;
            this.btnAccent.Location = new System.Drawing.Point(258, 61);
            this.btnAccent.Name = "btnAccent";
            this.btnAccent.Size = new System.Drawing.Size(26, 23);
            this.btnAccent.TabIndex = 2;
            this.btnAccent.Text = "...";
            this.btnAccent.UseVisualStyleBackColor = true;
            this.btnAccent.Click += new System.EventHandler(this.BtnAccentClick);
            // 
            // btnBrowsePRT
            // 
            this.btnBrowsePRT.ForeColor = System.Drawing.SystemColors.WindowText;
            this.btnBrowsePRT.Location = new System.Drawing.Point(223, 203);
            this.btnBrowsePRT.Name = "btnBrowsePRT";
            this.btnBrowsePRT.Size = new System.Drawing.Size(62, 23);
            this.btnBrowsePRT.TabIndex = 6;
            this.btnBrowsePRT.Text = "Browse...";
            this.btnBrowsePRT.UseVisualStyleBackColor = true;
            this.btnBrowsePRT.Click += new System.EventHandler(this.BtnBrowsePRTClick);
            // 
            // btnBackColor2
            // 
            this.btnBackColor2.Enabled = false;
            this.btnBackColor2.Location = new System.Drawing.Point(156, 61);
            this.btnBackColor2.Name = "btnBackColor2";
            this.btnBackColor2.Size = new System.Drawing.Size(26, 23);
            this.btnBackColor2.TabIndex = 1;
            this.btnBackColor2.Text = "...";
            this.btnBackColor2.UseVisualStyleBackColor = true;
            this.btnBackColor2.Click += new System.EventHandler(this.BtnBackColor2Click);
            // 
            // btnCommentColor
            // 
            this.btnCommentColor.ForeColor = System.Drawing.SystemColors.WindowText;
            this.btnCommentColor.Location = new System.Drawing.Point(258, 163);
            this.btnCommentColor.Name = "btnCommentColor";
            this.btnCommentColor.Size = new System.Drawing.Size(26, 23);
            this.btnCommentColor.TabIndex = 5;
            this.btnCommentColor.Text = "...";
            this.btnCommentColor.UseVisualStyleBackColor = true;
            this.btnCommentColor.Click += new System.EventHandler(this.BtnCommentColorClick);
            // 
            // btnBackColor
            // 
            this.btnBackColor.Enabled = false;
            this.btnBackColor.Location = new System.Drawing.Point(58, 61);
            this.btnBackColor.Name = "btnBackColor";
            this.btnBackColor.Size = new System.Drawing.Size(26, 23);
            this.btnBackColor.TabIndex = 0;
            this.btnBackColor.Text = "...";
            this.btnBackColor.UseVisualStyleBackColor = true;
            this.btnBackColor.Click += new System.EventHandler(this.BtnBackColorClick);
            // 
            // lblCustomLabels
            // 
            this.lblCustomLabels.AutoSize = true;
            this.lblCustomLabels.Location = new System.Drawing.Point(10, 132);
            this.lblCustomLabels.Name = "lblCustomLabels";
            this.lblCustomLabels.Size = new System.Drawing.Size(79, 13);
            this.lblCustomLabels.TabIndex = 3;
            this.lblCustomLabels.Text = "Custom Labels:";
            // 
            // lblFormText
            // 
            this.lblFormText.AutoSize = true;
            this.lblFormText.Location = new System.Drawing.Point(10, 97);
            this.lblFormText.Name = "lblFormText";
            this.lblFormText.Size = new System.Drawing.Size(95, 13);
            this.lblFormText.TabIndex = 3;
            this.lblFormText.Text = "Custom Form Text:";
            // 
            // lblAccent
            // 
            this.lblAccent.AutoSize = true;
            this.lblAccent.Location = new System.Drawing.Point(219, 45);
            this.lblAccent.Name = "lblAccent";
            this.lblAccent.Size = new System.Drawing.Size(79, 13);
            this.lblAccent.TabIndex = 3;
            this.lblAccent.Text = "Custom Accent";
            // 
            // lblBackColor2
            // 
            this.lblBackColor2.AutoSize = true;
            this.lblBackColor2.Location = new System.Drawing.Point(123, 45);
            this.lblBackColor2.Name = "lblBackColor2";
            this.lblBackColor2.Size = new System.Drawing.Size(66, 13);
            this.lblBackColor2.TabIndex = 3;
            this.lblBackColor2.Text = "Custom BG2";
            // 
            // txtAccent
            // 
            this.txtAccent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(162)))));
            this.txtAccent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAccent.Location = new System.Drawing.Point(234, 61);
            this.txtAccent.Name = "txtAccent";
            this.txtAccent.ReadOnly = true;
            this.txtAccent.Size = new System.Drawing.Size(26, 23);
            this.txtAccent.TabIndex = 2;
            this.txtAccent.TabStop = false;
            this.txtAccent.Text = "";
            // 
            // txtBackColor2
            // 
            this.txtBackColor2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(100)))), ((int)(((byte)(255)))));
            this.txtBackColor2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBackColor2.Location = new System.Drawing.Point(132, 61);
            this.txtBackColor2.Name = "txtBackColor2";
            this.txtBackColor2.ReadOnly = true;
            this.txtBackColor2.Size = new System.Drawing.Size(26, 23);
            this.txtBackColor2.TabIndex = 2;
            this.txtBackColor2.TabStop = false;
            this.txtBackColor2.Text = "";
            // 
            // lblBackColor
            // 
            this.lblBackColor.AutoSize = true;
            this.lblBackColor.Location = new System.Drawing.Point(29, 45);
            this.lblBackColor.Name = "lblBackColor";
            this.lblBackColor.Size = new System.Drawing.Size(60, 13);
            this.lblBackColor.TabIndex = 3;
            this.lblBackColor.Text = "Custom BG";
            // 
            // txtBackColor
            // 
            this.txtBackColor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(152)))), ((int)(((byte)(0)))), ((int)(((byte)(230)))));
            this.txtBackColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBackColor.Location = new System.Drawing.Point(34, 61);
            this.txtBackColor.Name = "txtBackColor";
            this.txtBackColor.ReadOnly = true;
            this.txtBackColor.Size = new System.Drawing.Size(26, 23);
            this.txtBackColor.TabIndex = 2;
            this.txtBackColor.TabStop = false;
            this.txtBackColor.Text = "";
            // 
            // radCustom
            // 
            this.radCustom.AutoSize = true;
            this.radCustom.Location = new System.Drawing.Point(223, 20);
            this.radCustom.Name = "radCustom";
            this.radCustom.Size = new System.Drawing.Size(60, 17);
            this.radCustom.TabIndex = 1;
            this.radCustom.Text = "Custom";
            this.radCustom.UseVisualStyleBackColor = true;
            this.radCustom.CheckedChanged += new System.EventHandler(this.RadCustomCheckedChanged);
            // 
            // radSystem
            // 
            this.radSystem.AutoSize = true;
            this.radSystem.Location = new System.Drawing.Point(150, 20);
            this.radSystem.Name = "radSystem";
            this.radSystem.Size = new System.Drawing.Size(59, 17);
            this.radSystem.TabIndex = 1;
            this.radSystem.Text = "System";
            this.radSystem.UseVisualStyleBackColor = true;
            this.radSystem.CheckedChanged += new System.EventHandler(this.RadSystemCheckedChanged);
            // 
            // radUNA
            // 
            this.radUNA.AutoSize = true;
            this.radUNA.Location = new System.Drawing.Point(86, 20);
            this.radUNA.Name = "radUNA";
            this.radUNA.Size = new System.Drawing.Size(48, 17);
            this.radUNA.TabIndex = 1;
            this.radUNA.Text = "UNA";
            this.radUNA.UseVisualStyleBackColor = true;
            this.radUNA.CheckedChanged += new System.EventHandler(this.RadUNACheckedChanged);
            // 
            // lblTheme
            // 
            this.lblTheme.AutoSize = true;
            this.lblTheme.Location = new System.Drawing.Point(8, 22);
            this.lblTheme.Name = "lblTheme";
            this.lblTheme.Size = new System.Drawing.Size(43, 13);
            this.lblTheme.TabIndex = 0;
            this.lblTheme.Text = "Theme:";
            // 
            // lblCommentColor
            // 
            this.lblCommentColor.AutoSize = true;
            this.lblCommentColor.Location = new System.Drawing.Point(8, 168);
            this.lblCommentColor.Name = "lblCommentColor";
            this.lblCommentColor.Size = new System.Drawing.Size(81, 13);
            this.lblCommentColor.TabIndex = 0;
            this.lblCommentColor.Text = "Comment Color:";
            // 
            // btnOptionsClose
            // 
            this.btnOptionsClose.Location = new System.Drawing.Point(234, 386);
            this.btnOptionsClose.Name = "btnOptionsClose";
            this.btnOptionsClose.Size = new System.Drawing.Size(75, 23);
            this.btnOptionsClose.TabIndex = 1;
            this.btnOptionsClose.Text = "Close";
            this.btnOptionsClose.UseVisualStyleBackColor = true;
            this.btnOptionsClose.Click += new System.EventHandler(this.BtnOptionsCloseClick);
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(325, 417);
            this.Controls.Add(this.btnOptionsClose);
            this.Controls.Add(this.gbASSISTIOptions);
            this.Controls.Add(this.btnOptionsApply);
            this.Controls.Add(this.gbASSISTUNAOptions);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.TopMost = true;
            this.gbASSISTIOptions.ResumeLayout(false);
            this.gbASSISTIOptions.PerformLayout();
            this.gbASSISTUNAOptions.ResumeLayout(false);
            this.gbASSISTUNAOptions.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbASSISTIOptions;
        private System.Windows.Forms.Label lblMaxSize;
        private System.Windows.Forms.Label lblMaxPages;
        private System.Windows.Forms.Label lblMaxInstructions;
        private System.Windows.Forms.Label lblMaxLines;
        private System.Windows.Forms.Button btnOptionsApply;
        private System.Windows.Forms.GroupBox gbASSISTUNAOptions;
        private System.Windows.Forms.MaskedTextBox txtMaxLines;
        private System.Windows.Forms.MaskedTextBox txtMaxInstructions;
        private System.Windows.Forms.MaskedTextBox txtMaxSize;
        private System.Windows.Forms.MaskedTextBox txtMaxPages;
        private System.Windows.Forms.Button btnOptionsClose;
        private System.Windows.Forms.Button btnBackColor;
        private System.Windows.Forms.Label lblBackColor;
        private System.Windows.Forms.RichTextBox txtBackColor;
        private System.Windows.Forms.RadioButton radCustom;
        private System.Windows.Forms.RadioButton radSystem;
        private System.Windows.Forms.RadioButton radUNA;
        private System.Windows.Forms.Label lblCommentColor;
        private System.Windows.Forms.Label lblBackColor2;
        private System.Windows.Forms.Button btnBackColor2;
        private System.Windows.Forms.RichTextBox txtBackColor2;
        private System.Windows.Forms.Button btnAccent;
        private System.Windows.Forms.Label lblAccent;
        private System.Windows.Forms.RichTextBox txtAccent;
        private System.Windows.Forms.Label lblTheme;
        private System.Windows.Forms.Button btnCommentColor;
        private CustomSourceEditor txtCommentColor;
        private System.Windows.Forms.TextBox txtBrowsePRT;
        private System.Windows.Forms.Label lblPRT;
        private System.Windows.Forms.Button btnBrowsePRT;
        private System.Windows.Forms.Label lblFormText;
        private System.Windows.Forms.TextBox txtFormText;
        private System.Windows.Forms.Button btnFormText;
        private System.Windows.Forms.TextBox txtLabelColor;
        private System.Windows.Forms.Button btnLabelColor;
        private System.Windows.Forms.Label lblCustomLabels;
    }
}