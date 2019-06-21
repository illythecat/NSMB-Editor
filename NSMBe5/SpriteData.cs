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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace NSMBe5
{
    public class SpriteData
    {
        public static Dictionary<int, SpriteData> datas = new Dictionary<int, SpriteData>();
        public static Dictionary<int, SpriteData> datas2 = new Dictionary<int, SpriteData>();
        public static List<string> spriteNames = new List<string>();
        public static List<int> categoryIds = new List<int>();
        public static List<string> categories = new List<string>();
        public static Dictionary<int, List<int>> spritesInCategory = new Dictionary<int, List<int>>();
        public static string directory = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
        public static string path = System.IO.Path.Combine(directory, "spritedata.xml");
        
        int spriteID;
        int categoryID;
        string name;
        string notes;
        public List<SpriteDataField> fields = new List<SpriteDataField>();

        public static string DownloadWebPage(string Url)
        {
            // Open a connection
            HttpWebRequest WebRequestObject = (HttpWebRequest)HttpWebRequest.Create(Url);

            // You can also specify additional header values like 
            // the user agent or the referer:
            WebRequestObject.UserAgent = "NSMBe/5.3";
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
                //classIDforSprite part goes first as if the file is missing it will throw before downloading.
                #region Get "class IDs for sprite" dictionary from file
                Dictionary<int, int> classIDforSprite = new Dictionary<int, int>();
                using (StreamReader sr = new StreamReader(Path.Combine(directory, "classIDforSprite.dict")))
                {
                    string _line;
                    while ((_line = sr.ReadLine()) != null)
                    {
                        string[] keyvalue = _line.Split('=');
                        classIDforSprite.Add(int.Parse(keyvalue[0]), int.Parse(keyvalue[1]));
                    }
                }
                #endregion

                #region Download spritedata.xml
                string data = DownloadWebPage("http://nsmbhd.net/spritexml.php");

                if (data.Trim() == "")
                {
                    new ErrorMSGBox("", "", "", "Got empty data").ShowDialog();
                    return;
                }

                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(data);
                sw.Close();
                fs.Close();
                #endregion

                #region Convert spritedata.xml
                #region Replace all sprite IDs with it's corresponding class ID
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                XmlNodeList spriteNodes = doc.SelectNodes("/database/sprites/sprite");

                foreach (XmlNode spriteNode in spriteNodes)
                {
                    XmlAttribute idAttribute = spriteNode.Attributes["id"];
                    int idAttributeValue = int.Parse(idAttribute.Value);
                    idAttribute.Value = classIDforSprite[idAttributeValue].ToString();
                }
                #endregion

                #region Remove all duplicated class ID's and keep first
                List<int> alreadyAdded = new List<int>();
                XmlNode spriteRootNode = doc.SelectSingleNode("/database/sprites");
                foreach (XmlNode spriteNode in spriteNodes)
                {
                    if (alreadyAdded.Contains(int.Parse(spriteNode.Attributes["id"].Value)))
                        spriteRootNode.RemoveChild(spriteRootNode.ChildNodes[alreadyAdded.Count]);
                    else
                        alreadyAdded.Add(int.Parse(spriteNode.Attributes["id"].Value));
                }
                #endregion

                #region Make sure all of the 325 sprite slots are in use
                for (int i = 0; i < 325; i++)
                {
                    //if doesn't exist add
                    if (!alreadyAdded.Contains(i))
                    {
                        XmlElement name = doc.CreateElement("name");
                        name.InnerText = "-";
                        XmlElement flags = doc.CreateElement("flags");
                        flags.SetAttribute("known", 1.ToString());
                        flags.SetAttribute("complete", 1.ToString());
                        XmlElement category = doc.CreateElement("category");
                        category.SetAttribute("id", 1.ToString());

                        XmlElement newSpriteNode = doc.CreateElement("sprite");
                        newSpriteNode.SetAttribute("id", i.ToString());
                        newSpriteNode.AppendChild(name);
                        newSpriteNode.AppendChild(flags);
                        newSpriteNode.AppendChild(category);
                        newSpriteNode.AppendChild(doc.CreateElement("notes"));
                        newSpriteNode.AppendChild(doc.CreateElement("files"));
                        spriteRootNode.AppendChild(newSpriteNode);
                    }
                }
                doc.Save(path);
                #endregion

                #region Order sprite slots so NSMBe doesn't mess everything
                XDocument xdoc = XDocument.Load(path);
                xdoc.Root.Element("sprites").ReplaceNodes(xdoc.Root.Element("sprites").Elements("sprite").OrderBy(i => (int)i.Attribute("id")));
                xdoc.Save(path);
                #endregion
                #endregion

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
            datas2.Clear();
            spriteNames.Clear();
            categoryIds.Clear();
            categories.Clear();
            spritesInCategory.Clear();

            datas = new Dictionary<int, SpriteData>();
            datas2 = new Dictionary<int, SpriteData>();

            if (!File.Exists(path))
            {
                if (MessageBox.Show(LanguageManager.Get("SpriteData", "Prompt"), LanguageManager.Get("SpriteData", "PromptTitle"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    Update();
                else
                    return;
            }
            try
            {
                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                XmlReader xmlr = XmlReader.Create(fs);
                
                xmlr.ReadToFollowing("category");
                do
                {
                    int id = int.Parse(xmlr.GetAttribute("id"));
                    categoryIds.Add(id);
                    categories.Add(xmlr.ReadElementContentAsString());
                    spritesInCategory.Add(id, new List<int>());
                } while (xmlr.ReadToNextSibling("category"));

                while (xmlr.ReadToFollowing("sprite"))
                {
                    SpriteData d = readFromStream(xmlr);

                    //Check for user spritedata
                    FileStream fs_user = new FileStream(Path.Combine(directory, "user_spritedata.xml"), FileMode.Open, FileAccess.Read, FileShare.Read);
                    XmlReader xmlr_user = XmlReader.Create(fs_user);
                    while (xmlr_user.ReadToFollowing("sprite"))
                    {
                        if (int.Parse(xmlr_user.GetAttribute("id")) == d.spriteID)
                        {
                            d = readFromStream(xmlr_user);
                        }
                    }
                    xmlr_user.Close();
                    fs_user.Close();

                    spriteNames.Add(d.name);
                    spritesInCategory[d.categoryID].Add(d.spriteID);

                    if (d != null)
                        datas2.Add(d.spriteID, d);
                }

                for (int i = 0; i < datas2.Count; i++)
                {
                    int ActorID = ROM.GetClassIDFromTable(i);
                    datas.Add(i, datas2[ActorID]);
                }

                datas2.Clear();

                xmlr.Close();
                fs.Close();
            }
            catch (Exception e)
            {
                new ErrorMSGBox(LanguageManager.Get("SpriteData", "ErrorTitle"), LanguageManager.Get("SpriteData", "ErrorParse"), "", e.ToString()).ShowDialog();
                datas.Clear();
                datas2.Clear();
            }
        } 
        
        public static SpriteData readFromStream(XmlReader xmlr)
        {
            SpriteData sd = new SpriteData();

            sd.spriteID = int.Parse(xmlr.GetAttribute("id"));
            //sd.spriteID = (int)ROM.GetClassIDFromTable(sd.spriteID);
            xmlr.ReadToFollowing("name");
            sd.name = xmlr.ReadElementContentAsString();
            xmlr.ReadToFollowing("category");
            sd.categoryID = int.Parse(xmlr.GetAttribute("id"));
            xmlr.ReadToFollowing("notes");
            sd.notes = xmlr.ReadElementContentAsString();

            string field = "field";
            if(sd.spriteID == 77 && Properties.Settings.Default.using_signboard_asm)
            {
                field = "fieldASM";
            }

            while (xmlr.ReadToNextSibling(field))
            {
                SpriteDataField f = new SpriteDataField();
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
                    f.startNibble = f.endNibble = int.Parse(nybbles);
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
                            f.values[j] = Int32.Parse(lulz[0]);
                            f.strings[j] = lulz[1];
                        }
                        break;
                    case "signedvalue":
                        if (values.Trim() == "")
                            f.data = 0;
                        else
                            f.data = Int32.Parse(values);
                        break;
                    case "value":
                        if (values.Trim() == "")
                            f.data = 0;
                        else
                            f.data = Int32.Parse(values);
                        break;
                    case "checkbox":
                        if (values.Trim() == "")
                            f.data = 1;
                        else
                            f.data = Int32.Parse(values);
                        break;
                    case "bitcheckbox":
                        if (values.Trim() == "")
                            f.data = 0;
                        else
                            f.data = Int32.Parse(values);
                        break;
                }
                sd.fields.Add(f);
            }

            return sd;
        }

        public class SpriteDataField
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
                return (endNibble - startNibble + 1) * 4;
                //return (endNibble - startNibble + 1);
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
                byte[] nibbles = new byte[12];
                //byte[] nibbles = new byte[48];
                for (int i = 0; i < 6; i++)
                {
                    nibbles[2 * i] = (byte)(data[i] >> 4);
                    nibbles[2 * i + 1] = (byte)(data[i] & 0xF);
                    /*nibbles[8 * i] = (byte)(data[i] >> 7);
                    nibbles[8 * i + 1] = (byte)((data[i] >> 6) & 0x1);
                    nibbles[8 * i + 2] = (byte)((data[i] >> 5) & 0x1);
                    nibbles[8 * i + 3] = (byte)((data[i] >> 4) & 0x1);
                    nibbles[8 * i + 4] = (byte)((data[i] >> 3) & 0x1);
                    nibbles[8 * i + 5] = (byte)((data[i] >> 2) & 0x1);
                    nibbles[8 * i + 6] = (byte)((data[i] >> 1) & 0x1);
                    nibbles[8 * i + 7] = (byte)(data[i] & 0x1);*/
                }

                int res = 0;
                for (int i = startNibble; i <= endNibble; i++)
                //for (int i = startNibble - 1; i <= endNibble - 1; i++)
                {
                    res = res << 4 | nibbles[i];
                    //res = res << 1 | nibbles[i];
                }

                if (display == "signedvalue" && res > getMax())
                {
                    res -= (1 << getBitCount());
                }

                if (display == "value" || display == "signedvalue")
                    res += this.data;

                if (display == "checkbox")
                    res /= this.data;

                if (display == "bitcheckbox")
                    res = res >> this.data & 1;

                return res;
            }

            public void setValue(int b, byte[] data)
            {
                byte[] nibbles = new byte[12];
                //byte[] nibbles = new byte[48];
                for (int i = 0; i < 6; i++)
                {
                    nibbles[2 * i] = (byte)(data[i] >> 4);
                    nibbles[2 * i + 1] = (byte)(data[i] & 0xF);
                    /*nibbles[8 * i] = (byte)(data[i] >> 7);
                    nibbles[8 * i + 1] = (byte)((data[i] >> 6) & 0x1);
                    nibbles[8 * i + 2] = (byte)((data[i] >> 5) & 0x1);
                    nibbles[8 * i + 3] = (byte)((data[i] >> 4) & 0x1);
                    nibbles[8 * i + 4] = (byte)((data[i] >> 3) & 0x1);
                    nibbles[8 * i + 5] = (byte)((data[i] >> 2) & 0x1);
                    nibbles[8 * i + 6] = (byte)((data[i] >> 1) & 0x1);
                    nibbles[8 * i + 7] = (byte)(data[i] & 0x1);*/
                }

                if (display == "value" || display == "signedvalue")
                    b -= this.data;

                if (display == "checkbox")
                    b *= this.data;

                if (display == "bitcheckbox")
                {
                    int num1 = 0;
                    for (int startNibble = this.startNibble; startNibble <= this.endNibble; ++startNibble)
                        num1 = num1 << 4 | (int)nibbles[startNibble];
                    int num2 = ~(1 << this.data);
                    b = num1 & num2 | b << this.data;
                }

                for (int i = endNibble; i >= startNibble; i--)
                //for (int i = endNibble - 1; i >= startNibble - 1; i--)
                {
                    nibbles[i] = (byte)(b & 0xF);
                    b = b >> 4;
                    /*nibbles[i] = (byte)(b & 0x1);
                    b = b >> 1;*/
                }

                for (int i = 0; i < 6; i++)
                {
                    data[i] = (byte)(nibbles[2 * i] << 4 | nibbles[2 * i + 1]);
                    //data[i] = (byte)(nibbles[8 * i] << 7 | nibbles[8 * i + 1] << 6 | nibbles[8 * i + 2] << 5 | nibbles[8 * i + 3] << 4 | nibbles[8 * i + 4] << 3 | nibbles[8 * i + 5] << 2 | nibbles[8 * i + 6] << 1 | nibbles[8 * i + 7]);
                }
            }
        }

        public class SpriteDataEditor : TableLayoutPanel
        {
            Dictionary<SpriteDataField, Control> controls = new Dictionary<SpriteDataField, Control>();

            List<LevelItem> sprites;
            NSMBSprite s;
            SpriteData sd;
            LevelEditorControl EdControl;
            public bool updating = false;

            public SpriteDataEditor(List<LevelItem> sprites, SpriteData sd, LevelEditorControl EdControl)
            {
                this.SizeChanged += new EventHandler(this_SizeChanged);
                updating = true;
                this.ColumnCount = 3;
                //Talbe layout panel doesn't automatically create row or column styles
                this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
                this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
                this.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
                this.RowCount = sd.fields.Count;
                for (int l = 0; l < this.RowCount; l++)
                    this.RowStyles.Add(new RowStyle(SizeType.Absolute));
                this.AutoSize = true;
                this.AutoSizeMode = AutoSizeMode.GrowAndShrink;

                this.sprites = sprites;
                foreach (LevelItem obj in sprites)
                    if (obj is NSMBSprite) {
                        s = obj as NSMBSprite;
                        break;
                    }
                this.sd = sd;
                this.Dock = DockStyle.Fill;
                this.EdControl = EdControl;

                int row = 0;
                foreach (SpriteDataField v in sd.fields)
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
                    else {
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

            public void UpdateData()
            {
                updating = true;
                foreach (SpriteDataField v in sd.fields)
                    updateValue(v);
                updating = false;
            }

            private Control CreateControlFor(SpriteDataField v)
            {
//                Console.WriteLine(v.display + " " + v.name);
                if (v.display == "checkbox" || v.display == "bitcheckbox")
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

            public void updateValue(SpriteDataField v)
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
                foreach(SpriteDataField sv in controls.Keys)
                {
                    int val = 0;
                    if (controls[sv] is NumericUpDown)
                        val = (int)(controls[sv] as NumericUpDown).Value;
                    else if (controls[sv] is ComboBox) {
                        int se = (controls[sv] as ComboBox).SelectedIndex;
                        if (se == -1)
                            val = 0;
                        else
                            val = sd.fields[index].values[(controls[sv] as ComboBox).SelectedIndex];
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
                    EdControl.UndoManager.Do(new ChangeSpriteDataAction(sprites, d));
                }
            }
        }
    }
}
