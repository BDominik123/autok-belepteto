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
            List<string> kesok = Kesok(adatok);
            Console.WriteLine("4. feladat");
            int menzasok = Menzasok(adatok);
            Console.WriteLine($"A menzán aznap {menzasok} tanuló ebédelt");
            Console.WriteLine("5. feladat");
            Konyvtar(adatok, menzasok);
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

        static List<string> Kesok(List<string> adatok)
        {
            List<string> kesok = new List<string>();
            for (int i = 0; i < adatok.Count; i++)
            {
                if (adatok[i].Split(' ')[2] == "1")
                {
                    if (TimeOnly.Parse(adatok[i].Split(' ')[1]) > TimeOnly.Parse("07:50"))
                    {
                        kesok.Add(adatok[i]);
                    }
                }
            }
            return kesok;
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
            List<string> kilepett = new List<string>();
            for (int i = 0; i < adatok.Count; i++)
            {
                if (adatok[i].Split(' ')[2] == "1" && TimeOnly.Parse(adatok[i].Split(' ')[1]) < TimeOnly.Parse("10:45") )
                {
                    kilepett.Add(adatok[i]);
                }
            }

            for(int i = 0; i < kilepett.Count; i++)
            {
                Console.WriteLine(kilepett[i]);
            }
        }
    }
}
