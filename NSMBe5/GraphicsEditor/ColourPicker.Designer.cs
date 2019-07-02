namespace NSMBe5 {
    partial class ColourPicker {
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
            this.cancelButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.r_UpDown = new System.Windows.Forms.NumericUpDown();
            this.g_UpDown = new System.Windows.Forms.NumericUpDown();
            this.b_UpDown = new System.Windows.Forms.NumericUpDown();
            this.colourPickerControl1 = new NSMBe5.ColourPickerControl();
            ((System.ComponentModel.ISupportInitialize)(this.r_UpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.g_UpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.b_UpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(257, 148);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(4);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(100, 28);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "<cancelButton>";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // saveButton
            // 
            this.saveButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.saveButton.Location = new System.Drawing.Point(149, 148);
            this.saveButton.Margin = new System.Windows.Forms.Padding(4);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(100, 28);
            this.saveButton.TabIndex = 3;
            this.saveButton.Text = "<saveButton>";
            this.saveButton.UseVisualStyleBackColor = true;
            // 
            // r_UpDown
            // 
            this.r_UpDown.Location = new System.Drawing.Point(288, 15);
            this.r_UpDown.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.r_UpDown.Name = "r_UpDown";
            this.r_UpDown.Size = new System.Drawing.Size(68, 22);
            this.r_UpDown.TabIndex = 4;
            this.r_UpDown.ValueChanged += new System.EventHandler(this.RBG_UpDown_ValueChanged);
            // 
            // g_UpDown
            // 
            this.g_UpDown.Location = new System.Drawing.Point(288, 43);
            this.g_UpDown.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.g_UpDown.Name = "g_UpDown";
            this.g_UpDown.Size = new System.Drawing.Size(68, 22);
            this.g_UpDown.TabIndex = 5;
            this.g_UpDown.ValueChanged += new System.EventHandler(this.RBG_UpDown_ValueChanged);
            // 
            // b_UpDown
            // 
            this.b_UpDown.Location = new System.Drawing.Point(288, 71);
            this.b_UpDown.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.b_UpDown.Name = "b_UpDown";
            this.b_UpDown.Size = new System.Drawing.Size(68, 22);
            this.b_UpDown.TabIndex = 6;
            this.b_UpDown.ValueChanged += new System.EventHandler(this.RBG_UpDown_ValueChanged);
            // 
            // colourPickerControl1
            // 
            this.colourPickerControl1.Location = new System.Drawing.Point(16, 15);
            this.colourPickerControl1.Margin = new System.Windows.Forms.Padding(5);
            this.colourPickerControl1.Name = "colourPickerControl1";
            this.colourPickerControl1.Size = new System.Drawing.Size(341, 126);
            this.colourPickerControl1.TabIndex = 1;
            this.colourPickerControl1.Value = 0;
            this.colourPickerControl1.ValueChanged += new System.EventHandler(this.ColourPickerControl_ValueChanged);
            // 
            // ColourPicker
            // 
            this.AcceptButton = this.saveButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(373, 191);
            this.Controls.Add(this.b_UpDown);
            this.Controls.Add(this.g_UpDown);
            this.Controls.Add(this.r_UpDown);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.colourPickerControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ColourPicker";
            this.Text = "<_TITLE>";
            ((System.ComponentModel.ISupportInitialize)(this.r_UpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.g_UpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.b_UpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ColourPickerControl colourPickerControl1;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.NumericUpDown r_UpDown;
        private System.Windows.Forms.NumericUpDown g_UpDown;
        private System.Windows.Forms.NumericUpDown b_UpDown;
    }
}