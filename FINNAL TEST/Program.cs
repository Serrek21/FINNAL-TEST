using System;
using System.IO;

namespace FINNAL_TEST
{
    class Program
    {
        delegate void ZbikaDel(int[,] b, int s);

        static void Main(string[] args)
        {
            Console.WriteLine("=== Вітання! ===");
            Console.WriteLine("Виберіть режим гри:");
            Console.WriteLine("1. 8 (3x3)");
            Console.WriteLine("2. 15 (4x4)");
            int mode = int.Parse(Console.ReadLine());

            int s;
            if (mode == 1)
            {
                s = 3;
            }
            else
            {
                s = 4;
            }

            int[,] b = new int[s, s];
            ZrobBoard(b, s);

            Console.WriteLine("\nВиберіть режим розмішування:");
            Console.WriteLine("1. Ручне розмішування");
            Console.WriteLine("2. Комп'ютерне розмішування");
            int Mode = int.Parse(Console.ReadLine());

            ZbikaDel shuffle;
            if (Mode == 2)
            {
                shuffle = NeRuchZbirka;
            }
            else
            {
                shuffle = RuchZbirka;
            }

            shuffle(b, s);
            Play(b, s);
        }

        static void ZrobBoard(int[,] b, int s)
        {
            int value = 1;
            for (int i = 0; i < s; i++)
            {
                for (int j = 0; j < s; j++)
                {
                    if (i == s - 1 && j == s - 1)
                    {
                        b[i, j] = 0;
                    }
                    else
                    {
                        b[i, j] = value++;
                    }
                }
            }
        }

        static void NeRuchZbirka(int[,] b, int s)
        {
            Random r = new Random();
            for (int i = 0; i < s * s; i++)
            {
                int i1 = r.Next(s);
                int j1 = r.Next(s);
                int i2 = r.Next(s);
                int j2 = r.Next(s);
                int temp = b[i1, j1];
                b[i1, j1] = b[i2, j2];
                b[i2, j2] = temp;
            }
        }

        static void RuchZbirka(int[,] b, int s)
        {
            Console.WriteLine("Введіть послідовність розміщення елементів (використовуйте 0 для пустої клітинки):");
            for (int i = 0; i < s; i++)
            {
                for (int j = 0; j < s; j++)
                {
                    b[i, j] = int.Parse(Console.ReadLine());
                }
            }
        }

        static void Play(int[,] b, int s)
        {
            int m = 0;
            DateTime startT = DateTime.Now;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Гра ===");
                PrintBoard(b, s);

                if (CheckW(b, s))
                {
                    DateTime endT = DateTime.Now;
                    TimeSpan timeT = endT - startT;
                    Console.WriteLine("\nВітаємо, Ви виграли!");
                    Console.WriteLine($"Кількість ходів: {m}");
                    Console.WriteLine($"Час гри: {timeT.TotalSeconds} секунд");

                    Save(m, timeT);
                    break;
                }

                Console.WriteLine("\nВведіть номер плитки, яку хочете перемістити (0 для виходу):");
                int t = int.Parse(Console.ReadLine());

                if (t == 0) break;

                if (Move(b, s, t))
                {
                    m++;
                }
                else
                {
                    Console.WriteLine("Неправильний хід! Спробуйте ще раз.");
                    Console.ReadLine();
                }
            }
        }

        static void Save(int m, TimeSpan timeT)
        {
            Console.WriteLine("Введіть шлях до файлу для збереження результатів:");
            string fileN = Console.ReadLine();

            using (StreamWriter w = new StreamWriter(fileN, true))
            {
                w.WriteLine($"Кількість ходів: {m}");
                w.WriteLine($"Час гри: {timeT.TotalSeconds} секунд");
                w.WriteLine("-------------------------------------------------");
            }
            Console.WriteLine($"Результати гри збережено у файл {fileN}");
        }

        static void PrintBoard(int[,] b, int s)
        {
            int w = 3;
            string Hl = new string('═', s * w + (s - 1) * 2 + 1);
            Console.WriteLine("╔" + Hl + "╗");
            for (int i = 0; i < s; i++)
            {
                Console.Write("║");
                for (int j = 0; j < s; j++)
                {
                    if (b[i, j] == 0)
                    {
                        Console.Write("    ");
                    }
                    else
                    {
                        Console.Write($" {b[i, j],2} ");
                    }
                    if (j < s - 1)
                    {
                        Console.Write("║");
                    }
                }
                Console.WriteLine("║");
                if (i < s - 1)
                {
                    Console.WriteLine("╟" + new string('─', s * w + (s - 1) * 2 + 1) + "╢");
                }
            }
            Console.WriteLine("╚" + Hl + "╝");
        }

        static bool Move(int[,] b, int s, int t)
        {
            int tX = -1, tY = -1;
            int eX = -1, eY = -1;

            for (int i = 0; i < s; i++)
            {
                for (int j = 0; j < s; j++)
                {
                    if (b[i, j] == t)
                    {
                        tX = i;
                        tY = j;
                    }
                    else if (b[i, j] == 0)
                    {
                        eX = i;
                        eY = j;
                    }
                }
            }

            if ((Math.Abs(tX - eX) == 1 && tY == eY) || (Math.Abs(tY - eY) == 1 && tX == eX))
            {
                b[eX, eY] = t;
                b[tX, tY] = 0;
                return true;
            }

            return false;
        }

        static bool CheckW(int[,] b, int s)
        {
            int v = 1;
            for (int i = 0; i < s; i++)
            {
                for (int j = 0; j < s; j++)
                {
                    if (i == s - 1 && j == s - 1)
                    {
                        return b[i, j] == 0;
                    }

                    if (b[i, j] != v++)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}