using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _4932Assignment1
{
    public partial class Form1 : Form
    {
        /* Create each form object */
        Destination destination = new Destination();
        Morph morph = new Morph();
        Source source = new Source();
        public Form1()
        {
            InitializeComponent();
            /* Makes all other forms child and stay within original form */
            destination.MdiParent = this;
            morph.MdiParent = this;
            source.MdiParent = this;
            destination.Show();
            morph.Show();
            source.Show();

        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        /* This method duplicate lines created from one form to another*/
        public void DuplicateLine(Line line, int form)
        {
            Line line_2 = new Line(line.StartPoint, line.EndPoint);
            if (form == 1)
            {
                destination.ReplicateLine(line_2);
            } else if (form == 2)
            {
                source.ReplicateLine(line_2);
            }
        }
    }
}