using CrazyRobot.model;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace CrazyRobotView
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.FileMenu = new System.Windows.Forms.MenuStrip();
            this.File = new System.Windows.Forms.ToolStripMenuItem();
            this.NewGame = new System.Windows.Forms.ToolStripMenuItem();
            this.Load = new System.Windows.Forms.ToolStripMenuItem();
            this.Save = new System.Windows.Forms.ToolStripMenuItem();
            this.Difficulty = new System.Windows.Forms.ToolStripMenuItem();
            this.Small = new System.Windows.Forms.ToolStripMenuItem();
            this.Medium = new System.Windows.Forms.ToolStripMenuItem();
            this.Big = new System.Windows.Forms.ToolStripMenuItem();
            this.Pause = new System.Windows.Forms.ToolStripMenuItem();
            this._openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this._saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelWalls = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.FileMenu.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // FileMenu
            // 
            this.FileMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.File,
            this.Difficulty,
            this.Pause});
            this.FileMenu.Location = new System.Drawing.Point(0, 0);
            this.FileMenu.Name = "FileMenu";
            this.FileMenu.Size = new System.Drawing.Size(434, 24);
            this.FileMenu.TabIndex = 0;
            this.FileMenu.Text = "FileMenu";
            // 
            // File
            // 
            this.File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NewGame,
            this.Load,
            this.Save});
            this.File.Name = "File";
            this.File.Size = new System.Drawing.Size(37, 20);
            this.File.Text = "File";
            // 
            // NewGame
            // 
            this.NewGame.Name = "NewGame";
            this.NewGame.Size = new System.Drawing.Size(132, 22);
            this.NewGame.Text = "New Game";
            this.NewGame.Click += new System.EventHandler(this.NewGame_Click);
            // 
            // Load
            // 
            this.Load.Name = "Load";
            this.Load.Size = new System.Drawing.Size(132, 22);
            this.Load.Text = "Load";
            this.Load.Click += new System.EventHandler(this.Load_Click);
            // 
            // Save
            // 
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(132, 22);
            this.Save.Text = "Save";
            this.Save.Click += new System.EventHandler(this.Save_Click);
            // 
            // Difficulty
            // 
            this.Difficulty.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Small,
            this.Medium,
            this.Big});
            this.Difficulty.Name = "Difficulty";
            this.Difficulty.Size = new System.Drawing.Size(50, 20);
            this.Difficulty.Text = "Game";
            // 
            // Small
            // 
            this.Small.CheckOnClick = true;
            this.Small.Name = "Small";
            this.Small.Size = new System.Drawing.Size(119, 22);
            this.Small.Text = "Small";
            this.Small.Click += new System.EventHandler(this.Small_Click);
            // 
            // Medium
            // 
            this.Medium.CheckOnClick = true;
            this.Medium.Name = "Medium";
            this.Medium.Size = new System.Drawing.Size(119, 22);
            this.Medium.Text = "Medium";
            this.Medium.Click += new System.EventHandler(this.Medium_Click);
            // 
            // Big
            // 
            this.Big.CheckOnClick = true;
            this.Big.Name = "Big";
            this.Big.Size = new System.Drawing.Size(119, 22);
            this.Big.Text = "Big";
            this.Big.Click += new System.EventHandler(this.Big_Click);
            // 
            // Pause
            // 
            this.Pause.Checked = true;
            this.Pause.CheckOnClick = true;
            this.Pause.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this.Pause.Name = "Pause";
            this.Pause.Size = new System.Drawing.Size(50, 20);
            this.Pause.Text = "Pause";
            this.Pause.Click += new System.EventHandler(this.Pause_Click);
            // 
            // _openFileDialog
            // 
            this._openFileDialog.Filter = "Crazy robot tábla (*.txt)|*.txt";
            this._openFileDialog.Title = "Crazy robot játék betöltése";
            // 
            // _saveFileDialog
            // 
            this._saveFileDialog.Filter = "Crazy robot tábla (*.txt)|*.txt";
            this._saveFileDialog.Title = "Crazy robot játék mentése";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelWalls,
            this.toolStripStatusLabelTime});
            this.statusStrip1.Location = new System.Drawing.Point(0, 389);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(434, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabelWalls
            // 
            this.toolStripStatusLabelWalls.Name = "toolStripStatusLabelWalls";
            this.toolStripStatusLabelWalls.Size = new System.Drawing.Size(85, 17);
            this.toolStripStatusLabelWalls.Text = "Placed Walls: 0";
            // 
            // toolStripStatusLabelTime
            // 
            this.toolStripStatusLabelTime.Name = "toolStripStatusLabelTime";
            this.toolStripStatusLabelTime.Size = new System.Drawing.Size(116, 17);
            this.toolStripStatusLabelTime.Text = "Elapsed time: 0:00:00";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 411);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.FileMenu);
            this.MainMenuStrip = this.FileMenu;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(450, 450);
            this.Name = "Form1";
            this.Text = "Crazy Robot";
            this.FileMenu.ResumeLayout(false);
            this.FileMenu.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private MenuStrip FileMenu;
        private ToolStripMenuItem File;
        private ToolStripMenuItem Load;
        private ToolStripMenuItem Save;
        private ToolStripMenuItem Difficulty;
        private ToolStripMenuItem Small;
        private ToolStripMenuItem Medium;
        private ToolStripMenuItem Big;
        private OpenFileDialog _openFileDialog;
        private SaveFileDialog _saveFileDialog;
        private ToolStripMenuItem NewGame;
        private ToolStripMenuItem Pause;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabelTime;
        private ToolStripStatusLabel toolStripStatusLabelWalls;
    }
    #endregion
}