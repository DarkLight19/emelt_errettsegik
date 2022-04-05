using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace telefon
{
    class Program
    {
        class Adat
        {
            public int sorszam { get; set; }
            public int varakozasiIdotartam { get; set; }
            public int aktivIdotartam { get; set; }
            public (int ora, int perc, int masodperc) mettol { get; set; }
            public (int ora, int perc, int masodperc) felvevesIdo { get; set; }
            public (int ora, int perc, int masodperc) meddig { get; set; }

            public Adat()
            {

            }
            public Adat(int sorszam, (int,int,int) mettol, (int, int, int) felvevesIdo, (int, int, int) meddig, int var, int akt)
            {
                this.sorszam = sorszam;
                this.mettol = mettol;
                this.felvevesIdo = felvevesIdo;
                this.meddig = meddig;
                varakozasiIdotartam = var;
                aktivIdotartam = akt;
            }
        }
        static void Main(string[] args)
        {
            //1.feladat:
            Console.WriteLine("1.feladat:");

            //2.feladat:
            Console.WriteLine("2.feladat:");
            string[] s = System.IO.File.ReadAllLines("hivas.txt");
            List<Adat> x = new List<Adat>();

            (int ora, int perc, int masodperc) aktido = (8, 0, 0);
            int counter = 0;
            foreach (var item in s)
            {
                ++counter;
                string[] temps = item.Split(' ');
                (int ora, int perc, int masodperc) mettol = (int.Parse(temps[0]), int.Parse(temps[1]), int.Parse(temps[2]));
                (int ora, int perc, int masodperc) meddig = (int.Parse(temps[3]), int.Parse(temps[4]), int.Parse(temps[5]));

                if (mettol.ora >= 12 && mettol.perc >= 0 && mettol.masodperc > 0/*(aktido.ora >=12 && aktido.perc >= 0 && aktido.masodperc > 0) && mpbe(mettol.ora, mettol.perc, mettol.masodperc) > mpbe(aktido.ora, aktido.perc, aktido.masodperc)*/)
                    break;

                if (mpbe(meddig.ora, meddig.perc, meddig.masodperc) - mpbe(aktido.ora, aktido.perc, aktido.masodperc) >= 0)
                {
                    x.Add(new Adat(counter, mettol, aktido, meddig, (mpbe(aktido.ora, aktido.perc, aktido.masodperc) - mpbe(mettol.ora, mettol.perc, mettol.masodperc)), (mpbe(meddig.ora, meddig.perc, meddig.masodperc) - mpbe(aktido.ora, aktido.perc, aktido.masodperc))));
                    aktido = meddig;
                }
                else
                    x.Add(new Adat(counter, mettol, (-1, -1, -1), meddig, (mpbe(meddig.ora, meddig.perc, meddig.masodperc) - mpbe(mettol.ora, mettol.perc, mettol.masodperc)), -1));

            }

            Console.WriteLine(x.Count);

            //3.feladat:
            Console.WriteLine("3.feladat:");
            {
                Dictionary<int, int> dict = new Dictionary<int, int>();
                foreach (var item in x)
                {
                    if (dict.ContainsKey(item.mettol.ora))
                        ++dict[item.mettol.ora];
                    else
                        dict.Add(item.mettol.ora, 1);
                    /*
                    if (item.mettol.ora == item.meddig.ora)
                        continue;

                    if (dict.ContainsKey(item.meddig.ora))
                        ++dict[item.meddig.ora];
                    else
                        dict.Add(item.meddig.ora, 1);*/
                }

                foreach (var item in dict)
                    Console.WriteLine(item.Key + " " + item.Value);

            }

            //4.feladat:
            Console.WriteLine("4.feladat:");
            {
                int max = x.Max(k => k.varakozasiIdotartam + k.aktivIdotartam);
                foreach (var item in x)
                {
                    if (item.varakozasiIdotartam + item.aktivIdotartam != max)
                        continue;

                    Console.WriteLine(item.sorszam + " " + max);
                    break;
                }
            }

            //5.feladat:
            Console.WriteLine("5.feladat:");
            {
                Console.Write("Adjon meg egy idopontot! (ora perc masodperc) ");
                string[] temps = Console.ReadLine().Split(' ');
                (int ora, int perc, int masodperc) bekert = (int.Parse(temps[0]), int.Parse(temps[1]), int.Parse(temps[2]));
                int bekertMasodpercben = mpbe(bekert.ora, bekert.perc, bekert.masodperc);
                Console.WriteLine("Hívó: " + x.Where(k => k.aktivIdotartam != -1 && mpbe(k.felvevesIdo.ora, k.felvevesIdo.perc, k.felvevesIdo.masodperc) < bekertMasodpercben && mpbe(k.meddig.ora, k.meddig.perc, k.meddig.masodperc) > bekertMasodpercben).ToList()[0].sorszam + ".");
                Console.WriteLine("Várakozó: " + (x.Where(k => mpbe(k.mettol.ora, k.mettol.perc, k.mettol.masodperc) < bekertMasodpercben && mpbe(k.meddig.ora, k.meddig.perc, k.meddig.masodperc) > bekertMasodpercben).Count() - 1) + " db");
            }

            //6.feladat:
            Console.WriteLine("6.feladat:");
            {
                List<Adat> temp = x.Where(k => k.felvevesIdo.ora != -1).ToList();
                Console.WriteLine(temp[temp.Count - 1].sorszam + " " + temp[temp.Count - 1].varakozasiIdotartam);
            }

            //7.feladat:
            Console.WriteLine("7.feladat:");
            {
                using (System.IO.StreamWriter f = new System.IO.StreamWriter(" sikeres.txt"))
                {
                    foreach (var item in x.Where(k=>k.aktivIdotartam != -1))
                        f.WriteLine(item.sorszam + " " + item.felvevesIdo.ora + " " + item.felvevesIdo.perc + " " + item.felvevesIdo.masodperc + " " + item.meddig.ora + " " + item.meddig.perc + " " + item.meddig.masodperc);
                }
            }

        }

        public static int mpbe(int o, int p, int mp)
        {
            return o * 3600 + p * 60 + mp;
        }
    }
}
