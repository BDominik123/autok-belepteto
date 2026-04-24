using System.Diagnostics.Metrics;

namespace Belepteto
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<string> adatok = Beolvasas("bedat.txt");
            Console.WriteLine("2. feladat");
            LeghamarabbBelepett(adatok);
            LegkesobbElhagyta(adatok);
            Kesok(adatok);
            Console.WriteLine("4. feladat");
            int menzasok = Menzasok(adatok);
            Console.WriteLine($"A menzán aznap {menzasok} tanuló ebédelt");
            Console.WriteLine("5. feladat");
            Konyvtar(adatok, menzasok);
            Console.WriteLine("6. feladat");
            Console.WriteLine("Az érintett tanulók:");
            Kilepett(adatok);
            Console.WriteLine("7. feladat");
            ErkezesTavozasIdotartam(adatok);
        }

        static List<string> Beolvasas(string fajl)
        {
            List<string> adatok = new List<string>();
            StreamReader sr = new StreamReader(fajl);
            while (!sr.EndOfStream)
            {
                string sor = sr.ReadLine();
                adatok.Add(sor);
            }
            sr.Close();
            return adatok;
        }

        static void LeghamarabbBelepett(List<string> adatok)
        {
            TimeOnly leghamarabb = TimeOnly.Parse(adatok.FirstOrDefault(x => x.Split(' ')[2] == "1").Split(' ')[1]);
            for (int i = 1; i < adatok.Count; i++)
            {
                TimeOnly ido = TimeOnly.Parse(adatok[i].Split(' ')[1]);
                if (ido < leghamarabb)
                {
                    leghamarabb = ido;
                }
            }

            Console.WriteLine($"Az első tanuló {leghamarabb}-kor lépett be");
        }

        static void LegkesobbElhagyta(List<string> adatok)
        {
            TimeOnly legkesobb = TimeOnly.Parse(adatok.FirstOrDefault(x => x.Split(' ')[2] == "2").Split(' ')[1]);
            for (int i = 1; i < adatok.Count; i++)
            {
                TimeOnly ido = TimeOnly.Parse(adatok[i].Split(' ')[1]);
                if (ido > legkesobb)
                {
                    legkesobb = ido;
                }
            }
            Console.WriteLine($"Az utolsó tanuló {legkesobb}-kor hagyta el a termet");

        }

        static void Kesok(List<string> adatok)
        {
            StreamWriter sw = new StreamWriter("kesok.txt");
            for (int i = 0; i < adatok.Count; i++)
            {
                if (adatok[i].Split(' ')[2] == "1")
                {
                    if (TimeOnly.Parse(adatok[i].Split(' ')[1]) > TimeOnly.Parse("07:50"))
                    {
                        sw.WriteLine(adatok[i]);
                    }
                }
            }
            sw.Close();
        }
        static int Menzasok(List<string> adatok)
        {
            int counter = 0;
            for (int i = 0; i < adatok.Count; i++)
            {
                if (adatok[i].Split(' ')[2] == "3")
                {
                    counter++;
                }
            }
            return counter;
        }

        static void Konyvtar(List<string> adatok, int menzasok)
        {
            int counter = 0;
            for (int i = 0; i < adatok.Count; i++)
            {
                if (adatok[i].Split(' ')[2] == "4")
                {
                    counter++;
                }
            }
            Console.WriteLine($"Aznap {counter} tanuló kölcsönzött a könyvtárban");
            if (counter > menzasok)
            {
                Console.WriteLine("A könyvtárban volt több tanuló, mint a menzán");
            }
            else if (counter < menzasok)
            {
                Console.WriteLine("A menzán volt több tanuló, mint a könyvtárban");
            }
            else
            {
                Console.WriteLine("A menzán és a könyvtárban ugyanannyi tanuló volt");
            }

        }
        static void Kilepett(List<string> adatok)
        {
            HashSet<string> belepettSzunetElott = new HashSet<string>();
            HashSet<string> belepettSzunetUtan = new HashSet<string>();

            for (int i = 0; i < adatok.Count; i++)
            {
                var split = adatok[i].Split(" ");
                string nev = split[0];
                TimeOnly ido = TimeOnly.Parse(split[1]);
                string szam = split[2];
                if (szam == "1" && ido < TimeOnly.Parse("10:45"))
                {
                    belepettSzunetElott.Add(nev);
                }
                
                if(belepettSzunetElott.Contains(nev) && TimeOnly.Parse("10:50") > ido && szam == "2") belepettSzunetElott.Remove(nev);

                if (szam == "1" && ido > TimeOnly.Parse("10:50") && ido<TimeOnly.Parse("11:00"))
                {
                    belepettSzunetUtan.Add(nev);
                }
            }

            belepettSzunetElott.IntersectWith(belepettSzunetUtan);

            foreach (string item in belepettSzunetElott)
            {
                Console.WriteLine(item);
            }
        }

        static void ErkezesTavozasIdotartam(List<string> adatok)
        {
            List<string> orderedByReverseTime = adatok.OrderByDescending(x => x.Split(' ')[1]).ToList();
            Console.WriteLine("Egy tanuló azonosítója: ");
            string id = Console.ReadLine();
            var erkezesSor = adatok.Find(x => x.Contains(id));
            if (erkezesSor == null)
            {
                Console.WriteLine("Ilyen azonosítójú tanuló aznap nem volt az iskolában.");
            }
            else
            {
                TimeOnly erkezes = TimeOnly.Parse(erkezesSor.Split(' ')[1]);
                var tavozasSor = orderedByReverseTime.Find(x => x.Contains(id));
                TimeOnly tavozas = TimeOnly.Parse(tavozasSor.Split(' ')[1]);
                TimeSpan idotartam = tavozas.ToTimeSpan() - erkezes.ToTimeSpan();
                Console.WriteLine($"A tanuló érkezése és távozása között {Math.Round(idotartam.TotalHours)} óra {idotartam.TotalMinutes%60} perc telt el.");
            }
        }



    }
}
