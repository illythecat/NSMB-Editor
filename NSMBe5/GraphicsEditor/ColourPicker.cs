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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NSMBe5 {
    public partial class ColourPicker : Form {
        public int R { get { return colourPickerControl1.R; } set { colourPickerControl1.R = value; r_UpDown.Value = value; } }
        public int G { get { return colourPickerControl1.G; } set { colourPickerControl1.G = value; g_UpDown.Value = value; } }
        public int B { get { return colourPickerControl1.B; } set { colourPickerControl1.B = value; b_UpDown.Value = value; } }
        public int Value { get { return colourPickerControl1.Value; } set { colourPickerControl1.Value = value; } }

        public ColourPicker() {
            InitializeComponent();
            LanguageManager.ApplyToContainer(this, "ColourPicker");
        }

        private void RBG_UpDown_ValueChanged(object sender, EventArgs e)
        {
            colourPickerControl1.R = (int)r_UpDown.Value;
            colourPickerControl1.G = (int)g_UpDown.Value;
            colourPickerControl1.B = (int)b_UpDown.Value;
            colourPickerControl1.renderer.Invalidate();
        }

        private void ColourPickerControl_ValueChanged(object sender, EventArgs e)
        {
            r_UpDown.Value = colourPickerControl1.R;
            g_UpDown.Value = colourPickerControl1.G;
            b_UpDown.Value = colourPickerControl1.B;
        }
    }
}
