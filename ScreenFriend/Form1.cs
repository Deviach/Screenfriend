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
        List<PictureBox> pics;
        int lastx = 0, lasty = 0;
        int boxW = 150;
        int boxH = 200;
        int bufferSpace = 5;
        Image imgToSave = null;
        public Form1()
        {
            InitializeComponent();
            //lastx = panel1.Location.X;
            //lasty = panel1.Location.Y;
            pics = new List<PictureBox>();
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
                pics.Add(p);
                p.Click += pictureBoxClick;
                
            }
            else
            {
                MessageBox.Show("No picture on clipboard");
            }
            
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //TODO: just keep track of the selected pb
            foreach (PictureBox pb in pics)
            {
                pb.BorderStyle = BorderStyle.None;
            }
            button2.Enabled = false;
            imgToSave = null;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //deselect
            button3_Click(null, null);

            foreach (PictureBox pb in pics)
            {
                pb.Dispose();
            }
            lastx = 0;
            lasty = 0;
            panel1.Refresh();
        }

        private void pictureBoxClick(object sender, EventArgs e)
        {
            foreach (PictureBox pb in pics)
            {
                pb.BorderStyle = BorderStyle.None;
            }
            PictureBox p = (PictureBox)sender;
            p.BorderStyle = BorderStyle.FixedSingle;
            button2.Enabled = true;
            imgToSave = p.Image;
        }
    }
}
