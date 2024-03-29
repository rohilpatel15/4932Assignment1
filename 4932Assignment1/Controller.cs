﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace _4932Assignment1
{
    public partial class Controller : Form
    {
        private FormBuilder final;
        private int count = 0;
        private int count2 = 0;
        private List<Bitmap> frames;

        public Controller(List<Bitmap> frames, FormBuilder final)
        {
            InitializeComponent();
            this.final = final;
            this.frames = frames;
            Location = new Point(ClientSize.Width - this.Width, ClientSize.Height - this.Height);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (count2 != frames.Count - 1)
            {
                count2++;
                final.SetImage(frames[count2]);
            }
        }

        private void Cycle()
        {
            if (count + 1 == ((Form1)MdiParent).GetFrames().Count)
            {
                count = 0;
                timer1.Stop();
            }
            else
            {
                count++;
                final.SetImage(((Form1)MdiParent).GetFrames()[count]);
            }
        }

        private Timer timer1;
        public void InitTimer()
        {
            timer1 = new Timer();
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 100; // in miliseconds
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Cycle();
        }
        private void Controller_Load(object sender, EventArgs e)
        {
            label1.Text = "Ready to morph";
        }
        private void Previous_Click(object sender, EventArgs e)
        {
            if (count2 != 0)
            {
                count2--;
                final.SetImage(frames[count2]);
            }
            else return;
        }

        private void Play_Click(object sender, EventArgs e)
        {
            if (((Form1)MdiParent).GetFrames().Count == 0)
            {
                return;
            }
            else InitTimer();
        }
    }
}
