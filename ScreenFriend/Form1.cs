using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

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
            textBox1.Text = Directory.GetCurrentDirectory();
            images = new Dictionary<int, Image>();
            this.FormClosing += Form1_FormClosing;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        public void addClipboard()
        {
            button1_Click(null, null);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel1.VerticalScroll.Value = 0;
            if (Clipboard.ContainsImage())
            {
                images.Add(Clipboard.GetImage().GetHashCode(), Clipboard.GetImage());
                PictureBox p = new PictureBox();
                p.Image = Clipboard.GetImage();
                p.Visible = true;
                p.SizeMode = PictureBoxSizeMode.StretchImage;
                Point pt = new Point(lastx, lasty);
                //pt.Offset(panel1.AutoScrollOffset);
                p.SetBounds(pt.X,pt.Y,boxW, boxH);
                lastx = (lastx + boxW+bufferSpace);
                if ((lastx + boxW + bufferSpace) > panel1.Width)
                {
                    Point npt = new Point(0, (lasty+boxH + bufferSpace));
                    
                    lastx = 0;

                    lasty = npt.Y;
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
            images.Clear();
            pics.Clear();
            lastx = 0;
            lasty = 0;
            panel1.Refresh();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = Directory.GetCurrentDirectory();
            fbd.ShowDialog();
            textBox1.Text = fbd.SelectedPath;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string path = textBox1.Text + @"\" + DateTime.Now.ToString("YYYY-MM-dd-HH-mm-ss")+".png";
            FileStream fs = new FileStream(path, FileMode.CreateNew);
            imgToSave.Save(fs, ImageFormat.Png);
            fs.Close();
            label1.Visible = true;
            label1.ForeColor = Color.Green;
            label1.Text = "File saved at " + DateTime.Now.ToString("HH:mm:ss");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        

      

        private void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            MessageBox.Show("LELELE");
        }

        private void pictureBoxClick(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            PictureBox p = (PictureBox)sender;
            if (me.Button == MouseButtons.Left) { 
                foreach (PictureBox pb in pics)
                {
                    pb.BorderStyle = BorderStyle.None;
                }
                
                p.BorderStyle = BorderStyle.FixedSingle;
                button2.Enabled = true;
                imgToSave = p.Image;
            }
            else
            {
                LargePicture lp = new LargePicture(p.Image);
                lp.Show();
                
            }
        }
    }
}
