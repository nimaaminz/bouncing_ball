using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace BouncingBall
{
    public partial class Form1 : Form
    {
        Stopwatch sp = new Stopwatch();
        int mouse_last_y;

        #region variable declared here . . . . 
        //Color . . . .
        Color BackPanel = Color.FromArgb(229, 229, 229);
        static Color BallBrush = Color.FromArgb(16, 137, 255);
        Color BallBorder = Color.FromArgb(35, 55, 77);
        SolidBrush brush = new SolidBrush(BallBrush);
        // location on page . . .
        float PositionX;
        float PositionY = 0;
        float last_pos_y;

        // ball details . . . 
        int ball_width = 35;
        int ball_height = 35;

        // Physics variables . . .
        const float a = 9.8f;
        float z = 150;
        float v = 0;
        float mass = 1f;
        float n = 0.8f;
        float K;      // kinetic energy k = 1/2 * m * v^2 ;

        // other . . . 
        bool check = false;
        bool check_event_call = true;

        #endregion

        public Form1()
        {
            InitializeComponent();
            PositionX = panel1.Width / 2;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            set_ball_height(100);
        }

        private void set_ball_height(float input_z)
        {
            z = input_z;
            PositionY = panel1.Height - ((int)z) - ball_height;
            //load_ball(PositionY); 
            if (last_pos_y != PositionY)
            {
                panel1.Refresh();
                last_pos_y = PositionY;
            }
            label3.Text = PositionY.ToString();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!(z == 0 && K < 1) && check_event_call == true)
            {
                //z = PositionY ; 
                v = v + a * ((float)1f / 5f);
                z = z - v * ((float)1f / 1.5f) /*+ (panel1.Height - z - ball_height)*/;

                if (z < 0)
                    z = 0;

                //set_ball_height(load_ball(z)); 
                set_ball_height(z);
                //set_ball_height_not_invert(z);

                if (z == 0)
                {
                    K = 0.5f * mass * v * v;
                    if (K > 0)
                    {
                        v = v * -1 * n;
                    }
                }
                label1.Text = z.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!check)
            {
                timer1.Enabled = true;
                button1.Text = "Stop";
                timer1.Start();
                check = true;
            }
            else
            {
                timer1.Enabled = false;
                button1.Text = "Start";
                timer1.Stop();
                check = false;

            }
            v = 0;
        }
        private void ball_creator(object sender, PaintEventArgs e)
        {
            //e.Graphics.Clear(BackPanel);
            e.Graphics.FillEllipse(brush, (int)PositionX, (int)PositionY, ball_width, ball_height);
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            check_event_call = false;
        }
        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {

            if (check_event_call == false)
            {
                // Y Axis . . . 
                long t = sp.ElapsedMilliseconds;
                int delta_Y = e.Y - mouse_last_y;
                if (t == 0) t = 1;
                float start_speed = delta_Y / t;
                mouse_last_y = e.Y;
                sp.Restart();
                label2.Text = start_speed.ToString();
                PositionX = e.X - ball_width / 2;
                v = start_speed * 20;
                set_ball_height(panel1.Height - e.Y - (ball_height / 2));
            }
        }
        private void panel_mouse_up(object sender, MouseEventArgs e)
        {
            check_event_call = true;
            sp.Restart();
        }


    }
}
