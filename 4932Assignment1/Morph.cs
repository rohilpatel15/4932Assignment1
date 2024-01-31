using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;

namespace _4932Assignment1
{
    public partial class Morph : Form
    {
        private Bitmap formMapMorph;
        public Morph()
        {
            InitializeComponent();
            formMapMorph = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (formMapMorph != null)
            {
                e.Graphics.DrawImage(formMapMorph, 0, 0, ClientSize.Width, ClientSize.Height);
            }

        }

        private void savToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {

        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void Morph_Load(object sender, EventArgs e)
        {

        }
        public void SetImage(Bitmap image, Bitmap loaded)
        {
            formMapMorph = image;
            Refresh();
        }
    }
}