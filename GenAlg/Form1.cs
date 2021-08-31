using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenAlg
{
    public partial class Form1 : Form
    {
        static Bitmap bmp = new Bitmap(7000, 5000);
        static Graphics graph = Graphics.FromImage(bmp);
        static Pen pen = new Pen(Color.Black, 1);
        static Pen pen_way = new Pen(Color.Red, 3);
        static Pen pen2 = new Pen(Color.DarkViolet, 2);
        static Pen penRed = new Pen(Color.Red, 3);
        static Pen penWhite = new Pen(Color.White, 4);
        Random random;
        //int[] x = {300, 580, 300, 20 }, y = {20, 300, 580, 300 }; 
        int[,] coordinates, m_weight;
        List<int[,]> AllMatrix ;
        List<List<int>> AllRes1, AllRes2;
        //    = { { 0, 10, 12, 16, 13, 12, 7, 6 },
        //                                 { 10, 0,  13, 8, 15, 8, 19, 5 },
        //                                 { 12, 13, 0, 16, 14, 12, 8, 6 },
        //                                 { 16, 8,  16, 0, 11, 12, 7, 9 },
        //                                 { 13, 15, 14, 11, 0,  9, 12, 8 },
        //                                 { 12, 8,  12, 12, 9,  0, 8, 15 },
        //                                 { 7,  19, 8,  7, 12,  8, 0, 19 },
        //                                 { 6,  5,  6,  9,  8, 15, 19, 0 }

        //};
        
        public Form1()
        {
            InitializeComponent();
            random = new Random();
            // graph.Clear(Color.White);
            // pictureBox1.Image = bmp;

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox_goldberg12.Clear();
            AllMatrix = new List<int[,]>();
            AllRes1 = new List<List<int>>();
            AllRes2 = new List<List<int>>();
            int res1 = 0, res2 = 0;
            for (int i = 0; i < Convert.ToInt32(textBox_count_matrix.Text); i++)
            {
                CreateMatr(trackBar1.Value, Convert.ToInt32(textBox_diap_a.Text), Convert.ToInt32(textBox_diapas_b.Text)); // матрица смежности
                AllMatrix.Add(m_weight);
                Genetik gen = new Genetik(Convert.ToInt32(textBox_replay.Text), Convert.ToInt32(textBox_individ.Text), trackBar1.Value, Convert.ToInt32(textBox_first_city.Text), Convert.ToInt32(textBox_cross.Text), Convert.ToInt32(textBox_mut.Text), m_weight);
                gen.CreateIndivid();
                AllRes1.Add(gen.Algorithm1());// first modification
                //AllRes2.Add(gen.Algorithm2());// second modification
                res1 += Way_weight(AllRes1[i]);
                //res2 += Way_weight(AllRes2[i]);
            }
            textBox_goldberg12.Text = "Первая модификация: "+res1.ToString() + Environment.NewLine;
            //textBox_goldberg12.Text += "Вторая модификация: " + res2.ToString();
            button_print_resolut_Click(sender, e);
        }
        private void button_print_resolut_Click(object sender, EventArgs e)
        {
            
            m_weight = AllMatrix[Convert.ToInt32(textBox_resolut_num.Text) - 1];
            textBox1.Clear();
            textBox_way.Clear();
            CreateCoord(trackBar1.Value, 370, 350);// nodes
            Draw_Node(trackBar1.Value, coordinates);
            CreateCoord(trackBar1.Value, 370, 342); // way
            Draw_Way(trackBar1.Value, m_weight, coordinates);
            PrintMatr(trackBar1.Value);

            Draw_Finale_Way(trackBar1.Value, m_weight, coordinates);
            //**********

            textBox_way.Text += Convert.ToInt32(textBox_first_city.Text) + "->";
            for (int i = 0; i < trackBar1.Value - 1; i++)
            {
                textBox_way.Text += AllRes1[Convert.ToInt32(textBox_resolut_num.Text)-1][i] + "->";
            }
            textBox_way.Text += Convert.ToInt32(textBox_first_city.Text)+ Environment.NewLine + "Route length = " + Way_weight(AllRes1[Convert.ToInt32(textBox_resolut_num.Text) - 1]);
            //textBox_way.Text += Environment.NewLine;
            //textBox_way.Text += Convert.ToInt32(textBox_first_city.Text) + "->";
            //for (int i = 0; i < trackBar1.Value - 1; i++)
            //{
            //    textBox_way.Text += AllRes2[Convert.ToInt32(textBox_resolut_num.Text) - 1][i] + "->";
            //}
            //textBox_way.Text += Convert.ToInt32(textBox_first_city.Text) + Environment.NewLine + "Длина маршрута = " + Way_weight(AllRes2[Convert.ToInt32(textBox_resolut_num.Text) - 1]);

        }

        private int Way_weight(List<int> indiv)
        {
            List<int> temp = new List<int>();
            temp.Add(Convert.ToInt32(textBox_first_city.Text));
            temp.AddRange(indiv);
            temp.Add(Convert.ToInt32(textBox_first_city.Text));
            int weight = 0;
            for (int i = 0; i < trackBar1.Value; i++)
            {
                weight += m_weight[temp[i] - 1, temp[i + 1] - 1];
            }
            return weight;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            textBox2.Text = Convert.ToString(trackBar1.Value);
        }

        double ToRadian(int x)
        {
            double res = (x * Math.PI) / 180;
            return res;
        }

        void Draw_Node(int count, int[,] coord)
        {
            graph.Clear(Color.White);

            for (int i = 0; i < count; i++)
            {                
                graph.DrawEllipse(pen2, coord[i,0], coord[i, 1], 20, 20);
                graph.DrawString(Convert.ToString(i+1), new Font("Arial", 10), Brushes.Black, coord[i, 0] + 2, coord[i, 1] + 3);                
            }
            pictureBox1.Image = bmp;
        }

        void Draw_Way(int count,int [,] matr, int [,] coord)
        {
            for (int i = 0; i < count - 1; i++)
            {
                for (int j = i + 1; j < count; j++)
                {
                    graph.DrawLine(pen, coord[i, 0]+9, coord[i, 1] + 9, coord[j, 0] + 9, coord[j, 1] + 9);
                    graph.DrawString(Convert.ToString(matr[i,j]), new Font("Arial", 12), Brushes.DarkMagenta, (coord[j, 0] + coord[i, 0])/2 ,(coord[j, 1] + coord[i, 1])/2);
                    
                }                
            }
            pictureBox1.Image = bmp;
        }
        void Draw_Finale_Way(int count, int[,] matr, int[,] coord)
        {
            int f_city = Convert.ToInt32(textBox_first_city.Text);
            graph.DrawLine(pen_way, coord[f_city - 1, 0] + 9, coord[f_city - 1, 1] + 9, coord[AllRes1[0][0] - 1, 0] + 9, coord[AllRes1[0][0] - 1, 1] + 9);
            AllRes1[0].Add(f_city);
            for (int i = 0; i < AllRes1[0].Count-1; i++)
            {
                
                    graph.DrawLine(pen_way, coord[AllRes1[0][i]-1, 0] + 9, coord[AllRes1[0][i]-1, 1] + 9, coord[AllRes1[0][i+1]-1, 0] + 9, coord[AllRes1[0][i+1] - 1, 1] + 9);
                    //graph.DrawString(Convert.ToString(matr[i, j]), new Font("Arial", 12), Brushes.DarkMagenta, (coord[j, 0] + coord[i, 0]) / 2, (coord[j, 1] + coord[i, 1]) / 2);

                
            }
            pictureBox1.Image = bmp;
        }

        void CreateCoord(int count, int center, int radius)
        {
            coordinates = new int[count, 2];
            double step = 360.00 / count;
            int num = 0;
            for (double i = 0; i < 359.5; i += step)
            {                
                coordinates[num, 0] = (int)(center + Math.Cos(ToRadian((int)i)) * radius); //x
                coordinates[num, 1] = (int)(center - Math.Sin(ToRadian((int)i)) * radius); //y
                num++;
            }
        }

        

        void CreateMatr(int count, int diap_l, int diap_r)
        {
            m_weight = new int[count, count];
            for (int i = 0; i < count-1; i++)
            {
                for (int j = i+1; j < count; j++)
                {
                    m_weight[i, j] = random.Next(diap_l, diap_r);
                }
            }
            for (int i = 1; i < count; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    m_weight[i, j] = m_weight[j, i];
                }
            }
        }

        void PrintMatr(int count)
        {
            //Console.Write(a[i, j].ToString().PadRight(3));

            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    textBox1.Text += m_weight[i, j].ToString()+new string(' ',5 - m_weight[i, j].ToString().Length);
                }
                textBox1.Text += Environment.NewLine; 
            }
        }

        
    }

    
}
