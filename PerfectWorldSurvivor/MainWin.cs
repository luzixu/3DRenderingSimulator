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
using System.Threading;

namespace PerfectWorldSurvivor
{
    public partial class MainWin : Form
    {

        public static bool enableTicks = true;

        public static bool lineMode = false;

        public static int triangles = 0;

        public static int cullingObjects = 0;

        public static int backfaceCulling = 0;

        public MainWin()
        {
            InitializeComponent();
        }
        private void _Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;
            this.KeyPreview = true;
            this.KeyPress += new KeyPressEventHandler(_HandleKeyPress);
            this.comboBox2.SelectedItem = this.comboBox2.Items[0];
            Scene scene = (Scene)pictureBox1;
            DisplayEngine.displayType = DisplayEngine.displayTypeRasterizedTriangle;

            main = this;
            scene.ExternLoad();
            this.pictureBox1.Refresh();
            if (enableTicks)
            {
                _InitTick();
            }
        }

        private void _Tick(object sender, EventArgs e)
        {
            fpsLabel.BeginInvoke(new MethodInvoker(() => fpsLabel.Text = fps.ToString()));
            showTrianglesLabel.BeginInvoke(new MethodInvoker(() => showTrianglesLabel.Text = triangles.ToString()));
            backfaceTrianglesLabel.BeginInvoke(new MethodInvoker(() => backfaceTrianglesLabel.Text = backfaceCulling.ToString()));
            cullingObjectsLabel.BeginInvoke(new MethodInvoker(() => cullingObjectsLabel.Text = cullingObjects.ToString()));
        }

        private void _InitTick()
        {
            System.Timers.Timer refreshDrawTimer = new System.Timers.Timer(1000);
            refreshDrawTimer.Elapsed += new System.Timers.ElapsedEventHandler(_Tick);
            refreshDrawTimer.AutoReset = true;
            refreshDrawTimer.Enabled = true;
            refreshDrawTimer.Start();
        }

        private void _HandleKeyPress(object sender, KeyPressEventArgs e)
        {
            char keyChar = e.KeyChar;
            _HandleKey(keyChar);
        }
        private void _HandleKey(char keyChar)
        {
            KeyMessageController keyMessageController = KeyMessageController.GetInstance();
            keyMessageController.AddKeyEvent(keyChar);
            keyMessageController.HandleKeyEvent();
            if (!enableTicks)
            {
                this.pictureBox1.Refresh();
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox.ObjectCollection items = comboBox2.Items;
            if (this.comboBox2.SelectedItem == items[0])
            {
                if (DisplayEngine.displayType != DisplayEngine.displayTypeRasterizedTriangle)
                {
                    DisplayEngine.displayType = DisplayEngine.displayTypeRasterizedTriangle;
                    _HandleKey(InputKeys.box);
                }
            }
            else if (this.comboBox2.SelectedItem == items[1])
            {
                if (DisplayEngine.displayType != DisplayEngine.displayTypeRasterizedTriangleWithLight)
                {
                    DisplayEngine.displayType = DisplayEngine.displayTypeRasterizedTriangleWithLight;
                    _HandleKey(InputKeys.planeTexture);
                }
            }
            else if (this.comboBox2.SelectedItem == items[2])
            {
                if (DisplayEngine.displayType != DisplayEngine.displayTypeSimpleTriangle)
                {
                    DisplayEngine.displayType = DisplayEngine.displayTypeSimpleTriangle;
                    _HandleKey(InputKeys.justLine);
                }
            }
            else if (this.comboBox2.SelectedItem == items[3])
            {
                if (DisplayEngine.displayType != DisplayEngine.displayTypeModeChicken)
                {
                    DisplayEngine.displayType = DisplayEngine.displayTypeModeChicken;
                    _HandleKey(InputKeys.chicken);
                }
            }
        }

        private void LightsOn_CheckedChanged(object sender, EventArgs e)
        {
            _HandleKey(InputKeys.reverseLightsOnState);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            lineMode = !lineMode;
            _HandleKey(InputKeys.lineMode);
        }
    }
}
