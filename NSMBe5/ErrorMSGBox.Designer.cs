using System.Drawing;

namespace NSMBe5
{
    partial class ErrorMSGBox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ErrorMSGBox));
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.reload_button = new System.Windows.Forms.Button();
            this.exit_button = new System.Windows.Forms.Button();
            this.continue_button = new System.Windows.Forms.Button();
            this.subtextLabel = new System.Windows.Forms.Label();
            this.textLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.Location = new System.Drawing.Point(12, 128);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(318, 109);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.ImageLocation = "";
            this.pictureBox1.Location = new System.Drawing.Point(12, 14);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(72, 72);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.reload_button);
            this.groupBox1.Controls.Add(this.exit_button);
            this.groupBox1.Controls.Add(this.continue_button);
            this.groupBox1.Location = new System.Drawing.Point(12, 243);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(318, 60);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // reload_button
            // 
            this.reload_button.Location = new System.Drawing.Point(213, 18);
            this.reload_button.Name = "reload_button";
            this.reload_button.Size = new System.Drawing.Size(96, 32);
            this.reload_button.TabIndex = 2;
            this.reload_button.Text = "Reload";
            this.reload_button.UseVisualStyleBackColor = true;
            this.reload_button.Click += new System.EventHandler(this.Reload_button_Click);
            // 
            // exit_button
            // 
            this.exit_button.Location = new System.Drawing.Point(111, 18);
            this.exit_button.Name = "exit_button";
            this.exit_button.Size = new System.Drawing.Size(96, 32);
            this.exit_button.TabIndex = 1;
            this.exit_button.Text = "Exit";
            this.exit_button.UseVisualStyleBackColor = true;
            this.exit_button.Click += new System.EventHandler(this.Exit_button_Click);
            // 
            // continue_button
            // 
            this.continue_button.Location = new System.Drawing.Point(9, 18);
            this.continue_button.Name = "continue_button";
            this.continue_button.Size = new System.Drawing.Size(96, 32);
            this.continue_button.TabIndex = 0;
            this.continue_button.Text = "Continue";
            this.continue_button.UseVisualStyleBackColor = true;
            this.continue_button.Click += new System.EventHandler(this.Continue_button_Click);
            // 
            // subtextLabel
            // 
            this.subtextLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.subtextLabel.AutoSize = true;
            this.subtextLabel.Location = new System.Drawing.Point(12, 100);
            this.subtextLabel.Name = "subtextLabel";
            this.subtextLabel.Size = new System.Drawing.Size(306, 17);
            this.subtextLabel.TabIndex = 3;
            this.subtextLabel.Text = "It is recommend that you reload the application.";
            // 
            // textLabel
            // 
            this.textLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textLabel.Location = new System.Drawing.Point(90, 14);
            this.textLabel.Name = "textLabel";
            this.textLabel.Size = new System.Drawing.Size(240, 84);
            this.textLabel.TabIndex = 4;
            this.textLabel.Text = "An unhandled exception has occured!";
            // 
            // ErrorMSGBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(342, 313);
            this.Controls.Add(this.textLabel);
            this.Controls.Add(this.subtextLabel);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.richTextBox1);
            this.Icon = global::NSMBe5.Properties.Resources.nsmbe;
            this.MinimumSize = new System.Drawing.Size(360, 360);
            this.Name = "ErrorMSGBox";
            this.Text = "Error";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button continue_button;
        private System.Windows.Forms.Button reload_button;
        private System.Windows.Forms.Button exit_button;
        private System.Windows.Forms.Label subtextLabel;
        private System.Windows.Forms.Label textLabel;
    }
}