using System;
using System.IO;
using System.Text;

namespace DataToGraph
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamWriter sw = new StreamWriter("result.csv", false, Encoding.UTF8);
            sw.WriteLine("Gen, Move");
            for (int i = 0; i < 215; i++)
            {
                using (StreamReader sr = new StreamReader($@"data\{i + 1}.txt"))
                {
                    var data = sr.ReadLine().Split(' ')[3];
                    sw.WriteLine($"{i + 1}, {data}");
                    sr.Close();
                }
            }
            sw.Close();
        }
    }
}
