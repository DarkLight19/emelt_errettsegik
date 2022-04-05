using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace otszaz
{
    class Adat
    {
        public List<string> dolgok { get; set; }
        public int db { get; set; }
        public int fizetendo { get; set; }
        public Adat()
        {
            dolgok = new List<string>();
        }

        public Adat(List<string> arucikkek)
        {
            dolgok = new List<string>(arucikkek);
            db = arucikkek.Count;
            fizetendoKiszamolas();
        }

        public void fizetendoKiszamolas()
        {
            fizetendo = 0;
            Dictionary<string, int> dict = new Dictionary<string, int>();
            foreach (var item in dolgok)
            {
                if(dict.ContainsKey(item))
                {
                    if (dict[item] == 1)
                        fizetendo += 450;
                    else
                        fizetendo += 400;
                }
                else
                {
                    dict.Add(item, 1);
                    fizetendo += 500;
                }    
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            //1. feladat:
            Console.WriteLine("1. feladat:");

            string[] s = System.IO.File.ReadAllLines("penztar.txt");
            List<Adat> x = new List<Adat>();

            List<string> temp = new List<string>();
            foreach (var item in s)
            {
                if (item != "F")
                {
                    temp.Add(item);
                    continue;
                }

                x.Add(new Adat(temp));
                temp = new List<string>();
            }
            //2.feladat:
            Console.WriteLine("2.feladat:");
            Console.WriteLine(x.Count);
            //3.feladat:
            Console.WriteLine("3.feladat:");
            Console.WriteLine(x[0].db);
            //4.feladat:
            Console.WriteLine("4.feladat:");

            Console.WriteLine("Vásárlás sorszáma: ");
            int sorszam = int.Parse(Console.ReadLine());

            Console.WriteLine("Árucikk neve: ");
            string arucikknev = Console.ReadLine();

            Console.WriteLine("Darabszám: ");
            int darabszam = int.Parse(Console.ReadLine());
            //5.feladat:
            Console.WriteLine("5.feladat:");
            Console.Write("a) ");
            bool elso = true;
            int tempDb = 0;
            int elsoSzam = 0;
            int utolsoSzam = 0;
            for (int i = 0; i < x.Count; i++)
            {
                if (x[i].dolgok.Contains(arucikknev))
                {
                    ++tempDb;

                    if (elso)
                    {
                        elso = false;
                        elsoSzam = i + 1;
                    }

                    utolsoSzam = i + 1;
                }
            }
            Console.WriteLine(elsoSzam + " " + utolsoSzam);
            Console.Write("b) ");
            Console.WriteLine(tempDb);
            //6.feladat:
            Console.WriteLine("6.feladat:");
            Console.WriteLine(ertek(darabszam));
            //7.feladat:
            Console.WriteLine("7.feladat:");
            List<string> used = new List<string>();
            foreach (var item in x[sorszam-1].dolgok)
            {
                if (used.Contains(item))
                    continue;

                Console.WriteLine(x[sorszam-1].dolgok.Where(k=>k.Contains(item)).Count() + " " + item);
                used.Add(item);
            }
            //8.feladat:
            Console.WriteLine("8.feladat:");
            using (System.IO.StreamWriter f = new System.IO.StreamWriter("osszeg.txt"))
            {
                for (int i = 0; i < x.Count; i++)
                    f.WriteLine(i + 1 + ": " + x[i].fizetendo);
            }
        }

        public static int ertek(int db)
        {
            if (db == 1)
                return 500;

            return 950 + (db - 2) * 400;
        }
    }
}
