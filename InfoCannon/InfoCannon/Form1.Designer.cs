namespace InfoCannon {
    partial class Form1 {
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSaveSettings = new System.Windows.Forms.Button();
            this.btnProcess = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.label3 = new System.Windows.Forms.Label();
            this.clearStatus = new System.Windows.Forms.Timer(this.components);
            this.btnTest = new System.Windows.Forms.Button();
            this.dpVideosFrom = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.btnPostVideos = new System.Windows.Forms.Button();
            this.lstVideos = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.txtAccessKey = new System.Windows.Forms.TextBox();
            this.lstPageID = new System.Windows.Forms.ListBox();
            this.btnAddPageID = new System.Windows.Forms.Button();
            this.btnRemovePageID = new System.Windows.Forms.Button();
            this.lstSource = new System.Windows.Forms.ListBox();
            this.btnUncheckAll = new System.Windows.Forms.Button();
            this.btnCheckAll = new System.Windows.Forms.Button();
            this.txtExtraText = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(86, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Access Key";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(92, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Page ID(s)";
            // 
            // btnSaveSettings
            // 
            this.btnSaveSettings.Location = new System.Drawing.Point(371, 38);
            this.btnSaveSettings.Name = "btnSaveSettings";
            this.btnSaveSettings.Size = new System.Drawing.Size(60, 23);
            this.btnSaveSettings.TabIndex = 4;
            this.btnSaveSettings.Text = "Save";
            this.btnSaveSettings.UseVisualStyleBackColor = true;
            this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(198, 232);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(113, 23);
            this.btnProcess.TabIndex = 5;
            this.btnProcess.Text = "Gather Videos";
            this.btnProcess.UseVisualStyleBackColor = true;
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripProgressBar1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 475);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(494, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            this.toolStripProgressBar1.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(88, 126);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "VOD Source";
            // 
            // clearStatus
            // 
            this.clearStatus.Interval = 5000;
            this.clearStatus.Tick += new System.EventHandler(this.clearStatus_Tick);
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(371, 9);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(60, 23);
            this.btnTest.TabIndex = 10;
            this.btnTest.Text = "Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // dpVideosFrom
            // 
            this.dpVideosFrom.Location = new System.Drawing.Point(161, 205);
            this.dpVideosFrom.Name = "dpVideosFrom";
            this.dpVideosFrom.Size = new System.Drawing.Size(186, 20);
            this.dpVideosFrom.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(90, 211);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Videos From";
            // 
            // btnPostVideos
            // 
            this.btnPostVideos.Location = new System.Drawing.Point(198, 446);
            this.btnPostVideos.Name = "btnPostVideos";
            this.btnPostVideos.Size = new System.Drawing.Size(113, 23);
            this.btnPostVideos.TabIndex = 14;
            this.btnPostVideos.Text = "Post Videos";
            this.btnPostVideos.UseVisualStyleBackColor = true;
            this.btnPostVideos.Click += new System.EventHandler(this.btnPostVideos_Click);
            // 
            // lstVideos
            // 
            this.lstVideos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstVideos.CheckBoxes = true;
            this.lstVideos.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lstVideos.GridLines = true;
            this.lstVideos.HideSelection = false;
            this.lstVideos.Location = new System.Drawing.Point(12, 261);
            this.lstVideos.MultiSelect = false;
            this.lstVideos.Name = "lstVideos";
            this.lstVideos.ShowItemToolTips = true;
            this.lstVideos.Size = new System.Drawing.Size(470, 179);
            this.lstVideos.TabIndex = 16;
            this.lstVideos.UseCompatibleStateImageBehavior = false;
            this.lstVideos.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Select the videos you would like to publish";
            this.columnHeader1.Width = 345;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Source";
            this.columnHeader2.Width = 100;
            // 
            // txtAccessKey
            // 
            this.txtAccessKey.Location = new System.Drawing.Point(161, 12);
            this.txtAccessKey.Name = "txtAccessKey";
            this.txtAccessKey.Size = new System.Drawing.Size(204, 20);
            this.txtAccessKey.TabIndex = 0;
            // 
            // lstPageID
            // 
            this.lstPageID.FormattingEnabled = true;
            this.lstPageID.Location = new System.Drawing.Point(161, 38);
            this.lstPageID.Name = "lstPageID";
            this.lstPageID.Size = new System.Drawing.Size(204, 82);
            this.lstPageID.TabIndex = 17;
            this.lstPageID.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstPageID_KeyDown);
            // 
            // btnAddPageID
            // 
            this.btnAddPageID.Location = new System.Drawing.Point(371, 67);
            this.btnAddPageID.Name = "btnAddPageID";
            this.btnAddPageID.Size = new System.Drawing.Size(60, 23);
            this.btnAddPageID.TabIndex = 18;
            this.btnAddPageID.Text = "Add";
            this.btnAddPageID.UseVisualStyleBackColor = true;
            this.btnAddPageID.Click += new System.EventHandler(this.btnAddPageID_Click);
            // 
            // btnRemovePageID
            // 
            this.btnRemovePageID.Location = new System.Drawing.Point(371, 96);
            this.btnRemovePageID.Name = "btnRemovePageID";
            this.btnRemovePageID.Size = new System.Drawing.Size(59, 23);
            this.btnRemovePageID.TabIndex = 19;
            this.btnRemovePageID.Text = "Remove";
            this.btnRemovePageID.UseVisualStyleBackColor = true;
            this.btnRemovePageID.Click += new System.EventHandler(this.btnRemovePageID_Click);
            // 
            // lstSource
            // 
            this.lstSource.FormattingEnabled = true;
            this.lstSource.Location = new System.Drawing.Point(161, 126);
            this.lstSource.Name = "lstSource";
            this.lstSource.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lstSource.Size = new System.Drawing.Size(204, 69);
            this.lstSource.TabIndex = 22;
            // 
            // btnUncheckAll
            // 
            this.btnUncheckAll.AccessibleDescription = "";
            this.btnUncheckAll.AccessibleName = "";
            this.btnUncheckAll.BackgroundImage = global::InfoCannon.Properties.Resources.uncheck1;
            this.btnUncheckAll.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnUncheckAll.FlatAppearance.BorderSize = 0;
            this.btnUncheckAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUncheckAll.Location = new System.Drawing.Point(33, 240);
            this.btnUncheckAll.Name = "btnUncheckAll";
            this.btnUncheckAll.Size = new System.Drawing.Size(16, 15);
            this.btnUncheckAll.TabIndex = 21;
            this.btnUncheckAll.TabStop = false;
            this.btnUncheckAll.UseVisualStyleBackColor = true;
            this.btnUncheckAll.Click += new System.EventHandler(this.btnUncheckAll_Click);
            // 
            // btnCheckAll
            // 
            this.btnCheckAll.BackgroundImage = global::InfoCannon.Properties.Resources.Checked;
            this.btnCheckAll.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnCheckAll.FlatAppearance.BorderSize = 0;
            this.btnCheckAll.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.btnCheckAll.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnCheckAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCheckAll.Location = new System.Drawing.Point(12, 240);
            this.btnCheckAll.Name = "btnCheckAll";
            this.btnCheckAll.Size = new System.Drawing.Size(16, 15);
            this.btnCheckAll.TabIndex = 20;
            this.btnCheckAll.TabStop = false;
            this.btnCheckAll.UseVisualStyleBackColor = true;
            this.btnCheckAll.Click += new System.EventHandler(this.btnCheckAll_Click);
            // 
            // txtExtraText
            // 
            this.txtExtraText.Location = new System.Drawing.Point(370, 235);
            this.txtExtraText.Name = "txtExtraText";
            this.txtExtraText.Size = new System.Drawing.Size(112, 20);
            this.txtExtraText.TabIndex = 23;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(368, 218);
            this.label5.Name = "label5";
            this.label5.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 24;
            this.label5.Text = "Extra Text:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 497);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtExtraText);
            this.Controls.Add(this.lstSource);
            this.Controls.Add(this.btnUncheckAll);
            this.Controls.Add(this.btnCheckAll);
            this.Controls.Add(this.btnRemovePageID);
            this.Controls.Add(this.btnAddPageID);
            this.Controls.Add(this.lstPageID);
            this.Controls.Add(this.lstVideos);
            this.Controls.Add(this.btnPostVideos);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dpVideosFrom);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnProcess);
            this.Controls.Add(this.btnSaveSettings);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtAccessKey);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Infowars Video Uploader";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSaveSettings;
        private System.Windows.Forms.Button btnProcess;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Timer clearStatus;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.DateTimePicker dpVideosFrom;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnPostVideos;
        private System.Windows.Forms.ListView lstVideos;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.TextBox txtAccessKey;
        private System.Windows.Forms.ListBox lstPageID;
        private System.Windows.Forms.Button btnAddPageID;
        private System.Windows.Forms.Button btnRemovePageID;
        private System.Windows.Forms.Button btnCheckAll;
        private System.Windows.Forms.Button btnUncheckAll;
        private System.Windows.Forms.ListBox lstSource;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.TextBox txtExtraText;
        private System.Windows.Forms.Label label5;
    }
}

