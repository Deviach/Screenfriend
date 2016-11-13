using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScreenFriend
{
    public partial class LargePicture : Form
    {
        public LargePicture(Image img)
        {
            InitializeComponent();
            pictureBox1.Image = img;
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            this.Size = img.Size;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.W))
            {
                this.Close();
                return true;
            }
            else if (keyData == (Keys.Control | Keys.X))
            {
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

    }
}
