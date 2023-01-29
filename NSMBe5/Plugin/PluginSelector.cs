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

			PluginInfo[] infos = PluginManager.GetAvailablePlugins();

			foreach (PluginInfo info in infos)
			{
				pluginGridView.Rows.Add(info.id, info.name, info.priority.ToString(), info.enabled);
			}

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
