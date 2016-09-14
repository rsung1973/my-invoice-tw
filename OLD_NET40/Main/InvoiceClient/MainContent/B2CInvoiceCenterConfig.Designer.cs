namespace InvoiceClient.MainContent
{
    partial class B2CInvoiceCenterConfig
    {
        /// <summary> 
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 元件設計工具產生的程式碼

        /// <summary> 
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnRefresh = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.FailedInvoiceInfo = new System.Windows.Forms.Label();
            this.btnRetry = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.JobStatus = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnRefresh
            // 
            this.btnRefresh.AutoSize = true;
            this.btnRefresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnRefresh.Location = new System.Drawing.Point(570, 47);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(134, 44);
            this.btnRefresh.TabIndex = 18;
            this.btnRefresh.Text = "重新整理";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.FailedInvoiceInfo);
            this.panel1.Location = new System.Drawing.Point(0, 45);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(465, 400);
            this.panel1.TabIndex = 17;
            // 
            // FailedInvoiceInfo
            // 
            this.FailedInvoiceInfo.AutoSize = true;
            this.FailedInvoiceInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FailedInvoiceInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.FailedInvoiceInfo.Location = new System.Drawing.Point(0, 0);
            this.FailedInvoiceInfo.Name = "FailedInvoiceInfo";
            this.FailedInvoiceInfo.Size = new System.Drawing.Size(217, 29);
            this.FailedInvoiceInfo.TabIndex = 9;
            this.FailedInvoiceInfo.Text = "發票資料傳送失敗!!";
            // 
            // btnRetry
            // 
            this.btnRetry.Enabled = false;
            this.btnRetry.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnRetry.Location = new System.Drawing.Point(473, 45);
            this.btnRetry.Name = "btnRetry";
            this.btnRetry.Size = new System.Drawing.Size(84, 39);
            this.btnRetry.TabIndex = 16;
            this.btnRetry.Text = "重送";
            this.btnRetry.UseVisualStyleBackColor = true;
            this.btnRetry.Click += new System.EventHandler(this.btnRetry_Click);
            // 
            // btnPause
            // 
            this.btnPause.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnPause.Location = new System.Drawing.Point(570, 0);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(84, 39);
            this.btnPause.TabIndex = 15;
            this.btnPause.Text = "暫停";
            this.btnPause.UseVisualStyleBackColor = true;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // btnSend
            // 
            this.btnSend.Enabled = false;
            this.btnSend.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnSend.Location = new System.Drawing.Point(473, 0);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(84, 39);
            this.btnSend.TabIndex = 14;
            this.btnSend.Text = "執行";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // JobStatus
            // 
            this.JobStatus.AutoSize = true;
            this.JobStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.JobStatus.Location = new System.Drawing.Point(1, 0);
            this.JobStatus.Name = "JobStatus";
            this.JobStatus.Size = new System.Drawing.Size(133, 29);
            this.JobStatus.TabIndex = 13;
            this.JobStatus.Text = "系統狀態：";
            // 
            // B2CInvoiceCenterConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnRetry);
            this.Controls.Add(this.btnPause);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.JobStatus);
            this.Name = "B2CInvoiceCenterConfig";
            this.Size = new System.Drawing.Size(816, 637);
            this.VisibleChanged += new System.EventHandler(this.InvoiceCenterConfig_VisibleChanged);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label FailedInvoiceInfo;
        private System.Windows.Forms.Button btnRetry;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Label JobStatus;

    }
}
