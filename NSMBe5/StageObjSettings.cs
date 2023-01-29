/*
*   This file is part of NSMB Editor 5.
*
*   NSMB Editor 5 is free software: you can redistribute it and/or modify
*   it under the terms of the GNU General Public License as published by
*   the Free Software Foundation, either version 3 of the License, or
*   (at your option) any later version.
*
*   NSMB Editor 5 is distributed in the hope that it will be useful,
*   but WITHOUT ANY WARRANTY; without even the implied warranty of
*   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
*   GNU General Public License for more details.
*
*   You should have received a copy of the GNU General Public License
*   along with NSMB Editor 5.  If not, see <http://www.gnu.org/licenses/>.
*/

using NSMBe5.Plugin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Xml;

namespace NSMBe5
{
	public class StageObjSettings
	{
		public static readonly List<StageObjSettings> datas = new List<StageObjSettings>();
		public static readonly List<int> categoryIds = new List<int>();
		public static readonly List<string> categories = new List<string>();
		public static readonly Dictionary<int, List<int>> objectInCategories = new Dictionary<int, List<int>>();
		public static string directory = Path.GetDirectoryName(Application.ExecutablePath);
		public static string path = Path.Combine(directory, "stageobjsettings.xml");
		
		public readonly int ObjectID;
		public readonly int CategoryID;
		public readonly string Name;
		public readonly string Notes;
		public readonly List<StageObjSettingsField> Fields;

		StageObjSettings(int objectID, int categoryID, string name, string notes, List<StageObjSettingsField> fields)
		{
			ObjectID = objectID;
			CategoryID = categoryID;
			Name = name;
			Notes = notes;
			Fields = fields;
		}

		public static string DownloadWebPage(string Url)
		{
			// Open a connection
			HttpWebRequest WebRequestObject = (HttpWebRequest)HttpWebRequest.Create(Url);

			// You can also specify additional header values like 
			// the user agent or the referer:
			WebRequestObject.UserAgent = "NSMBe/" + Version.GetString();
			WebRequestObject.Referer = "";

			// Request response:
			WebResponse Response = WebRequestObject.GetResponse();

			// Open data stream:
			Stream WebStream = Response.GetResponseStream();

			// Create reader object:
			StreamReader Reader = new StreamReader(WebStream);

			// Read the entire stream content:
			string PageContent = Reader.ReadToEnd();

			// Cleanup
			Reader.Close();
			WebStream.Close();
			Response.Close();

			return PageContent;
		}

		public static void Update()
		{
			try
			{
				string data = DownloadWebPage(Properties.Resources.StageObjSettingsProviderURL);

				if (data.Trim() == "")
				{
					new ErrorMSGBox("", "", "", "Got empty data").ShowDialog();
					return;
				}

				if (!Directory.Exists(directory))
					Directory.CreateDirectory(directory);

				File.WriteAllText(path, data);

				Load();
				MessageBox.Show(LanguageManager.Get("SpriteData", "Updated"), "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			catch (Exception e)
			{
				new ErrorMSGBox(LanguageManager.Get("SpriteData", "ErrorTitle"), string.Format(LanguageManager.Get("SpriteData", "ErrorUpdate"), "\n" + e.Message), "In this case it is recommended that you continue.", e.ToString()).ShowDialog();
			}
		}

		public static void Load()
		{
			if (!ROM.isNSMBRom)
				return;

			//Delete existing
			datas.Clear();
			categoryIds.Clear();
			categories.Clear();
			objectInCategories.Clear();

			if (!File.Exists(path))
			{
				if (MessageBox.Show(LanguageManager.Get("SpriteData", "Prompt"), LanguageManager.Get("SpriteData", "PromptTitle"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
					Update();
				return;
			}
			try
			{
				FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
				XmlReader xmlr = XmlReader.Create(fs);

				string newSettingsPath = Path.Combine(directory, "stageobjsettings_new.xml");
				bool newSettingsExists = File.Exists(newSettingsPath);
				string patchSettingsPath = Path.Combine(Properties.Settings.Default.ROMFolder, "stageobjsettings_patch.xml");
				bool patchSettingsExists = File.Exists(patchSettingsPath);

				xmlr.ReadToFollowing("category");
				do
				{
					int id = int.Parse(xmlr.GetAttribute("id"));
					categoryIds.Add(id);
					categories.Add(xmlr.ReadElementContentAsString());
					objectInCategories.Add(id, new List<int>());
				}
				while (xmlr.ReadToNextSibling("category"));

				// plugins
				foreach (PluginStageObj stageObj in PluginManager.GetStageObjects())
				{
					datas.Add(stageObj.Settings);
				}

				// patches (rom directory)
				if (patchSettingsExists)
				{
					FileStream fs_rom = new FileStream(patchSettingsPath, FileMode.Open, FileAccess.Read, FileShare.Read);
					XmlReader xmlr_rom = XmlReader.Create(fs_rom);
					while (xmlr_rom.ReadToFollowing("class"))
					{
						StageObjSettings d = CreateFromStream(xmlr_rom);
						if (GetObject(d.ObjectID) == null)
							datas.Add(d);
					}
					xmlr_rom.Close();
					fs_rom.Close();
				}

				// default override (exe directory)
				if (newSettingsExists)
				{
					FileStream fs_new = new FileStream(newSettingsPath, FileMode.Open, FileAccess.Read, FileShare.Read);
					XmlReader xmlr_new = XmlReader.Create(fs_new);
					while (xmlr_new.ReadToFollowing("class"))
					{
						StageObjSettings d = CreateFromStream(xmlr_new);
						if (GetObject(d.ObjectID) == null)
							datas.Add(d);
					}
					xmlr_new.Close();
					fs_new.Close();
				}

				// default (exe directory, updatable)
				while (xmlr.ReadToFollowing("class"))
				{
					StageObjSettings d = CreateFromStream(xmlr);
					if (GetObject(d.ObjectID) == null)
						datas.Add(d);
				}

				xmlr.Close();
				fs.Close();
			}
			catch (Exception e)
			{
				new ErrorMSGBox(LanguageManager.Get("SpriteData", "ErrorTitle"), LanguageManager.Get("SpriteData", "ErrorParse"), "", e.ToString()).ShowDialog();
				datas.Clear();
			}
		}
		
		public static StageObjSettings CreateFromStream(XmlReader xmlr)
		{
			int objectID;
			int categoryID;
			string name;
			string notes;
			List<StageObjSettingsField> fields = new List<StageObjSettingsField>();

			objectID = int.Parse(xmlr.GetAttribute("id"));
			xmlr.ReadToFollowing("name");
			name = xmlr.ReadElementContentAsString();
			xmlr.ReadToFollowing("category");
			categoryID = int.Parse(xmlr.GetAttribute("id"));
			xmlr.ReadToFollowing("notes");
			notes = xmlr.ReadElementContentAsString();

			while (xmlr.ReadToNextSibling("field"))
			{
				StageObjSettingsField f = new StageObjSettingsField();
				f.display = xmlr.GetAttribute("type");
				f.name = xmlr.GetAttribute("name");
				f.notes = xmlr.GetAttribute("notes");
				string nybbles = xmlr.GetAttribute("id");
				if (nybbles.Contains("-"))
				{
					string[] nybbles2 = nybbles.Split('-');
					f.startNibble = int.Parse(nybbles2[0]);
					f.endNibble = int.Parse(nybbles2[1]);
				}
				else
				{
					f.startNibble = f.endNibble = int.Parse(nybbles);
				}
				string values = xmlr.GetAttribute("values");
				switch (f.display)
				{
				case "list":
					string[] items = values.Split(',');
					f.values = new int[items.Length];
					f.strings = new string[items.Length];

					for (int j = 0; j < items.Length; j++)
					{
						string[] lulz = items[j].Split(new char[] { '=' });
						f.values[j] = int.Parse(lulz[0]);
						f.strings[j] = lulz[1];
					}
					break;
				case "signedvalue":
					if (values.Trim() == "")
						f.data = 0;
					else
						f.data = int.Parse(values);
					break;
				case "value":
					if (values.Trim() == "")
						f.data = 0;
					else
						f.data = int.Parse(values);
					break;
				case "checkbox":
					if (values.Trim() == "")
						f.data = 1;
					else
						f.data = int.Parse(values);
					break;
				}
				fields.Add(f);
			}

			return new StageObjSettings(objectID, categoryID, name, notes, fields);
		}

		public static StageObjSettings GetObject(int objectID)
		{
			foreach (StageObjSettings settings in datas)
			{
				if (settings.ObjectID == objectID)
					return settings;
			}
			return null;
		}

		public static string GetObjectName(int objectID)
		{
			StageObjSettings settings = GetObject(objectID);
			if (settings == null)
				return "";
			return settings.Name;
		}

		public class StageObjSettingsField
		{
			public string name;
			public string notes;
			public string display;
			public int startNibble;
			public int endNibble;

			//For list
			public int[] values;
			public string[] strings;

			//For values and checkboxes
			public int data;

			public int getBitCount()
			{
				return endNibble - startNibble + 1;
			}

			public int getMin()
			{
				int value = 0;
				if (display == "signedvalue")
					value = -((1 << (getBitCount()) - 1));
				if (display == "value" || display == "signedvalue")
					value += this.data;
				return value;
			}

			public int getMax()
			{
				int value = (1 << (getBitCount())) - 1;
				if (display == "signedvalue")
					value = (1 << (getBitCount() - 1)) - 1;
				if (display == "value" || display == "signedvalue")
					value += this.data;
				return value;
			}

			public int getValue(byte[] data)
			{
				byte[] nibbles = new byte[48];
				for (int i = 0; i < 6; i++)
				{
					nibbles[8 * i] = (byte)(data[i] >> 7);
					nibbles[8 * i + 1] = (byte)((data[i] >> 6) & 0x1);
					nibbles[8 * i + 2] = (byte)((data[i] >> 5) & 0x1);
					nibbles[8 * i + 3] = (byte)((data[i] >> 4) & 0x1);
					nibbles[8 * i + 4] = (byte)((data[i] >> 3) & 0x1);
					nibbles[8 * i + 5] = (byte)((data[i] >> 2) & 0x1);
					nibbles[8 * i + 6] = (byte)((data[i] >> 1) & 0x1);
					nibbles[8 * i + 7] = (byte)(data[i] & 0x1);
				}

				int res = 0;
				for (int i = startNibble - 1; i <= endNibble - 1; i++)
				{
					res = res << 1 | nibbles[i];
				}

				if (display == "signedvalue" && res > getMax())
				{
					res -= (1 << getBitCount());
				}

				if (display == "value" || display == "signedvalue")
					res += this.data;

				if (display == "checkbox")
					res /= this.data;

				return res;
			}

			public void setValue(int b, byte[] data)
			{
				byte[] nibbles = new byte[48];
				for (int i = 0; i < 6; i++)
				{
					nibbles[8 * i] = (byte)(data[i] >> 7);
					nibbles[8 * i + 1] = (byte)((data[i] >> 6) & 0x1);
					nibbles[8 * i + 2] = (byte)((data[i] >> 5) & 0x1);
					nibbles[8 * i + 3] = (byte)((data[i] >> 4) & 0x1);
					nibbles[8 * i + 4] = (byte)((data[i] >> 3) & 0x1);
					nibbles[8 * i + 5] = (byte)((data[i] >> 2) & 0x1);
					nibbles[8 * i + 6] = (byte)((data[i] >> 1) & 0x1);
					nibbles[8 * i + 7] = (byte)(data[i] & 0x1);
				}

				if (display == "value" || display == "signedvalue")
					b -= this.data;

				if (display == "checkbox")
					b *= this.data;

				for (int i = endNibble - 1; i >= startNibble - 1; i--)
				{
					nibbles[i] = (byte)(b & 0x1);
					b = b >> 1;
				}

				for (int i = 0; i < 6; i++)
				{
					data[i] = (byte)(nibbles[8 * i] << 7 | nibbles[8 * i + 1] << 6 | nibbles[8 * i + 2] << 5 | nibbles[8 * i + 3] << 4 | nibbles[8 * i + 4] << 3 | nibbles[8 * i + 5] << 2 | nibbles[8 * i + 6] << 1 | nibbles[8 * i + 7]);
				}
			}
		}

		public class StageObjSettingsEditor : TableLayoutPanel
		{
			Dictionary<StageObjSettingsField, Control> controls = new Dictionary<StageObjSettingsField, Control>();

			List<LevelItem> objects;
			NSMBStageObj s;
			StageObjSettings sd;
			LevelEditorControl EdControl;
			public bool updating = false;

			public StageObjSettingsEditor(List<LevelItem> objects, StageObjSettings sd, LevelEditorControl EdControl)
			{
				this.SizeChanged += new EventHandler(this_SizeChanged);
				updating = true;
				this.ColumnCount = 3;
				//Talbe layout panel doesn't automatically create row or column styles
				this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
				this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
				this.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
				this.RowCount = sd.Fields.Count;
				for (int l = 0; l < this.RowCount; l++)
					this.RowStyles.Add(new RowStyle(SizeType.Absolute));
				this.AutoSize = true;
				this.AutoSizeMode = AutoSizeMode.GrowAndShrink;

				this.objects = objects;
				foreach (LevelItem obj in objects)
				{
					if (obj is NSMBStageObj)
					{
						s = obj as NSMBStageObj;
						break;
					}
				}
				this.sd = sd;
				this.Dock = DockStyle.Fill;
				this.EdControl = EdControl;

				int row = 0;
				foreach (StageObjSettingsField v in sd.Fields)
				{
					Control c = CreateControlFor(v);
					c.Anchor = AnchorStyles.Left | AnchorStyles.Right;
					if (c is CheckBox || c is Label)
					{
						c.Font = new System.Drawing.Font(c.Font.FontFamily, c.Font.Size * 0.9F);
						this.Controls.Add(c, 0, row);
						this.RowStyles[row].Height = 25;
						if (v.notes == "")
							this.SetColumnSpan(c, 3);
						else
						{
							NotesCtrl note = new NotesCtrl();
							this.Controls.Add(note, 2, row);
							note.Text = v.notes;
						}
					}
					else
					{
						this.Controls.Add(c, 1, row);
						Label l = new Label();
						l.Text = v.name;
						l.Font = new System.Drawing.Font(l.Font.FontFamily, l.Font.Size * 0.9F);
						l.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
						l.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
						this.Controls.Add(l, 0, row);
						this.RowStyles[row].Height = 25;
						if (v.notes == "")
							this.SetColumnSpan(c, 2);
						else
						{
							NotesCtrl note = new NotesCtrl();
							this.Controls.Add(note, 2, row);
							note.Text = v.notes;
						}
					}
					row++;
					controls.Add(v, c);
				}
				updating = false;
			}

			public void this_SizeChanged(object sender, EventArgs e)
			{
//                Console.Out.WriteLine(this.Width.ToString());
				if (this.Width != 200)
				{
					for (int l = 0; l < this.RowCount; l++)
					{
						Control ctrl = this.GetControlFromPosition(0, l);
						if (ctrl is Label)
						{
							ctrl.MaximumSize = new System.Drawing.Size(this.Width / 2, 0);
							this.RowStyles[l].Height = Math.Max(ctrl.PreferredSize.Height, this.GetControlFromPosition(1, l).Height) + 4;
						}
					}
				}
			}

			public void UpdateData()
			{
				updating = true;
				foreach (StageObjSettingsField v in sd.Fields)
					updateValue(v);
				updating = false;
			}

			private Control CreateControlFor(StageObjSettingsField v)
			{
//                Console.WriteLine(v.display + " " + v.name);
				if (v.display == "checkbox")
				{
					CheckBox c = new CheckBox();
					c.Checked = v.getValue(s.Data) == 1;
					c.Text = v.name;
					c.CheckedChanged += new EventHandler(saveData);
					return c;
				}
				else if (v.display == "list")
				{
					ComboBox c = new ComboBox();
					c.DropDownStyle = ComboBoxStyle.DropDownList;
					c.Items.AddRange(v.strings);
					try
					{
						c.SelectedIndex = Array.IndexOf(v.values, v.getValue(s.Data));
					}
					catch (ArgumentOutOfRangeException) { } //just in case
					//c.SelectedIndexChanged += new EventHandler(saveData);
					c.SelectionChangeCommitted += new EventHandler(saveData);
					//c.DropDownClosed += new EventHandler(saveData);
					
					return c;
				}
				else if (v.display == "label")
				{
					Label c = new Label();
					c.Text = v.name;
					return c;
				}
				else if (v.display == "binary")
				{
					BinaryEdit c = new BinaryEdit();
					c.CheckBoxCount = v.getBitCount();
					c.value = v.getValue(s.Data);
					c.ValueChanged += new EventHandler(saveData);
					return c;
				}
				else
				{
					NumericUpDown c = new NumericUpDown();
					c.Minimum = v.getMin();
					c.Maximum = v.getMax();
					c.Value = v.getValue(s.Data);
					c.ValueChanged += new EventHandler(saveData);
					return c;
				}
			}

			public void updateValue(StageObjSettingsField v)
			{
				Control c = controls[v];
				int value = v.getValue(s.Data);
				if (c is CheckBox)
					(c as CheckBox).Checked = value == 1;
				if (c is ComboBox)
					(c as ComboBox).SelectedIndex = Array.IndexOf(v.values, value);
				if (c is BinaryEdit)
					(c as BinaryEdit).value = value;
				if (c is NumericUpDown)
					(c as NumericUpDown).Value = value;
			}
			
			public void saveData(object sender, EventArgs e)
			{
				byte[] d = s.Data.Clone() as byte[];
				int index = 0;
				foreach(StageObjSettingsField sv in controls.Keys)
				{
					int val = 0;
					if (controls[sv] is NumericUpDown)
						val = (int)(controls[sv] as NumericUpDown).Value;
					else if (controls[sv] is ComboBox) {
						int se = (controls[sv] as ComboBox).SelectedIndex;
						if (se == -1)
							val = 0;
						else
							val = sd.Fields[index].values[(controls[sv] as ComboBox).SelectedIndex];
					}
					else if (controls[sv] is CheckBox)
						val = (controls[sv] as CheckBox).Checked ? 1 : 0;
					else if (controls[sv] is BinaryEdit)
						val = (controls[sv] as BinaryEdit).value;
					sv.setValue(val, d);
					index++;
				}

				if (!updating && sender != null)
				{
					EdControl.UndoManager.Do(new ChangeStageObjSettingsAction(objects, d));
				}
			}
		}
	}
}
