using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NSMBe5
{
    public partial class SetBGPropertiesDialog : Form
    {
        public bool Canceled = true;
        public string bgName;
        public int bgNCGID;
        public int bgNCLID;
        public int bgNSCID;
        public int bgBMPOffs;
        public int bgPALOffs;

        public SetBGPropertiesDialog(int ID, string Name, int NCGID, int NCLID, int NSCID, int BMPOffs, int PALOffs, bool IsLevelBG)
        {
            InitializeComponent();

            if(IsLevelBG)
            {
                bmpoffs_UpDown.Enabled = false;
                paloffs_UpDown.Enabled = false;
            }

            name_textBox.Text = Name;
            ncgid_UpDown.Value = NCGID;
            nclid_UpDown.Value = NCLID;
            nscid_UpDown.Value = NSCID;
            bmpoffs_UpDown.Value = BMPOffs;
            paloffs_UpDown.Value = PALOffs;

            ShowDialog();
        }

        private void Cancel_btn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Apply_btn_Click(object sender, EventArgs e)
        {
            Canceled = false;

            bgName = name_textBox.Text;
            bgNCGID = (int)ncgid_UpDown.Value;
            bgNCLID = (int)nclid_UpDown.Value;
            bgNSCID = (int)nscid_UpDown.Value;
            bgBMPOffs = (int)bmpoffs_UpDown.Value;
            bgPALOffs = (int)paloffs_UpDown.Value;

            Close();
        }
    }
}
