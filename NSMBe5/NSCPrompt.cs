using System;
using System.Windows.Forms;

namespace NSMBe5
{
    public partial class NSCPrompt : Form
    {
        public int tileWidth = 32;
        public int imgOffs = 0;
        public int palOffs = 0;
        int fileSize = 0;
        public bool canceled = true;

        public NSCPrompt(int fileSize)
        {
            InitializeComponent();
            this.fileSize = fileSize;

            int totalPixels = fileSize * 32;
            int width = tileWidth * 8;
            int height = totalPixels / width;
            res_label.Text = "NSC Dimensions: " + width + "x" + height + " (" + totalPixels + ")";

            ShowDialog();
        }

        private void Defaultpref_btn_Click(object sender, EventArgs e)
        {
            tilewidth_UpDown.Value = 32;
            bmpoff_UpDown.Value = 0;
            paloff_UpDown.Value = 0;
        }

        private void Tilesetpref_btn_Click(object sender, EventArgs e)
        {
            tilewidth_UpDown.Value = 64;
            bmpoff_UpDown.Value = 192;
            paloff_UpDown.Value = 2;
        }

        private void Bgpref_btn_Click(object sender, EventArgs e)
        {
            tilewidth_UpDown.Value = 64;
            bmpoff_UpDown.Value = 576;
            paloff_UpDown.Value = 10;
        }

        private void Fgpref_btn_Click(object sender, EventArgs e)
        {
            tilewidth_UpDown.Value = 64;
            bmpoff_UpDown.Value = 256;
            paloff_UpDown.Value = 8;
        }

        private void Proceed_btn_Click(object sender, EventArgs e)
        {
            canceled = false;
            Close();
        }

        private void UpDown_ValueChanged(object sender, EventArgs e)
        {
            tileWidth = (int)tilewidth_UpDown.Value;
            imgOffs = (int)bmpoff_UpDown.Value;
            palOffs = (int)paloff_UpDown.Value;

            int totalPixels = fileSize * 32;
            int width = tileWidth * 8;
            int height = totalPixels / width;

            res_label.Text = "NSC Dimensions: " + width + "x" + height + " (" + totalPixels + ")";
        }
    }
}
