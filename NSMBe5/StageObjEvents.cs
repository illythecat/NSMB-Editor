using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Be.Timvw.Framework.ComponentModel;

namespace NSMBe5
{
    public partial class StageObjEventsViewer : Form
    {
        LevelEditorControl ed;
        SortableBindingList<SpriteDataRow> spriteList = new SortableBindingList<SpriteDataRow>();
        bool refreshing = false;

        public StageObjEventsViewer(LevelEditorControl ed)
        {
            InitializeComponent();
            this.ed = ed;
            spriteTable.DataSource = spriteList;

            LanguageManager.ApplyToContainer(this, "SpriteEvents");
            this.EventID.HeaderText = LanguageManager.Get("SpriteEvents", "EventID");
            this.SpriteNum.HeaderText = LanguageManager.Get("SpriteEvents", "SpriteNumber");
            this.SpriteName.HeaderText = LanguageManager.Get("SpriteEvents", "SpriteName");
        }

        public void ReloadSprites(object sender, EventArgs e)
        {
            refreshing = true;
            spriteList.Clear();
            foreach (NSMBStageObj s in ed.Level.Sprites) {
                if (s.Data[0] != 0)
                    spriteList.Add(new SpriteDataRow(s, s.Data[0]));
                if (s.Data[1] != 0)
                    spriteList.Add(new SpriteDataRow(s, s.Data[1]));
            }
            spriteTable.ClearSelection();
            refreshing = false;
        }

        private void SpriteEvents_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void spriteTable_SelectionChanged(object sender, EventArgs e)
        {
            if (refreshing) return;
            List<LevelItem> sprites = new List<LevelItem>();
            NSMBStageObj s;
            foreach (DataGridViewRow row in spriteTable.SelectedRows)
            {
                s = spriteList[row.Index].sprite;
                if (ed.Level.Sprites.Contains(s) && !sprites.Contains(s))
                    sprites.Add(s);
            }
            ed.SelectObject(sprites);
            ed.ScrollToObjects(sprites);
            ed.repaint();
            this.BringToFront();
        }

        private class SpriteDataRow
        {
            public NSMBStageObj sprite;
            public int _eventID;

            public SpriteDataRow(NSMBStageObj sprite, int eventID)
            {
                this.sprite = sprite;
                this._eventID = eventID;
            }

            public int eventID {
                get {
                    return _eventID;
                }
            }
            public int spriteType {
                get {
                    return sprite.Type;
                }
            }
            public string spriteName {
                get {
					int objectID = ROM.GetObjIDFromTable(sprite.Type);
					return StageObjSettings.GetObjectName(objectID);
				}
            }
        }
    }
}
