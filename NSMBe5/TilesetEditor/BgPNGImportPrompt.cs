using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NSMBe5.TilemapEditor
{
    public partial class BgPNGImportPrompt : Form
    {
        public int fgHeightInPixels;
        public int bgHeightInPixels;
        public int fgCharacterCount;
        public int bgCharacterCount;
        public int bgFirstTileOffset;
        public bool finished = false;

        public BgPNGImportPrompt(bool topLayer, bool mappedTileset)
        {
            InitializeComponent();

            if (topLayer)
                label1.Text = "Before importing as PNG, please input a custom tile count if you want to make use of the foreground overwrite bug that allows for higher quality foregrounds, while sacrificing background tiles, otherwise just proceed.";
            else
                label1.Text = "Before importing as PNG, please input the same settings you used when importing the corresponding foreground.";

            UpdateVariables();

            if (mappedTileset)
            {
                finished = true;
                return;
            }

            ShowDialog();
        }

        private void UpdateVariables()
        {
            fgCharacterCount = (int)numericUpDown1.Value * 2;
            bgCharacterCount = 640 - fgCharacterCount;
            fgHeightInPixels = (fgCharacterCount * 64) / 256;
            bgHeightInPixels = (bgCharacterCount * 64) / 256;
            bgFirstTileOffset = fgHeightInPixels - 80;
            fgBmpSize_label.Text = "Foreground Bitmap Size: 256x" + fgHeightInPixels;
            bgBmpSize_label.Text = "Background Bitmap Size: 256x" + bgHeightInPixels;
        }

        private void Proceed_btn_Click(object sender, EventArgs e)
        {
            finished = true;
            Close();
        }

        private void NumericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if ((sender as NumericUpDown).Value % 2 != 0)
                numericUpDown1.Value++;

            UpdateVariables();
        }
    }
}
