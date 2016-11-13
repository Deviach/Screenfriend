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
    public partial class Form1 : Form
    {
        Dictionary<int,Image> images;
        int lastx = 0, lasty = 0;
        int boxW = 150;
        int boxH = 200;
        int bufferSpace = 5;
        public Form1()
        {
            InitializeComponent();
            //lastx = panel1.Location.X;
            //lasty = panel1.Location.Y;
            
            images = new Dictionary<int, Image>();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsImage())
            {
                images.Add(Clipboard.GetImage().GetHashCode(), Clipboard.GetImage());
                PictureBox p = new PictureBox();
                p.Image = Clipboard.GetImage();
                p.Visible = true;
                p.SizeMode = PictureBoxSizeMode.StretchImage;
                p.SetBounds(lastx, lasty, boxW, boxH);
                lastx = (lastx + boxW+bufferSpace);
                if ((lastx + boxW + bufferSpace) > panel1.Width)
                {
                    lastx = 0;
                    lasty += boxH + bufferSpace;
                }
                panel1.Controls.Add(p);
            }
            else
            {
                MessageBox.Show("No picture on clipboard");
            }
            
            
        }
    }
}
