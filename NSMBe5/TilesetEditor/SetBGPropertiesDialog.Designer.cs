namespace NSMBe5
{
    partial class SetBGPropertiesDialog
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
            this.name_textBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ncgid_UpDown = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.nclid_UpDown = new System.Windows.Forms.NumericUpDown();
            this.nscid_UpDown = new System.Windows.Forms.NumericUpDown();
            this.bmpoffs_UpDown = new System.Windows.Forms.NumericUpDown();
            this.paloffs_UpDown = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.apply_btn = new System.Windows.Forms.Button();
            this.cancel_btn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ncgid_UpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nclid_UpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nscid_UpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bmpoffs_UpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.paloffs_UpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // name_textBox
            // 
            this.name_textBox.Location = new System.Drawing.Point(114, 13);
            this.name_textBox.Name = "name_textBox";
            this.name_textBox.Size = new System.Drawing.Size(292, 22);
            this.name_textBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Name:";
            // 
            // ncgid_UpDown
            // 
            this.ncgid_UpDown.Location = new System.Drawing.Point(114, 41);
            this.ncgid_UpDown.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.ncgid_UpDown.Name = "ncgid_UpDown";
            this.ncgid_UpDown.Size = new System.Drawing.Size(120, 22);
            this.ncgid_UpDown.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "NCG File ID:";
            // 
            // nclid_UpDown
            // 
            this.nclid_UpDown.Location = new System.Drawing.Point(114, 69);
            this.nclid_UpDown.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nclid_UpDown.Name = "nclid_UpDown";
            this.nclid_UpDown.Size = new System.Drawing.Size(120, 22);
            this.nclid_UpDown.TabIndex = 4;
            // 
            // nscid_UpDown
            // 
            this.nscid_UpDown.Location = new System.Drawing.Point(114, 97);
            this.nscid_UpDown.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nscid_UpDown.Name = "nscid_UpDown";
            this.nscid_UpDown.Size = new System.Drawing.Size(120, 22);
            this.nscid_UpDown.TabIndex = 5;
            // 
            // bmpoffs_UpDown
            // 
            this.bmpoffs_UpDown.Location = new System.Drawing.Point(114, 125);
            this.bmpoffs_UpDown.Maximum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.bmpoffs_UpDown.Name = "bmpoffs_UpDown";
            this.bmpoffs_UpDown.Size = new System.Drawing.Size(120, 22);
            this.bmpoffs_UpDown.TabIndex = 6;
            // 
            // paloffs_UpDown
            // 
            this.paloffs_UpDown.Location = new System.Drawing.Point(114, 153);
            this.paloffs_UpDown.Maximum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.paloffs_UpDown.Name = "paloffs_UpDown";
            this.paloffs_UpDown.Size = new System.Drawing.Size(120, 22);
            this.paloffs_UpDown.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 17);
            this.label3.TabIndex = 8;
            this.label3.Text = "NCL File ID:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 99);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 17);
            this.label4.TabIndex = 9;
            this.label4.Text = "NSC File ID:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 127);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 17);
            this.label5.TabIndex = 10;
            this.label5.Text = "Bitmap Offset:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 155);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(98, 17);
            this.label6.TabIndex = 11;
            this.label6.Text = "Palette Offset:";
            // 
            // apply_btn
            // 
            this.apply_btn.Location = new System.Drawing.Point(326, 143);
            this.apply_btn.Name = "apply_btn";
            this.apply_btn.Size = new System.Drawing.Size(80, 32);
            this.apply_btn.TabIndex = 12;
            this.apply_btn.Text = "Apply";
            this.apply_btn.UseVisualStyleBackColor = true;
            this.apply_btn.Click += new System.EventHandler(this.Apply_btn_Click);
            // 
            // cancel_btn
            // 
            this.cancel_btn.Location = new System.Drawing.Point(240, 143);
            this.cancel_btn.Name = "cancel_btn";
            this.cancel_btn.Size = new System.Drawing.Size(80, 32);
            this.cancel_btn.TabIndex = 13;
            this.cancel_btn.Text = "Cancel";
            this.cancel_btn.UseVisualStyleBackColor = true;
            this.cancel_btn.Click += new System.EventHandler(this.Cancel_btn_Click);
            // 
            // SetBGPropertiesDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(416, 186);
            this.Controls.Add(this.cancel_btn);
            this.Controls.Add(this.apply_btn);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.paloffs_UpDown);
            this.Controls.Add(this.bmpoffs_UpDown);
            this.Controls.Add(this.nscid_UpDown);
            this.Controls.Add(this.nclid_UpDown);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ncgid_UpDown);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.name_textBox);
            this.Name = "SetBGPropertiesDialog";
            this.Text = "Set Background Properties";
            ((System.ComponentModel.ISupportInitialize)(this.ncgid_UpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nclid_UpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nscid_UpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bmpoffs_UpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paloffs_UpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox name_textBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown ncgid_UpDown;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nclid_UpDown;
        private System.Windows.Forms.NumericUpDown nscid_UpDown;
        private System.Windows.Forms.NumericUpDown bmpoffs_UpDown;
        private System.Windows.Forms.NumericUpDown paloffs_UpDown;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button apply_btn;
        private System.Windows.Forms.Button cancel_btn;
    }
}