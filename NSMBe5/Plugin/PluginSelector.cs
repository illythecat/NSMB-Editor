using NSMBe5.DSFileSystem;
using System;
using System.Windows.Forms;

namespace NSMBe5.Plugin
{
	public partial class PluginSelector : Form
	{
		public static PluginSelector Current = null;

		public PluginSelector()
		{
			InitializeComponent();
			Icon = Properties.Resources.nsmbe;

			romPlugsCb.Checked = Properties.Settings.Default.EnableRomPlugin;

			LoadGridViewElements();

			Current = this;
		}

		private void saveBtn_Click(object sender, EventArgs e)
		{
			if (romPlugsCb.Checked)
				PluginManager.EnableRomPlugin();
			else
				PluginManager.DisableRomPlugin();

			foreach (DataGridViewRow viewRow in pluginGridView.Rows)
			{
				int pluginIndex = (int)viewRow.Cells[0].Value;
				int pluginPriority = int.Parse((string)viewRow.Cells[2].Value);
				bool pluginEnabled = (bool)viewRow.Cells[3].Value;
				if (pluginEnabled)
					PluginManager.EnablePlugin(pluginIndex, pluginPriority);
				else
					PluginManager.DisablePlugin(pluginIndex);
			}

			PluginManager.LoadStageObjects();
			StageObjSettings.Load();

			Properties.Settings.Default.EnableRomPlugin = romPlugsCb.Checked;
			PluginManager.SaveEnabledPlugins();

			Close();
		}

		private void installButton_Click(object sender, EventArgs e)
		{
			OpenFileDialog dialog = new OpenFileDialog();

			dialog.Filter = "NSMBe Plugins (*.dll)|*.dll|All Files (*.*)|*.*";
			dialog.FilterIndex = 1;
			dialog.Multiselect = true;

			DialogResult result = dialog.ShowDialog(this);

			if (result != DialogResult.OK)
				return;

			string pluginPath = PluginManager.GetPluginDirectory();

			if (!System.IO.Directory.Exists(pluginPath))
				System.IO.Directory.CreateDirectory(pluginPath);

			foreach (string path in dialog.FileNames)
			{
				string dest = System.IO.Path.Combine(pluginPath, System.IO.Path.GetFileName(path));

                System.IO.File.Copy(path, dest);

				PluginManager.LoadPlugin(dest);
			}
			
			LoadGridViewElements();
		}

		private void pluginGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex == 2)
			{
				DataGridViewRow viewRow = ((DataGridView)sender).Rows[e.RowIndex];
				if (!int.TryParse((string)viewRow.Cells[2].Value, out _))
				{
					viewRow.Cells[2].Value = "0";
				}
			}
		}

		private void PluginSelector_FormClosed(object sender, FormClosedEventArgs e)
		{
			Current = null;
		}

		private void LoadGridViewElements()
		{
			pluginGridView.Rows.Clear();

			PluginInfo[] infos = PluginManager.GetAvailablePlugins();

			foreach (PluginInfo info in infos)
			{
				pluginGridView.Rows.Add(info.id, info.name, info.priority.ToString(), info.enabled);
			}
		}

		public static void Open()
		{
			if (Current != null)
			{
				Current.Focus();
			}
			else
			{
				new PluginSelector().Show();
			}
		}
	}
}
