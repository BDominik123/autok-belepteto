using System.Diagnostics.Metrics;

namespace Autok
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<string> autok = Beolvasas("jeladas.txt");
            Console.WriteLine("2. feladat");
            UtolsoJeladas(autok);
            Console.WriteLine("3. feladat");
            ElsoAutoJelzesei(autok);
            Console.WriteLine("4. feladat");
            JeladasSzamlalo(autok);
            Console.WriteLine("5. feladat");
            LegnagyobbSebesseg(autok);
            Console.WriteLine("\n6. feladat");
            JarmuJelzeseiEsTavolsaga(autok);
            FajlbaIrIdoAdatokat(autok);
        }

        static List<string> Beolvasas(string fajl)
        {
            List<string> autok = new List<string>();
            StreamReader sr = new StreamReader(fajl);
            while (!sr.EndOfStream)
            {
                string sor = sr.ReadLine();
                autok.Add(sor);
            }
            sr.Close();
            return autok;
        }

        static void UtolsoJeladas(List<string> autok)
        {
            string[] utolso = autok[autok.Count - 1].Split("\t");
            TimeOnly idopont = TimeOnly.Parse($"{utolso[1]}:{utolso[2]}");
            Console.WriteLine($"Az utolsó jeladás időpontja {idopont}, a jármű rendszáma: {utolso[0]}");
        }

        static void ElsoAutoJelzesei(List<string> autok)
        {
            string[] elso = autok[0].Split("\t");
            List<TimeOnly> idok = new List<TimeOnly>();
            for (int i = 0; i < autok.Count; i++)
            {
                if (autok[i].Contains(elso[0]))
                {
                    string[] jelzes = autok[i].Split("\t");
                    idok.Add(TimeOnly.Parse($"{jelzes[1]}:{jelzes[2]}"));
                }
            }
            Console.WriteLine($"Az első jármű: {elso[0]}");
            Console.Write("Jeladásainak időpontja: ");
            foreach (var ido in idok) Console.Write($"{ido} ");
        }

        static void JeladasSzamlalo(List<string> autok)
        {
            Console.Write("Kérem, adja meg az órát: ");
            string ora = Console.ReadLine();
            Console.Write("Kérem, adja meg a percet: ");
            string perc = Console.ReadLine();
            int counter = 0;
            for (int i = 0; i < autok.Count; i++)
            {
                string[] split = autok[i].Split("\t");
                string ciklusOra = split[1];
                string ciklusPerc = split[2];
                if ($"{ora}:{perc}" == $"{ciklusOra}:{ciklusPerc}") counter++;
                else if (counter != 0)
                {
                    break;
                }
            }
            Console.WriteLine($"A jeladások száma: {counter}");
        }

        static void LegnagyobbSebesseg(List<string> autok)
        {
            int maxSebesseg = autok.Select(sor => int.Parse(sor.Split('\t')[3])).Max();
            string[] auto = autok.Where(sor => int.Parse(sor.Split('\t')[3].Trim()) == maxSebesseg).Select(sor => sor.Split('\t')[0]).ToArray();

            Console.WriteLine($"a legnagyobb sebesség: {maxSebesseg}");
            Console.Write("A járművek: ");
            foreach (string s in auto) Console.Write(s + " ");
        }

        static void JarmuJelzeseiEsTavolsaga(List<string> autok)
        {
            Console.WriteLine("Kérem, adja meg a rendszámot: ");
            string input = Console.ReadLine();
            List<TimeOnly> idopontok = new List<TimeOnly>();
            List<int> sebesseg = new List<int>();
            for (int i = 0; i < autok.Count; i++)
            {
                string[] split = autok[i].Split("\t");
                if (split[0] == input)
                {
                    idopontok.Add(TimeOnly.Parse($"{split[1]}:{split[2]}"));
                    sebesseg.Add(int.Parse(split[3]));
                }
            }
            double osszkm = 0;

            for (int i = 0; i < idopontok.Count - 1; i++)
            {
                TimeSpan elteltIdo = idopontok[i + 1].ToTimeSpan() - idopontok[i].ToTimeSpan();
                double elteltPerc = (double)elteltIdo.TotalMinutes;
                Console.WriteLine($"{idopontok[i]} {osszkm} km");
                osszkm += Math.Round(elteltPerc * sebesseg[i] / 60, 1);

            }
            if (osszkm > 0)
            {
                Console.WriteLine($"{idopontok[idopontok.Count - 1]} {osszkm} km");
            }

        }
        static void FajlbaIrIdoAdatokat(List<string> autok)
        {
            Dictionary<string, string> statisztika = new Dictionary<string, string>();

            foreach (string sor in autok)
            {
                string[] adatok = sor.Split('\t');
                string rendszam = adatok[0];
                string ora = adatok[1];
                string perc = adatok[2];

                if (!statisztika.ContainsKey(rendszam))
                {
                    statisztika[rendszam] = $"{ora} {perc} {ora} {perc}";
                }
                else
                {
                    string[] regiIdok = statisztika[rendszam].Split(' ');
                    statisztika[rendszam] = $"{regiIdok[0]} {regiIdok[1]} {ora} {perc}";
                }
            }

            using (StreamWriter sw = new StreamWriter("ido.txt"))
            {
                foreach (var bejegyzes in statisztika)
                {
                    sw.WriteLine($"{bejegyzes.Key} {bejegyzes.Value}");
                }
            }
        }
    }
}
