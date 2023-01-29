using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;

namespace NSMBe5.Plugin
{
	public static class PluginManager
	{
		private class PluginHandle
		{
			public string path;
			public Assembly asm;
			public Type clazz;
			public string name;
			public int priority;
			public bool enabled;
			public bool romScope;
		}

		private static string loadErrorText = "An error occured while trying to load a plugin.";
		private static readonly List<PluginHandle> loadedPlugins = new List<PluginHandle>();
		private static PluginStageObj[] globalStageObjs = new PluginStageObj[0];
		private static int romPluginID = -1;

		public static void Initialize()
		{
			if (Properties.Settings.Default.EnableRomPlugin)
				EnableRomPlugin();
			LoadEnabledPlugins();
			LoadStageObjects();
		}

		public static int LoadPlugin(string pluginPath)
		{
			Assembly pluginAsm;
			Type pluginClass;
			string pluginName;

			try
			{
				pluginAsm = Assembly.LoadFile(pluginPath);
			}
			catch (Exception ex)
			{
				new ErrorMSGBox("", loadErrorText, "Could not load the assembly.", ex.Message).ShowDialog();
				return -1;
			}

			try
			{
				pluginClass = pluginAsm.GetType("Plugin");
			}
			catch (Exception ex)
			{
				new ErrorMSGBox("", loadErrorText, "Could not find the Plugin class.", ex.Message).ShowDialog();
				return -1;
			}

			try
			{
				FieldInfo nameField = pluginClass.GetField("Name", BindingFlags.Public | BindingFlags.Static);
				pluginName = (string)nameField.GetValue(null);
			}
			catch (Exception ex)
			{
				new ErrorMSGBox("", loadErrorText, "Could not get the plugin name.", ex.Message).ShowDialog();
				return -1;
			}

			PluginHandle handle = new PluginHandle();
			handle.path = pluginPath;
			handle.asm = pluginAsm;
			handle.clazz = pluginClass;
			handle.name = pluginName;
			handle.priority = 0;
			handle.romScope = false;

			int pluginIndex = loadedPlugins.Count;
			loadedPlugins.Add(handle);

			return pluginIndex;
		}

		public static void EnableRomPlugin()
		{
			if (romPluginID == -1)
			{
				string pluginPath = Path.Combine(Properties.Settings.Default.ROMFolder, "nsmbe.dll");
				if (File.Exists(pluginPath))
				{
					romPluginID = LoadPlugin(pluginPath);
					loadedPlugins[romPluginID].romScope = true;
				}
			}
			if (romPluginID != -1)
				EnablePlugin(romPluginID, 1000);
		}

		public static void DisableRomPlugin()
		{
			if (romPluginID != -1)
				DisablePlugin(romPluginID);
		}

		public static void EnablePlugin(int pluginIndex, int priority)
		{
			PluginHandle plugin = loadedPlugins[pluginIndex];
			if (plugin.enabled)
				return;

			try
			{
				MethodInfo method = plugin.clazz.GetMethod("OnEnable", BindingFlags.Public | BindingFlags.Static);
				method.Invoke(null, null);
			}
			catch (Exception ex)
			{
				new ErrorMSGBox("", loadErrorText, "Could not call OnEnable().", ex.Message).ShowDialog();
				return;
			}

			plugin.enabled = true;
			plugin.priority = priority;
		}

		public static void DisablePlugin(int pluginIndex)
		{
			PluginHandle plugin = loadedPlugins[pluginIndex];
			if (!plugin.enabled)
				return;

			try
			{
				MethodInfo method = plugin.clazz.GetMethod("OnDisable", BindingFlags.Public | BindingFlags.Static);
				method.Invoke(null, null);
			}
			catch (Exception ex)
			{
				new ErrorMSGBox("", loadErrorText, "Could not call OnDisable().", ex.Message).ShowDialog();
				return;
			}

			plugin.enabled = false;
		}

		public static void LoadStageObjects()
		{
			List<PluginStageObj> stageObjs = new List<PluginStageObj>();
			List<int> stageObjPriorities = new List<int>();

			for (int i = 0; i < loadedPlugins.Count; i++)
			{
				PluginHandle plugin = loadedPlugins[i];
				if (!plugin.enabled)
					continue;

				PluginStageObj[] pluginObjs;

				try
				{
					FieldInfo stageObjsField = plugin.clazz.GetField("StageObjects");
					pluginObjs = (PluginStageObj[])stageObjsField.GetValue(null);
				}
				catch (Exception ex)
				{
					new ErrorMSGBox("", loadErrorText, "Failed to call GetStageObjects().", ex.Message).ShowDialog();
					return;
				}

				foreach (PluginStageObj pluginObj in pluginObjs)
				{
					InitializeStageObject(pluginObj);

					/* If the stageObjs list doesn't contain any object with
					 * the same ID, add our object.
					 * If the list already contains an object with the same ID,
					 * replace it if the plugin has more priority.
					 */
					bool isWeaker = false;
					int replIdx = -1;
					for (int j = 0; j < stageObjs.Count; j++)
					{
						if (stageObjs[j].Settings.ObjectID == pluginObj.Settings.ObjectID)
						{
							int prio = stageObjPriorities[j];
							if (prio > plugin.priority)
								isWeaker = true;
							else
								replIdx = j;
						}
					}

					if (!isWeaker)
					{
						if (replIdx == -1)
						{
							stageObjs.Add(pluginObj);
							stageObjPriorities.Add(plugin.priority);
						}
						else
						{
							stageObjs[replIdx] = pluginObj;
							stageObjPriorities[replIdx] = plugin.priority;
						}
					}
				}
			}

			globalStageObjs = stageObjs.ToArray();
		}

		private static void InitializeStageObject(PluginStageObj pluginObj)
		{
			XmlReader reader = XmlReader.Create(new StringReader(pluginObj.SettingsXML));
			reader.ReadToFollowing("class");
			pluginObj.Settings = StageObjSettings.CreateFromStream(reader);
			reader.Close();
		}

		public static PluginStageObj[] GetStageObjects()
		{
			return globalStageObjs;
		}

		public static string GetPluginDirectory()
		{
			return Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Plugins");
		}

		public static PluginInfo[] GetAvailablePlugins()
		{
			string pluginDir = GetPluginDirectory();

			if (!Directory.Exists(pluginDir))
				return new PluginInfo[0];

			string[] files = Directory.GetFiles(pluginDir, "*.dll", SearchOption.TopDirectoryOnly);
			foreach (string file in files)
			{
				bool loaded = false;
				foreach (PluginHandle plugin in loadedPlugins)
				{
					if (plugin.path == file)
					{
						loaded = true;
						continue;
					}
				}
				if (!loaded)
					LoadPlugin(file);
			}

			List<PluginInfo> infos = new List<PluginInfo>();
			for (int i = 0; i < loadedPlugins.Count; i++)
			{
				PluginHandle plugin = loadedPlugins[i];
				if (plugin.romScope)
					continue;
				infos.Add(new PluginInfo(i, plugin.path, plugin.name, plugin.enabled, plugin.priority));
			}

			return infos.ToArray();
		}

		public static void LoadEnabledPlugins()
		{
			string pluginDir = GetPluginDirectory();

			if (!Directory.Exists(pluginDir))
				return;

			string enabledPluginsData = Properties.Settings.Default.EnabledPlugins;
			if (enabledPluginsData.Length != 0)
			{
				string[] enabledPlugins = enabledPluginsData.Split(';');
				foreach (string pluginConfData in enabledPlugins)
				{
					string[] pluginConf = pluginConfData.Split(',');
					string pluginFile = Path.Combine(pluginDir, pluginConf[0]);
					int pluginPrio = int.Parse(pluginConf[1]);

					if (File.Exists(pluginFile))
					{
						int pluginIndex = LoadPlugin(pluginFile);
						EnablePlugin(pluginIndex, pluginPrio);
					}
				}
			}
		}

		public static void SaveEnabledPlugins()
		{
			string pluginStr = "";
			for (int i = 0; i < loadedPlugins.Count; i++)
			{
				PluginHandle plugin = loadedPlugins[i];
				if (plugin.enabled && !plugin.romScope)
					pluginStr += Path.GetFileName(plugin.path) + "," + plugin.priority + ";";
			}
			if (pluginStr.Length > 0)
				pluginStr = pluginStr.Substring(0, pluginStr.Length - 1);

			Properties.Settings.Default.EnabledPlugins = pluginStr;
			Properties.Settings.Default.Save();
		}
	}
}
