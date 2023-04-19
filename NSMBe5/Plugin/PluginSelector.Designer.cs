namespace NSMBe5.Plugin
{
	partial class PluginSelector
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
            this.pluginGridView = new System.Windows.Forms.DataGridView();
            this.PluginGridID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PluginGridName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PluginGridPriority = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PluginGridEnabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.saveBtn = new System.Windows.Forms.Button();
            this.romPlugsCb = new System.Windows.Forms.CheckBox();
            this.installButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pluginGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // pluginGridView
            // 
            this.pluginGridView.AllowUserToAddRows = false;
            this.pluginGridView.AllowUserToDeleteRows = false;
            this.pluginGridView.AllowUserToResizeRows = false;
            this.pluginGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pluginGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.pluginGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PluginGridID,
            this.PluginGridName,
            this.PluginGridPriority,
            this.PluginGridEnabled});
            this.pluginGridView.Location = new System.Drawing.Point(9, 10);
            this.pluginGridView.Margin = new System.Windows.Forms.Padding(2);
            this.pluginGridView.Name = "pluginGridView";
            this.pluginGridView.RowHeadersWidth = 51;
            this.pluginGridView.RowTemplate.Height = 24;
            this.pluginGridView.Size = new System.Drawing.Size(260, 238);
            this.pluginGridView.TabIndex = 0;
            this.pluginGridView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.pluginGridView_CellEndEdit);
            // 
            // PluginGridID
            // 
            this.PluginGridID.HeaderText = "ID";
            this.PluginGridID.MinimumWidth = 6;
            this.PluginGridID.Name = "PluginGridID";
            this.PluginGridID.ReadOnly = true;
            this.PluginGridID.Visible = false;
            this.PluginGridID.Width = 125;
            // 
            // PluginGridName
            // 
            this.PluginGridName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.PluginGridName.HeaderText = "Name";
            this.PluginGridName.MinimumWidth = 6;
            this.PluginGridName.Name = "PluginGridName";
            this.PluginGridName.ReadOnly = true;
            // 
            // PluginGridPriority
            // 
            this.PluginGridPriority.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.PluginGridPriority.HeaderText = "Priority";
            this.PluginGridPriority.MinimumWidth = 6;
            this.PluginGridPriority.Name = "PluginGridPriority";
            this.PluginGridPriority.Width = 63;
            // 
            // PluginGridEnabled
            // 
            this.PluginGridEnabled.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.PluginGridEnabled.HeaderText = "Enabled";
            this.PluginGridEnabled.MinimumWidth = 6;
            this.PluginGridEnabled.Name = "PluginGridEnabled";
            this.PluginGridEnabled.Width = 52;
            // 
            // saveBtn
            // 
            this.saveBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveBtn.Location = new System.Drawing.Point(207, 256);
            this.saveBtn.Margin = new System.Windows.Forms.Padding(2);
            this.saveBtn.Name = "saveBtn";
            this.saveBtn.Size = new System.Drawing.Size(62, 30);
            this.saveBtn.TabIndex = 1;
            this.saveBtn.Text = "Save";
            this.saveBtn.UseVisualStyleBackColor = true;
            this.saveBtn.Click += new System.EventHandler(this.saveBtn_Click);
            // 
            // romPlugsCb
            // 
            this.romPlugsCb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.romPlugsCb.AutoSize = true;
            this.romPlugsCb.Location = new System.Drawing.Point(9, 262);
            this.romPlugsCb.Margin = new System.Windows.Forms.Padding(2);
            this.romPlugsCb.Name = "romPlugsCb";
            this.romPlugsCb.Size = new System.Drawing.Size(123, 17);
            this.romPlugsCb.TabIndex = 2;
            this.romPlugsCb.Text = "Enable ROM plugins";
            this.romPlugsCb.UseVisualStyleBackColor = true;
            // 
            // installButton
            // 
            this.installButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.installButton.Location = new System.Drawing.Point(138, 256);
            this.installButton.Name = "installButton";
            this.installButton.Size = new System.Drawing.Size(64, 30);
            this.installButton.TabIndex = 3;
            this.installButton.Text = "Install";
            this.installButton.UseVisualStyleBackColor = true;
            this.installButton.Click += new System.EventHandler(this.installButton_Click);
            // 
            // PluginSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(278, 296);
            this.Controls.Add(this.installButton);
            this.Controls.Add(this.romPlugsCb);
            this.Controls.Add(this.saveBtn);
            this.Controls.Add(this.pluginGridView);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "PluginSelector";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Plugin Selector";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PluginSelector_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.pluginGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.DataGridView pluginGridView;
		private System.Windows.Forms.Button saveBtn;
		private System.Windows.Forms.CheckBox romPlugsCb;
		private System.Windows.Forms.DataGridViewTextBoxColumn PluginGridID;
		private System.Windows.Forms.DataGridViewTextBoxColumn PluginGridName;
		private System.Windows.Forms.DataGridViewTextBoxColumn PluginGridPriority;
		private System.Windows.Forms.DataGridViewCheckBoxColumn PluginGridEnabled;
        private System.Windows.Forms.Button installButton;
    }
}