using System.Drawing;
using System.Windows.Forms;

namespace _4932Assignment1
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menu_strip = new System.Windows.Forms.MenuStrip();
            this.morphingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.beginToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.framesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.frames5 = new System.Windows.Forms.ToolStripMenuItem();
            this.frames10 = new System.Windows.Forms.ToolStripMenuItem();
            this.threadsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.t1 = new System.Windows.Forms.ToolStripMenuItem();
            this.t2 = new System.Windows.Forms.ToolStripMenuItem();
            this.t3 = new System.Windows.Forms.ToolStripMenuItem();
            this.t4 = new System.Windows.Forms.ToolStripMenuItem();
            this.t8 = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_strip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menu_strip
            // 
            this.menu_strip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.morphingToolStripMenuItem,
            this.framesToolStripMenuItem,
            this.threadsToolStripMenuItem});
            this.menu_strip.Location = new System.Drawing.Point(0, 0);
            this.menu_strip.Name = "menu_strip";
            this.menu_strip.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.menu_strip.Size = new System.Drawing.Size(925, 24);
            this.menu_strip.TabIndex = 1;
            this.menu_strip.Text = "menuStrip1";
            // 
            // morphingToolStripMenuItem
            // 
            this.morphingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.beginToolStripMenuItem});
            this.morphingToolStripMenuItem.Name = "morphingToolStripMenuItem";
            this.morphingToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
            this.morphingToolStripMenuItem.Text = "Morph";
            // 
            // beginToolStripMenuItem
            // 
            this.beginToolStripMenuItem.Name = "beginToolStripMenuItem";
            this.beginToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.beginToolStripMenuItem.Text = "Start";
            this.beginToolStripMenuItem.Click += new System.EventHandler(this.beginToolStripMenuItem_Click1);
            // 
            // framesToolStripMenuItem
            // 
            this.framesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.frames5,
            this.frames10});
            this.framesToolStripMenuItem.Name = "framesToolStripMenuItem";
            this.framesToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.framesToolStripMenuItem.Text = "Frames";
            // 
            // frames5
            // 
            this.frames5.Name = "frames5";
            this.frames5.Size = new System.Drawing.Size(86, 22);
            this.frames5.Text = "5";
            // 
            // frames10
            // 
            this.frames10.Name = "frames10";
            this.frames10.Size = new System.Drawing.Size(86, 22);
            this.frames10.Text = "10";
            // 
            // threadsToolStripMenuItem
            // 
            this.threadsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.t1,
            this.t2,
            this.t3,
            this.t4,
            this.t8});
            this.threadsToolStripMenuItem.Name = "threadsToolStripMenuItem";
            this.threadsToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.threadsToolStripMenuItem.Text = "Threads";
            // 
            // t1
            // 
            this.t1.Name = "t1";
            this.t1.Size = new System.Drawing.Size(80, 22);
            this.t1.Text = "1";
            // 
            // t2
            // 
            this.t2.Name = "t2";
            this.t2.Size = new System.Drawing.Size(80, 22);
            this.t2.Text = "2";
            // 
            // t3
            // 
            this.t3.Name = "t3";
            this.t3.Size = new System.Drawing.Size(80, 22);
            this.t3.Text = "3";
            // 
            // t4
            // 
            this.t4.Name = "t4";
            this.t4.Size = new System.Drawing.Size(80, 22);
            this.t4.Text = "4";
            // 
            // t8
            // 
            this.t8.Name = "t8";
            this.t8.Size = new System.Drawing.Size(80, 22);
            this.t8.Text = "8";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(925, 549);
            this.Controls.Add(this.menu_strip);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menu_strip;
            this.Name = "Form1";
            this.Text = "4932 Assignment 1";
            this.menu_strip.ResumeLayout(false);
            this.menu_strip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MenuStrip menu_strip;
        private ToolStripMenuItem morphingToolStripMenuItem;
        private ToolStripMenuItem beginToolStripMenuItem;
        private ToolStripMenuItem framesToolStripMenuItem;
        private ToolStripMenuItem frames5;
        private ToolStripMenuItem frames10;
        private ToolStripMenuItem threadsToolStripMenuItem;
        private ToolStripMenuItem t1;
        private ToolStripMenuItem t2;
        private ToolStripMenuItem t3;
        private ToolStripMenuItem t4;
        private ToolStripMenuItem t8;
    }
}

