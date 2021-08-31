using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using GraphX;
using System.Drawing;

namespace GenAlg
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            
        }
    }
}


class Genetik
{
    int  replay,count_individ, count_city, first_city, chance_cross, chance_mut;
    int num_best;
    List<List<int>> individ = new List<List<int>>();
    List<List<int>> next_gen = new List<List<int>>();
    int[,] matrix;
    //List<int>[] ind = new List<int>[5];


    public Genetik(int repl,int cnt_ind, int cnt_city, int f_city, int cross, int mut, int[,] matr)// подаем количество особей, городов, кроссовер/мутация, начальный город
    {
        replay = repl;
        count_individ = cnt_ind;
        count_city = cnt_city;
        first_city = f_city;
        chance_cross = cross;
        chance_mut = mut;
        matrix = matr;
    }

    public void CreateIndivid()
    {                  
        Random rnd = new Random();
        int tmp = 0;
        for (int i = 0; i < count_individ; i++)
        {
            individ.Add(new List<int>());
            next_gen.Add(new List<int>()); // для следующего поколения
            for (int j = 0; j < count_city-1; )
            {
                tmp = rnd.Next(1, count_city+1);
                if (!individ[i].Contains(tmp) && tmp!=first_city)// & / ||
                {
                    individ[i].Add(tmp);
                    //str += tmp;
                    j++;
                }                
            }          
                        
            
        }       
        
    }
    public void Cross(int num_ind, bool mut)
    {
        Random rand = new Random();
        int dot_cros = rand.Next(1,count_city-1);//??
        int chance_1 = rand.Next(0, 100);
        int chance_2 = rand.Next(0, 100);
        int partner = num_ind;
        if (chance_1 < chance_cross)
        {
            while (partner == num_ind)
                partner = rand.Next(0, count_individ); //??
            List<int> tmp1 = new List<int>(), left_1 = new List<int>();//, right_1 = new List<int>();
            List<int> tmp2 = new List<int>(), left_2 = new List<int>();//, right_2 = new List<int>();
            
            if (chance_2 < chance_mut)//************************************* cross + mut
            {
                for (int i = 0; i < dot_cros; i++)/////////////////////////
                {
                    left_1.Add(individ[num_ind][i]);
                    tmp1.Add(left_1[i]);
                    left_2.Add(individ[partner][i]);
                    tmp2.Add(left_2[i]);
                }
                for (int i = dot_cros; i < count_city - 1; i++) //
                {
                    for (int j = dot_cros; j < count_city - 1; j++)
                    {
                        if (left_2.Contains(individ[num_ind][j]) && !left_1.Contains(individ[partner][j]))
                        {                            
                            left_2.Add(individ[partner][j]);
                        }
                        if (!left_2.Contains(individ[num_ind][j]) && left_1.Contains(individ[partner][j]))
                        {
                            left_1.Add(individ[num_ind][j]);                            
                        }
                    }

                }//
                for (int i = dot_cros; i < count_city - 1; i++)//??
                {
                    if (!left_2.Contains(individ[num_ind][i]) && !left_1.Contains(individ[partner][i]))//??
                    {
                        tmp1.Add(individ[partner][i]);////tut*****8
                        tmp2.Add(individ[num_ind][i]);
                    }
                    else
                    {
                        tmp1.Add(individ[num_ind][i]);////tut*****8
                        tmp2.Add(individ[partner][i]);
                    }

                }///////////////////
                if(!mut)
                    BestWayCross(Mutation(tmp1), Mutation(tmp2), num_ind);
                else
                    BestWayCross(Mutation2(tmp1), Mutation2(tmp2), num_ind);
                ////////// проверка на лучшую особь //+

            } // +,+,-
            else //***********************************************************crossover
            {
                for (int i = 0; i < dot_cros; i++)/////////////////////////
                {
                    left_1.Add(individ[num_ind][i]);
                    tmp1.Add(left_1[i]);
                    left_2.Add(individ[partner][i]);
                    tmp2.Add(left_2[i]);
                }
                for (int i = dot_cros; i < count_city - 1; i++) //
                {
                    for (int j = dot_cros; j < count_city - 1; j++)
                    {
                        if (left_2.Contains(individ[num_ind][j]) && !left_1.Contains(individ[partner][j]))
                        {                            
                            left_2.Add(individ[partner][j]);
                        }
                        if (!left_2.Contains(individ[num_ind][j]) && left_1.Contains(individ[partner][j]))
                        {
                            left_1.Add(individ[num_ind][j]);                            
                        }
                    }

                }//
                for (int i = dot_cros; i < count_city-1; i++)//??
                {
                    if (!left_2.Contains(individ[num_ind][i]) && !left_1.Contains(individ[partner][i]))//??
                    {
                        tmp1.Add(individ[partner][i]);////tut*****8
                        tmp2.Add(individ[num_ind][i]);
                    }
                    else
                    {
                        tmp1.Add(individ[num_ind][i]);////tut*****8
                        tmp2.Add(individ[partner][i]);
                    }

                }/////////////////////////////////////////////--
                BestWayCross(tmp1, tmp2, num_ind);                
                ////////// проверка на лучшую особь
            }  //     +,    - 
        }
        else if (chance_2 < chance_mut) //*****************************MUTATION**************
        {
            List<int> tmp = new List<int>();
            if (!mut)
                tmp.AddRange(Mutation(individ[num_ind]));
            else
                tmp.AddRange(Mutation2(individ[num_ind]));

            if (Way_weight(individ[num_ind]) > Way_weight(tmp))
                next_gen[num_ind].AddRange(tmp);
            else
                next_gen[num_ind].AddRange(individ[num_ind]);
            
        }// +,+
        else
            next_gen[num_ind].AddRange(individ[num_ind]);


    }

    public List<int> Mutation(List<int> list)
    {
        Random rand = new Random();
        int tmp = 0, city_1 = 0, city_2 = 0;

        city_1 = rand.Next(0, count_city - 1);
        city_2 = city_1;
        while (city_2 == city_1)
        {
            city_2 = rand.Next(0, count_city - 1);
        }
        tmp = list[city_1];
        list[city_1] = list[city_2];
        list[city_2] = tmp;

        return list;

    }

    public List<int> Mutation2(List<int> list)//
    {
        Random rand = new Random();
        int tmp_left = 0, tmp_right = 0, city_left_1 = 0, city_left_2 = 0, city_right_1 = 0, city_right_2 = 0;

        city_left_1 = rand.Next(0, count_city - 4);
        city_left_2 = city_left_1 + 1;

        city_right_1 = rand.Next(city_left_2+1, count_city - 2);
        city_right_2 = city_right_1 + 1;

        tmp_left = list[city_left_1];
        list[city_left_1] = list[city_right_1];
        list[city_right_1] = tmp_left;

        tmp_right = list[city_left_2];
        list[city_left_2] = list[city_right_2];
        list[city_right_2] = tmp_right;

        return list;

    }

    private void BestWayCross(List<int> tmp1, List<int> tmp2,int num_ind)
    {
        if (Way_weight(tmp1) < Way_weight(tmp2)) //
        {
            if (Way_weight(tmp1) < Way_weight(individ[num_ind]))
            {
                next_gen[num_ind].AddRange(tmp1);
            }
            else
                next_gen[num_ind].AddRange(individ[num_ind]);

        }
        else
        {
            if (Way_weight(tmp2) < Way_weight(individ[num_ind]))
            {
                next_gen[num_ind].AddRange(tmp2);
            }
            else
                next_gen[num_ind].AddRange(individ[num_ind]);
        }
    }

    

 
    private int Way_weight(List<int> indiv)
    {
        List<int> temp = new List<int>();
        temp.Add(first_city);
        temp.AddRange(indiv);
        temp.Add(first_city);
        int weight = 0;
        for (int i = 0; i < count_city; i++)
        {
            weight += matrix[temp[i]-1, temp[i + 1]-1];
        }
        return weight;
    }

    private int All_Weight()
    {
        int min = 1024*1024;
        for (int i = 0; i < count_individ; i++)
        {
            if (Way_weight(individ[i])< min)
            {
                min = Way_weight(individ[i]);
                num_best = i;
            }            
        }

        return min;
    }

    public List<int> Algorithm1()
    {
        int stop = 0, min_weight = All_Weight();
        //List<int> Best = new List<int>();
        //Random rand = new Random();
        while (stop <replay)
        {
            for (int i = 0; i < count_individ; i++) // основной цикл
            {
                Cross(i,false);                
            }
            for (int i = 0; i < count_individ; i++)
            {
                individ[i].Clear();
                individ[i].AddRange(next_gen[i]);
                next_gen[i].Clear();
            }
            if (min_weight > All_Weight())
            {
                min_weight = All_Weight();
                stop = 0;
            }
            else
                stop++;
        }
        return individ[num_best];
    }

    public List<int> Algorithm2()
    {
        int stop = 0, min_weight = All_Weight();
        //List<int> Best = new List<int>();
        //Random rand = new Random();
        while (stop < replay)
        {
            for (int i = 0; i < count_individ; i++) // основной цикл
            {
                Cross(i,true);
            }
            for (int i = 0; i < count_individ; i++)
            {
                individ[i].Clear();
                individ[i].AddRange(next_gen[i]);
                next_gen[i].Clear();
            }
            if (min_weight > All_Weight())
            {
                min_weight = All_Weight();
                stop = 0;
            }
            else
                stop++;
        }
        return individ[num_best];
    }
}





