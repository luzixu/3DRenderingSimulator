using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PerfectWorldSurvivor.Draw;
using PerfectWorldSurvivor.UI;


namespace PerfectWorldSurvivor
{
    partial class MainWin
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public static int fps;

        //this.pictureBox1.Size = new System.Drawing.Size(909, 443);

        public static MainWin main;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.LightsOn = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.fpsLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.showTrianglesLabel = new System.Windows.Forms.Label();
            this.pictureBox1 = new PerfectWorldSurvivor.Scene();
            this.label5 = new System.Windows.Forms.Label();
            this.backfaceTrianglesLabel = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cullingObjectsLabel = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBox2
            // 
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "盒子",
            "纹理平板",
            "线框",
            "纹理模型"});
            this.comboBox2.Location = new System.Drawing.Point(330, 22);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(121, 20);
            this.comboBox2.TabIndex = 4;
            this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // LightsOn
            // 
            this.LightsOn.AutoSize = true;
            this.LightsOn.Location = new System.Drawing.Point(31, 26);
            this.LightsOn.Name = "LightsOn";
            this.LightsOn.Size = new System.Drawing.Size(36, 16);
            this.LightsOn.TabIndex = 5;
            this.LightsOn.Text = "光";
            this.LightsOn.UseVisualStyleBackColor = true;
            this.LightsOn.CheckedChanged += new System.EventHandler(this.LightsOn_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(748, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "fps";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.fpsLabel.AutoSize = true;
            this.fpsLabel.Location = new System.Drawing.Point(866, 65);
            this.fpsLabel.Name = "label2";
            this.fpsLabel.Size = new System.Drawing.Size(17, 12);
            this.fpsLabel.TabIndex = 7;
            this.fpsLabel.Text = "60";
            this.fpsLabel.Click += new System.EventHandler(this.label2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(732, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "render triangles";
            // 
            // label4
            // 
            this.showTrianglesLabel.AutoSize = true;
            this.showTrianglesLabel.Location = new System.Drawing.Point(866, 100);
            this.showTrianglesLabel.Name = "label4";
            this.showTrianglesLabel.Size = new System.Drawing.Size(11, 12);
            this.showTrianglesLabel.TabIndex = 9;
            this.showTrianglesLabel.Text = "0";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 65);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(714, 500);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(732, 138);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "backface";
            // 
            // label6
            // 
            this.backfaceTrianglesLabel.AutoSize = true;
            this.backfaceTrianglesLabel.Location = new System.Drawing.Point(866, 138);
            this.backfaceTrianglesLabel.Name = "label6";
            this.backfaceTrianglesLabel.Size = new System.Drawing.Size(11, 12);
            this.backfaceTrianglesLabel.TabIndex = 11;
            this.backfaceTrianglesLabel.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(732, 175);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(83, 12);
            this.label7.TabIndex = 12;
            this.label7.Text = "cullObjectNum";
            // 
            // label8
            // 
            this.cullingObjectsLabel.AutoSize = true;
            this.cullingObjectsLabel.Location = new System.Drawing.Point(866, 175);
            this.cullingObjectsLabel.Name = "label8";
            this.cullingObjectsLabel.Size = new System.Drawing.Size(11, 12);
            this.cullingObjectsLabel.TabIndex = 13;
            this.cullingObjectsLabel.Text = "0";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(89, 26);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(72, 16);
            this.checkBox1.TabIndex = 14;
            this.checkBox1.Text = "线框模式";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // MainWin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 589);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.cullingObjectsLabel);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.backfaceTrianglesLabel);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.showTrianglesLabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.fpsLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LightsOn);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.pictureBox1);
            this.Name = "MainWin";
            this.Text = "Display";
            this.Load += new System.EventHandler(this._Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private Scene pictureBox1;
        private ComboBox comboBox2;
        private CheckBox LightsOn;
        private Label label1;
        private Label fpsLabel;
        private Label label3;
        private Label showTrianglesLabel;
        private Label label5;
        private Label backfaceTrianglesLabel;
        private Label label7;
        private Label cullingObjectsLabel;
        private CheckBox checkBox1;


    }

    public class Scene : System.Windows.Forms.PictureBox
    {
        public void ExternLoad()
        {
            this.DoubleBuffered = true;
            int rectWidth = this.ClientRectangle.Width;
            int rectHeight = this.ClientRectangle.Height;
            _displayRect = new Rectangle(0, 0, rectWidth, rectHeight);
            _bitmap = new Bitmap(rectWidth, rectHeight);
            _stage = new RenderEngine(_bitmap);
            if (MainWin.enableTicks)
            {
                _InitTick();
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (_stage != null)
            {
                _stage.Dispose();
            }
        }

        private void _InitTick()
        {
            System.Timers.Timer refreshDrawTimer = new System.Timers.Timer();
            refreshDrawTimer.Elapsed += new System.Timers.ElapsedEventHandler(_Tick);
            refreshDrawTimer.AutoReset = true;
            refreshDrawTimer.Enabled = true;
            refreshDrawTimer.Start();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            if (!MainWin.enableTicks)
            {
                _graphics = pe.Graphics;
                _stage.Render();
                _graphics.DrawImage(_bitmap, _displayRect);
            }
        }

        private void _Tick(object sender, EventArgs e)
        {
            long start = System.DateTime.Now.Ticks;
            lock (_stage)
            {
                if (_graphics == null)
                {
                    _graphics = this.CreateGraphics();
                }
                _stage.Render();
                _graphics.DrawImage(_bitmap, _displayRect);
            }

            //Calculate FPS
            _framesCount++;
            long frameTimeSpan = (System.DateTime.Now.Ticks - start);
            _frameTimer += frameTimeSpan;
            if (_frameTimer >= _transformTicksToSecond)
            {
                double fps = _transformTicksToSecond * _framesCount / frameTimeSpan;
                MainWin.fps = (int)fps;
                _frameTimer = 0;
                _framesCount = 0;
            }
        }

        private Bitmap _bitmap;

        private Rectangle _displayRect;

        private RenderEngine _stage;

        private Graphics _graphics;

        private float _framesCount = 0;

        private long _frameTimer = 0;

        private const float _transformTicksToSecond = 1000000f;

    }
}

