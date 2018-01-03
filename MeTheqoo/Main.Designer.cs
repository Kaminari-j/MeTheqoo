namespace KSHTool
{
    partial class Main
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
			this.tbUrl = new System.Windows.Forms.TextBox();
			this.btnDownload = new System.Windows.Forms.Button();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.statusLabel_ServiceName = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
			this.listBox_Download = new System.Windows.Forms.ListBox();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tbUrl
			// 
			this.tbUrl.Location = new System.Drawing.Point(12, 12);
			this.tbUrl.Name = "tbUrl";
			this.tbUrl.Size = new System.Drawing.Size(569, 19);
			this.tbUrl.TabIndex = 0;
			this.tbUrl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbUrl_KeyDown);
			// 
			// btnDownload
			// 
			this.btnDownload.Location = new System.Drawing.Point(587, 10);
			this.btnDownload.Name = "btnDownload";
			this.btnDownload.Size = new System.Drawing.Size(75, 23);
			this.btnDownload.TabIndex = 1;
			this.btnDownload.Text = "Download";
			this.btnDownload.UseVisualStyleBackColor = true;
			this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.statusLabel_ServiceName});
			this.statusStrip1.Location = new System.Drawing.Point(0, 269);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(673, 23);
			this.statusStrip1.TabIndex = 2;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// statusLabel_ServiceName
			// 
			this.statusLabel_ServiceName.Name = "statusLabel_ServiceName";
			this.statusLabel_ServiceName.Size = new System.Drawing.Size(160, 18);
			this.statusLabel_ServiceName.Text = "statusLabel_ServiceName";
			// 
			// toolStripProgressBar1
			// 
			this.toolStripProgressBar1.Name = "toolStripProgressBar1";
			this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 17);
			// 
			// listBox_Download
			// 
			this.listBox_Download.FormattingEnabled = true;
			this.listBox_Download.ItemHeight = 12;
			this.listBox_Download.Location = new System.Drawing.Point(13, 38);
			this.listBox_Download.Name = "listBox_Download";
			this.listBox_Download.Size = new System.Drawing.Size(648, 220);
			this.listBox_Download.TabIndex = 3;
			// 
			// Main
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(673, 292);
			this.Controls.Add(this.listBox_Download);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.btnDownload);
			this.Controls.Add(this.tbUrl);
			this.Name = "Main";
			this.Text = "sh_downloader";
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

		#endregion

		private System.Windows.Forms.TextBox tbUrl;
		private System.Windows.Forms.Button btnDownload;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel statusLabel_ServiceName;
		public System.Windows.Forms.ListBox listBox_Download;
		private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
	}
}

