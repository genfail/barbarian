namespace MIDI
{
    partial class frmMain
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
            this.components = new System.ComponentModel.Container();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSystemLoad = new System.Windows.Forms.Button();
            this.btnSystemSave = new System.Windows.Forms.Button();
            this.btnSystemDownload = new System.Windows.Forms.Button();
            this.btnSystemUpload = new System.Windows.Forms.Button();
            this.grbLongMessage = new System.Windows.Forms.GroupBox();
            this.btnSetPatch = new System.Windows.Forms.Button();
            this.btnDownload = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnUpload = new System.Windows.Forms.Button();
            this.cboPatch = new System.Windows.Forms.ComboBox();
            this.txtBank = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.grbOutput = new System.Windows.Forms.GroupBox();
            this.btnOutOpenClose = new System.Windows.Forms.Button();
            this.cboMIDIDevices = new System.Windows.Forms.ComboBox();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.grbInput = new System.Windows.Forms.GroupBox();
            this.cboMIDIInDevices = new System.Windows.Forms.ComboBox();
            this.btnInOpenClose = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.tmrSendTimer = new System.Windows.Forms.Timer(this.components);
            this.GroupBox1.SuspendLayout();
            this.grbLongMessage.SuspendLayout();
            this.grbOutput.SuspendLayout();
            this.grbInput.SuspendLayout();
            this.SuspendLayout();
            // 
            // GroupBox1
            // 
            this.GroupBox1.Controls.Add(this.btnSystemLoad);
            this.GroupBox1.Controls.Add(this.btnSystemSave);
            this.GroupBox1.Controls.Add(this.btnSystemDownload);
            this.GroupBox1.Controls.Add(this.btnSystemUpload);
            this.GroupBox1.Location = new System.Drawing.Point(12, 220);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(648, 64);
            this.GroupBox1.TabIndex = 18;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "System Data";
            // 
            // btnSystemLoad
            // 
            this.btnSystemLoad.Location = new System.Drawing.Point(448, 24);
            this.btnSystemLoad.Name = "btnSystemLoad";
            this.btnSystemLoad.Size = new System.Drawing.Size(88, 23);
            this.btnSystemLoad.TabIndex = 3;
            this.btnSystemLoad.Text = "Load";
            this.btnSystemLoad.Click += new System.EventHandler(this.btnSystemLoad_Click);
            // 
            // btnSystemSave
            // 
            this.btnSystemSave.Location = new System.Drawing.Point(344, 24);
            this.btnSystemSave.Name = "btnSystemSave";
            this.btnSystemSave.Size = new System.Drawing.Size(88, 23);
            this.btnSystemSave.TabIndex = 2;
            this.btnSystemSave.Text = "Save";
            this.btnSystemSave.Click += new System.EventHandler(this.btnSystemSave_Click);
            // 
            // btnSystemDownload
            // 
            this.btnSystemDownload.Location = new System.Drawing.Point(216, 24);
            this.btnSystemDownload.Name = "btnSystemDownload";
            this.btnSystemDownload.Size = new System.Drawing.Size(88, 23);
            this.btnSystemDownload.TabIndex = 1;
            this.btnSystemDownload.Text = "Download";
            this.btnSystemDownload.Click += new System.EventHandler(this.btnSystemDownload_Click);
            // 
            // btnSystemUpload
            // 
            this.btnSystemUpload.Location = new System.Drawing.Point(112, 24);
            this.btnSystemUpload.Name = "btnSystemUpload";
            this.btnSystemUpload.Size = new System.Drawing.Size(88, 23);
            this.btnSystemUpload.TabIndex = 0;
            this.btnSystemUpload.Text = "Upload";
            this.btnSystemUpload.Click += new System.EventHandler(this.btnSystemUpload_Click);
            // 
            // grbLongMessage
            // 
            this.grbLongMessage.Controls.Add(this.btnSetPatch);
            this.grbLongMessage.Controls.Add(this.btnDownload);
            this.grbLongMessage.Controls.Add(this.btnOpen);
            this.grbLongMessage.Controls.Add(this.btnUpload);
            this.grbLongMessage.Controls.Add(this.cboPatch);
            this.grbLongMessage.Controls.Add(this.txtBank);
            this.grbLongMessage.Controls.Add(this.btnSave);
            this.grbLongMessage.Location = new System.Drawing.Point(340, 12);
            this.grbLongMessage.Name = "grbLongMessage";
            this.grbLongMessage.Size = new System.Drawing.Size(320, 200);
            this.grbLongMessage.TabIndex = 17;
            this.grbLongMessage.TabStop = false;
            this.grbLongMessage.Text = "Patches";
            // 
            // btnSetPatch
            // 
            this.btnSetPatch.Location = new System.Drawing.Point(94, 57);
            this.btnSetPatch.Name = "btnSetPatch";
            this.btnSetPatch.Size = new System.Drawing.Size(114, 23);
            this.btnSetPatch.TabIndex = 21;
            this.btnSetPatch.Text = "Set Patch";
            this.btnSetPatch.UseVisualStyleBackColor = true;
            this.btnSetPatch.Click += new System.EventHandler(this.btnSetPatch_Click);
            // 
            // btnDownload
            // 
            this.btnDownload.Location = new System.Drawing.Point(48, 160);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(104, 23);
            this.btnDownload.TabIndex = 19;
            this.btnDownload.Text = "Download";
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(168, 160);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(104, 23);
            this.btnOpen.TabIndex = 18;
            this.btnOpen.Text = "Open";
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnUpload
            // 
            this.btnUpload.Location = new System.Drawing.Point(48, 113);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(104, 23);
            this.btnUpload.TabIndex = 17;
            this.btnUpload.Text = "Upload";
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // cboPatch
            // 
            this.cboPatch.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            this.cboPatch.Location = new System.Drawing.Point(168, 30);
            this.cboPatch.Name = "cboPatch";
            this.cboPatch.Size = new System.Drawing.Size(104, 21);
            this.cboPatch.TabIndex = 16;
            this.cboPatch.Text = "1";
            // 
            // txtBank
            // 
            this.txtBank.Location = new System.Drawing.Point(48, 30);
            this.txtBank.Name = "txtBank";
            this.txtBank.Size = new System.Drawing.Size(104, 20);
            this.txtBank.TabIndex = 15;
            this.txtBank.Text = "01";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(168, 113);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(104, 23);
            this.btnSave.TabIndex = 12;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // grbOutput
            // 
            this.grbOutput.Controls.Add(this.btnOutOpenClose);
            this.grbOutput.Controls.Add(this.cboMIDIDevices);
            this.grbOutput.Location = new System.Drawing.Point(12, 12);
            this.grbOutput.Name = "grbOutput";
            this.grbOutput.Size = new System.Drawing.Size(320, 96);
            this.grbOutput.TabIndex = 16;
            this.grbOutput.TabStop = false;
            this.grbOutput.Text = "Output";
            // 
            // btnOutOpenClose
            // 
            this.btnOutOpenClose.Location = new System.Drawing.Point(216, 56);
            this.btnOutOpenClose.Name = "btnOutOpenClose";
            this.btnOutOpenClose.Size = new System.Drawing.Size(88, 24);
            this.btnOutOpenClose.TabIndex = 8;
            this.btnOutOpenClose.Text = "Open";
            this.btnOutOpenClose.Click += new System.EventHandler(this.btnOutOpenClose_Click1);
            // 
            // cboMIDIDevices
            // 
            this.cboMIDIDevices.Location = new System.Drawing.Point(8, 29);
            this.cboMIDIDevices.Name = "cboMIDIDevices";
            this.cboMIDIDevices.Size = new System.Drawing.Size(296, 21);
            this.cboMIDIDevices.TabIndex = 4;
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(12, 292);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(648, 112);
            this.txtLog.TabIndex = 14;
            // 
            // grbInput
            // 
            this.grbInput.Controls.Add(this.cboMIDIInDevices);
            this.grbInput.Controls.Add(this.btnInOpenClose);
            this.grbInput.Location = new System.Drawing.Point(12, 116);
            this.grbInput.Name = "grbInput";
            this.grbInput.Size = new System.Drawing.Size(320, 96);
            this.grbInput.TabIndex = 15;
            this.grbInput.TabStop = false;
            this.grbInput.Text = "Input";
            // 
            // cboMIDIInDevices
            // 
            this.cboMIDIInDevices.Location = new System.Drawing.Point(8, 24);
            this.cboMIDIInDevices.Name = "cboMIDIInDevices";
            this.cboMIDIInDevices.Size = new System.Drawing.Size(296, 21);
            this.cboMIDIInDevices.TabIndex = 10;
            // 
            // btnInOpenClose
            // 
            this.btnInOpenClose.Location = new System.Drawing.Point(216, 56);
            this.btnInOpenClose.Name = "btnInOpenClose";
            this.btnInOpenClose.Size = new System.Drawing.Size(80, 24);
            this.btnInOpenClose.TabIndex = 9;
            this.btnInOpenClose.Text = "Open";
            this.btnInOpenClose.Click += new System.EventHandler(this.btnInOpenClose_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // tmrSendTimer
            // 
            this.tmrSendTimer.Tick += new System.EventHandler(this.tmrSendTimer_Tick);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(682, 420);
            this.Controls.Add(this.GroupBox1);
            this.Controls.Add(this.grbLongMessage);
            this.Controls.Add(this.grbOutput);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.grbInput);
            this.Name = "frmMain";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.GroupBox1.ResumeLayout(false);
            this.grbLongMessage.ResumeLayout(false);
            this.grbLongMessage.PerformLayout();
            this.grbOutput.ResumeLayout(false);
            this.grbInput.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.GroupBox GroupBox1;
        internal System.Windows.Forms.Button btnSystemLoad;
        internal System.Windows.Forms.Button btnSystemSave;
        internal System.Windows.Forms.Button btnSystemDownload;
        internal System.Windows.Forms.Button btnSystemUpload;
        internal System.Windows.Forms.GroupBox grbLongMessage;
        internal System.Windows.Forms.Button btnDownload;
        internal System.Windows.Forms.Button btnOpen;
        internal System.Windows.Forms.Button btnUpload;
        internal System.Windows.Forms.ComboBox cboPatch;
        internal System.Windows.Forms.TextBox txtBank;
        internal System.Windows.Forms.Button btnSave;
        internal System.Windows.Forms.GroupBox grbOutput;
        internal System.Windows.Forms.Button btnOutOpenClose;
        internal System.Windows.Forms.ComboBox cboMIDIDevices;
        internal System.Windows.Forms.TextBox txtLog;
        internal System.Windows.Forms.GroupBox grbInput;
        internal System.Windows.Forms.ComboBox cboMIDIInDevices;
        internal System.Windows.Forms.Button btnInOpenClose;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Timer tmrSendTimer;
        private System.Windows.Forms.Button btnSetPatch;
    }
}

