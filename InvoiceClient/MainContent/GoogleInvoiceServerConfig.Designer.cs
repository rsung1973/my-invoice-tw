namespace InvoiceClient.MainContent
{
    partial class GoogleInvoiceServerConfig
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnInvService = new System.Windows.Forms.Button();
            this.btnAutoInvService = new System.Windows.Forms.Button();
            this.AutoInvServiceInterval = new System.Windows.Forms.TextBox();
            this.cbAutoInvService = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnUninstall = new System.Windows.Forms.Button();
            this.btnInstall = new System.Windows.Forms.Button();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.AutoSize = true;
            this.groupBox3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox3.Controls.Add(this.btnInvService);
            this.groupBox3.Controls.Add(this.btnAutoInvService);
            this.groupBox3.Controls.Add(this.AutoInvServiceInterval);
            this.groupBox3.Controls.Add(this.cbAutoInvService);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.groupBox3.Location = new System.Drawing.Point(0, 290);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(805, 155);
            this.groupBox3.TabIndex = 16;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "GUI PDF Task";
            // 
            // btnInvService
            // 
            this.btnInvService.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnInvService.Location = new System.Drawing.Point(548, 87);
            this.btnInvService.Name = "btnInvService";
            this.btnInvService.Size = new System.Drawing.Size(135, 39);
            this.btnInvService.TabIndex = 16;
            this.btnInvService.Text = "Retrieve";
            this.btnInvService.UseVisualStyleBackColor = true;
            this.btnInvService.Click += new System.EventHandler(this.btnInvService_Click);
            // 
            // btnAutoInvService
            // 
            this.btnAutoInvService.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnAutoInvService.Location = new System.Drawing.Point(598, 34);
            this.btnAutoInvService.Name = "btnAutoInvService";
            this.btnAutoInvService.Size = new System.Drawing.Size(84, 39);
            this.btnAutoInvService.TabIndex = 15;
            this.btnAutoInvService.Text = "Set";
            this.btnAutoInvService.UseVisualStyleBackColor = true;
            this.btnAutoInvService.Click += new System.EventHandler(this.btnAutoInvService_Click);
            // 
            // AutoInvServiceInterval
            // 
            this.AutoInvServiceInterval.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.AutoInvServiceInterval.Location = new System.Drawing.Point(422, 34);
            this.AutoInvServiceInterval.Name = "AutoInvServiceInterval";
            this.AutoInvServiceInterval.Size = new System.Drawing.Size(87, 35);
            this.AutoInvServiceInterval.TabIndex = 14;
            // 
            // cbAutoInvService
            // 
            this.cbAutoInvService.AutoSize = true;
            this.cbAutoInvService.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cbAutoInvService.Location = new System.Drawing.Point(2, 34);
            this.cbAutoInvService.Name = "cbAutoInvService";
            this.cbAutoInvService.Size = new System.Drawing.Size(419, 33);
            this.cbAutoInvService.TabIndex = 13;
            this.cbAutoInvService.Text = "Retrieve GUI PDF interval of mins：";
            this.cbAutoInvService.UseVisualStyleBackColor = true;
            this.cbAutoInvService.CheckedChanged += new System.EventHandler(this.cbAutoInvService_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.AutoSize = true;
            this.groupBox2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox2.Controls.Add(this.btnStop);
            this.groupBox2.Controls.Add(this.btnRun);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.groupBox2.Location = new System.Drawing.Point(0, 145);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(805, 145);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Enable／Disable";
            // 
            // btnStop
            // 
            this.btnStop.AutoSize = true;
            this.btnStop.Enabled = false;
            this.btnStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnStop.Location = new System.Drawing.Point(0, 72);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(323, 44);
            this.btnStop.TabIndex = 12;
            this.btnStop.Text = "Stop E-GUI Service";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnRun
            // 
            this.btnRun.AutoSize = true;
            this.btnRun.Enabled = false;
            this.btnRun.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnRun.Location = new System.Drawing.Point(0, 26);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(323, 44);
            this.btnRun.TabIndex = 11;
            this.btnRun.Text = "Run E-GUI Service";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox1.Controls.Add(this.btnUninstall);
            this.groupBox1.Controls.Add(this.btnInstall);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(805, 145);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Install／Uninstall";
            // 
            // btnUninstall
            // 
            this.btnUninstall.AutoSize = true;
            this.btnUninstall.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnUninstall.Location = new System.Drawing.Point(0, 72);
            this.btnUninstall.Name = "btnUninstall";
            this.btnUninstall.Size = new System.Drawing.Size(307, 44);
            this.btnUninstall.TabIndex = 12;
            this.btnUninstall.Text = "Uninstall Windows Service";
            this.btnUninstall.UseVisualStyleBackColor = true;
            this.btnUninstall.Click += new System.EventHandler(this.btnUninstall_Click);
            // 
            // btnInstall
            // 
            this.btnInstall.AutoSize = true;
            this.btnInstall.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnInstall.Location = new System.Drawing.Point(0, 26);
            this.btnInstall.Name = "btnInstall";
            this.btnInstall.Size = new System.Drawing.Size(310, 44);
            this.btnInstall.TabIndex = 11;
            this.btnInstall.Text = "Install Windows Service";
            this.btnInstall.UseVisualStyleBackColor = true;
            this.btnInstall.Click += new System.EventHandler(this.btnInstall_Click);
            // 
            // GoogleInvoiceServerConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "GoogleInvoiceServerConfig";
            this.Size = new System.Drawing.Size(805, 643);
            this.VisibleChanged += new System.EventHandler(this.InvoiceServerConfig_VisibleChanged);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnInvService;
        private System.Windows.Forms.Button btnAutoInvService;
        private System.Windows.Forms.TextBox AutoInvServiceInterval;
        private System.Windows.Forms.CheckBox cbAutoInvService;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnUninstall;
        private System.Windows.Forms.Button btnInstall;
    }
}
