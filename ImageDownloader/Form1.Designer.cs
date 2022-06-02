namespace ImageDownloader
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
            this.btnDownload = new System.Windows.Forms.Button();
            this.lblExistImages = new System.Windows.Forms.Label();
            this.lblNotExistImages = new System.Windows.Forms.Label();
            this.lblNewDownloadImages = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnDownload
            // 
            this.btnDownload.Location = new System.Drawing.Point(12, 133);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(99, 23);
            this.btnDownload.TabIndex = 0;
            this.btnDownload.Text = "Download";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // lblExistImages
            // 
            this.lblExistImages.AutoSize = true;
            this.lblExistImages.Location = new System.Drawing.Point(13, 13);
            this.lblExistImages.Name = "lblExistImages";
            this.lblExistImages.Size = new System.Drawing.Size(50, 13);
            this.lblExistImages.TabIndex = 1;
            this.lblExistImages.Text = "0 Images";
            // 
            // lblNotExistImages
            // 
            this.lblNotExistImages.AutoSize = true;
            this.lblNotExistImages.Location = new System.Drawing.Point(13, 38);
            this.lblNotExistImages.Name = "lblNotExistImages";
            this.lblNotExistImages.Size = new System.Drawing.Size(50, 13);
            this.lblNotExistImages.TabIndex = 2;
            this.lblNotExistImages.Text = "0 Images";
            // 
            // lblNewDownloadImages
            // 
            this.lblNewDownloadImages.AutoSize = true;
            this.lblNewDownloadImages.Location = new System.Drawing.Point(13, 64);
            this.lblNewDownloadImages.Name = "lblNewDownloadImages";
            this.lblNewDownloadImages.Size = new System.Drawing.Size(50, 13);
            this.lblNewDownloadImages.TabIndex = 3;
            this.lblNewDownloadImages.Text = "0 Images";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.lblNewDownloadImages);
            this.Controls.Add(this.lblNotExistImages);
            this.Controls.Add(this.lblExistImages);
            this.Controls.Add(this.btnDownload);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.Label lblExistImages;
        private System.Windows.Forms.Label lblNotExistImages;
        private System.Windows.Forms.Label lblNewDownloadImages;
    }
}

