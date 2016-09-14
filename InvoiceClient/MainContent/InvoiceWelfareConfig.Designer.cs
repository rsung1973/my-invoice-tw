namespace InvoiceClient.MainContent
{
    partial class InvoiceWelfareConfig
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
            this.btnGetWelfareAgency = new System.Windows.Forms.Button();
            this.WelfareInfo = new System.Windows.Forms.TextBox();
            this.btnUpdateWelfare = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.btnAutoWelfare = new System.Windows.Forms.Button();
            this.AutoWelfareInterval = new System.Windows.Forms.TextBox();
            this.cbAutoWelfare = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnGetWelfareAgency
            // 
            this.btnGetWelfareAgency.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnGetWelfareAgency.Location = new System.Drawing.Point(546, 101);
            this.btnGetWelfareAgency.Name = "btnGetWelfareAgency";
            this.btnGetWelfareAgency.Size = new System.Drawing.Size(135, 39);
            this.btnGetWelfareAgency.TabIndex = 17;
            this.btnGetWelfareAgency.Text = "重新下載";
            this.btnGetWelfareAgency.UseVisualStyleBackColor = true;
            this.btnGetWelfareAgency.Click += new System.EventHandler(this.btnGetWelfareAgency_Click);
            // 
            // WelfareInfo
            // 
            this.WelfareInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.WelfareInfo.Location = new System.Drawing.Point(0, 79);
            this.WelfareInfo.Multiline = true;
            this.WelfareInfo.Name = "WelfareInfo";
            this.WelfareInfo.ReadOnly = true;
            this.WelfareInfo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.WelfareInfo.Size = new System.Drawing.Size(483, 337);
            this.WelfareInfo.TabIndex = 16;
            // 
            // btnUpdateWelfare
            // 
            this.btnUpdateWelfare.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnUpdateWelfare.Location = new System.Drawing.Point(546, 51);
            this.btnUpdateWelfare.Name = "btnUpdateWelfare";
            this.btnUpdateWelfare.Size = new System.Drawing.Size(135, 39);
            this.btnUpdateWelfare.TabIndex = 15;
            this.btnUpdateWelfare.Text = "檢查更新";
            this.btnUpdateWelfare.UseVisualStyleBackColor = true;
            this.btnUpdateWelfare.Click += new System.EventHandler(this.btnUpdateWelfare_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label6.Location = new System.Drawing.Point(0, 36);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(277, 29);
            this.label6.TabIndex = 14;
            this.label6.Text = "發票捐贈社福機購資料：";
            // 
            // btnAutoWelfare
            // 
            this.btnAutoWelfare.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnAutoWelfare.Location = new System.Drawing.Point(596, 0);
            this.btnAutoWelfare.Name = "btnAutoWelfare";
            this.btnAutoWelfare.Size = new System.Drawing.Size(84, 39);
            this.btnAutoWelfare.TabIndex = 13;
            this.btnAutoWelfare.Text = "確定";
            this.btnAutoWelfare.UseVisualStyleBackColor = true;
            this.btnAutoWelfare.Click += new System.EventHandler(this.btnAutoWelfare_Click);
            // 
            // AutoWelfareInterval
            // 
            this.AutoWelfareInterval.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.AutoWelfareInterval.Location = new System.Drawing.Point(403, 0);
            this.AutoWelfareInterval.Name = "AutoWelfareInterval";
            this.AutoWelfareInterval.Size = new System.Drawing.Size(87, 35);
            this.AutoWelfareInterval.TabIndex = 12;
            // 
            // cbAutoWelfare
            // 
            this.cbAutoWelfare.AutoSize = true;
            this.cbAutoWelfare.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cbAutoWelfare.Location = new System.Drawing.Point(0, 0);
            this.cbAutoWelfare.Name = "cbAutoWelfare";
            this.cbAutoWelfare.Size = new System.Drawing.Size(438, 33);
            this.cbAutoWelfare.TabIndex = 11;
            this.cbAutoWelfare.Text = "自動更新社福機購資料，間隔(分鐘)：";
            this.cbAutoWelfare.UseVisualStyleBackColor = true;
            this.cbAutoWelfare.CheckedChanged += new System.EventHandler(this.cbAutoWelfare_CheckedChanged);
            // 
            // InvoiceWelfareConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnGetWelfareAgency);
            this.Controls.Add(this.WelfareInfo);
            this.Controls.Add(this.btnUpdateWelfare);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnAutoWelfare);
            this.Controls.Add(this.AutoWelfareInterval);
            this.Controls.Add(this.cbAutoWelfare);
            this.Name = "InvoiceWelfareConfig";
            this.Size = new System.Drawing.Size(800, 600);
            this.VisibleChanged += new System.EventHandler(this.InvoiceWelfareConfig_VisibleChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGetWelfareAgency;
        private System.Windows.Forms.TextBox WelfareInfo;
        private System.Windows.Forms.Button btnUpdateWelfare;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnAutoWelfare;
        private System.Windows.Forms.TextBox AutoWelfareInterval;
        private System.Windows.Forms.CheckBox cbAutoWelfare;


    }
}
