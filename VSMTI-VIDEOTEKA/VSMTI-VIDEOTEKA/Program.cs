/*
 * -------------------------------------------
 * 
 * Autor: Brigita Perković
 * Projekt: Evidencija filmova u videoteci
 * Predmet: Osnove programiranja
 * Ustanova: VŠMTI
 * Godina: 2019./2020.
 * 
 * -------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;


namespace VSMTI_VIDEOTEKA
{
    public struct Korisnik {
        public string ime;
        public string prezime;
        public string korisnickoIme;
        public string lozinka;

        public Korisnik(string i,string p, string k, string l ) {
            ime = i;
            prezime = p;
            korisnickoIme = k;
            lozinka = l;
        }
    }

    public struct Film {
        public int redniBroj;
        public string sifra;
        public string nazivFilma;
        public string zanrFilm;
        public string redatelj;
        public string glumci;

        public Film(int rb, string s, string n , string z, string r, string g)
        {
            redniBroj = rb;
            sifra = s;
            nazivFilma = n;
            zanrFilm = z;
            redatelj = r;
            glumci = g;
        }
    }

    class Program
    {
        private static IEnumerable<Film> listaFilmova;

        static void Main(string[] args)
        {
            /*
             * ------------------------------------------------------
             * Datoteka sa vremenom izvođenja akcije i naziom akcije.
             * ------------------------------------------------------
             */

            string path = "C:\\Users\\Kriščn Perkz\\Desktop\\VSMTI-PROJEKT\\VSMTI-VIDEOTEKA\\VSMTI-VIDEOTEKA\\Datoteke\\logovi.txt";
            DateTime vrijeme = DateTime.Now;
            string pokretanje = vrijeme.ToString()+ " - Pokretanje programa.";
            File.WriteAllText(path, pokretanje); 
            XmlSerializer xsKorisnici;
            List<Korisnik> korisnici = new List<Korisnik>();
            xsKorisnici = new XmlSerializer(typeof(List<Korisnik>));
            korisnici = dohvatiKorisnikeIzXMLDatoteke(xsKorisnici);
            prikaziPrijavuIRegistraciju(xsKorisnici, korisnici);
        }

        private static void prikaziPrijavuIRegistraciju(XmlSerializer xs, List<Korisnik> korisnici) {
            while (true) {
                Console.WriteLine();
                Console.WriteLine("-----IZBORNIK-----");
                Console.WriteLine("1. Prijava");
                Console.WriteLine("2. Registracija");
                Console.WriteLine("3. Izlaz");
                Console.Write("Vaš odabir: ");
                string odabirUStringu = Console.ReadLine();
                int odabir;
                if (int.TryParse(odabirUStringu, out odabir))
                {
                    switch (odabir)
                    {

                        case 1:
                            prijavaKorisnika(korisnici);
                            break;
                        case 2:
                            registracijaKorisnika(korisnici,xs);

                            break;
                        case 3:                            
                            DateTime vrijeme = DateTime.Now;
                            string izlazIzPrograma = vrijeme.ToString() + " - Izlaz iz programa.";
                            pracenjeRadaPrograma(izlazIzPrograma);
                            return;                       
                    }                   
                }
            }
        }

        private static void pracenjeRadaPrograma(string zapis)
        {
            StreamWriter sw = new StreamWriter("C:\\Users\\Kriščn Perkz\\Desktop\\VSMTI-PROJEKT\\VSMTI-VIDEOTEKA\\VSMTI-VIDEOTEKA\\Datoteke\\logovi.txt", true);
            sw.WriteLine(zapis);
            sw.Close();
        }

        private static void registracijaKorisnika(List<Korisnik> korisnici, XmlSerializer xs)
        {
            /*
             * ______________________________________________________________
             * 
             * Provjerava dali već postoji korisnik koji se želi registrirati
             * 
             * ______________________________________________________________
             */
            
                
            if (dodajKorisnika(korisnici, xs)) {
                Console.WriteLine("Uspjesno ste se registrirali!");
            }
            else
            {
                Console.WriteLine("Korisnik s tim korisnickim imenom vec postoji!");
            }
            
        }

        private static void prijavaKorisnika(List<Korisnik> korisnici)
        {
            /*
             * -------------------------------------------------
             * Prijava postoječeg korisnika upisanog u datoteku!
             * -------------------------------------------------
             */
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("----Prijava korisnika----");
            Console.Write("Unesite korisnicko ime: ");
            string korisnickoIme = Console.ReadLine();
            Console.Write("Unesite lozinku: ");
            string lozinka = Console.ReadLine();

            bool provjeraPrijave = provjeraPrijaveKorisnika(korisnickoIme, lozinka,korisnici);

            if (provjeraPrijave)
            {
                /*
                 * --------------------------------------------
                 * Projera dali je  korisnik upisan u datoteku!
                 * --------------------------------------------
                 */
                XmlSerializer xs;
                List<Film> filmovi = new List<Film>();
                xs = new XmlSerializer(typeof(List<Film>));
                filmovi = dohvatiFilmoveIzXMLDatoteke(xs);
                DateTime vrijeme = DateTime.Now;
                string prijava = vrijeme.ToString() + " - Uspješna prijava korisnika " +korisnickoIme+".";
                pracenjeRadaPrograma(prijava);
                prikaziFilmoveMain(xs,filmovi);
            }
            else {
                Console.WriteLine("Unjeli ste pogresno korisnicko ime ili lozinku!!!");
                Console.WriteLine();
                Console.Write("Pritisnite ENTER za povratak u glavni izbornik ili ESC za izlaz iz programa");
                var tipka = Console.ReadKey();
                if (tipka.Key == ConsoleKey.Enter)
                {
                    Console.Clear();
                }
                if (tipka.Key == ConsoleKey.Escape)
                {
                    DateTime vrijeme = DateTime.Now;
                    string izlazIzPrograma = vrijeme.ToString() + " - Izlaz iz programa.";
                    pracenjeRadaPrograma(izlazIzPrograma);
                    return;
                }
            }           
        }

        private static void prikaziFilmoveMain(XmlSerializer xs,List<Film> filmovi) {
            DateTime vrijeme = DateTime.Now;
            string izbornik = vrijeme.ToString() + " - Prikaz izbornika za prijavljenje korisnike.";
            pracenjeRadaPrograma(izbornik);
            Console.Clear();
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("-----IZBORNIK-----");
                Console.WriteLine("1. Prikaži sve filmove");
                Console.WriteLine("2. Pretraži filmove");
                Console.WriteLine("3. Dodaj film");
                Console.WriteLine("4. Obriši film");               
                Console.WriteLine("5. Odjava");
                Console.Write("Vaš odabir: ");
                string odabirUStringu = Console.ReadLine();
                int odabir;
                if (int.TryParse(odabirUStringu, out odabir))
                {
                    switch (odabir)
                    {

                        case 1:
                            prikaziFilmove(filmovi);
                            break;
                        case 2:
                            pretraziFilmove(filmovi);
                            break;
                        case 3:
                            dodajFilm(filmovi, xs);
                            break;
                        case 4:
                            obrisiFilm(filmovi, xs);
                            break;                     
                        case 5:
                            Console.Clear();
                             vrijeme = DateTime.Now;
                            string izlazIzPrograma = vrijeme.ToString() + " - Izlaz iz programa.";
                            pracenjeRadaPrograma(izlazIzPrograma);
                            return;                            
                    }                   
                }               
            }            
        }
                                                                        
        private static bool provjeraPrijaveKorisnika(string korisnickoIme, string lozinka, List<Korisnik> korisnici)
        {
            foreach (Korisnik k in korisnici)
            {
                if (k.korisnickoIme == korisnickoIme && k.lozinka==lozinka)
                {
                    return true;
                }
            }
            return false;
        }

        private static List<Film> dohvatiFilmoveIzXMLDatoteke(XmlSerializer xs)
        {
            FileStream fs = dajFileStreamCitanje();
            List<Film> filmovi = (List<Film>) xs.Deserialize(fs);
            fs.Close();
            return filmovi;
        }

        private static List<Korisnik> dohvatiKorisnikeIzXMLDatoteke(XmlSerializer xs)
        {
            FileStream fs = dajFileStreamCitanjeKorisnici();
            List<Korisnik> korisnici = (List<Korisnik>)xs.Deserialize(fs);
            fs.Close();
            return korisnici;
        }

        private static FileStream dajFileStreamCitanje()
        {
            
            return new FileStream("C:\\Users\\Kriščn Perkz\\Desktop\\VSMTI-PROJEKT\\VSMTI-VIDEOTEKA\\VSMTI-VIDEOTEKA\\filmovi.xml", FileMode.Open, FileAccess.Read);
        }

        private static FileStream dajFileStreamPisanje()
        {
           
            return new FileStream("C:\\Users\\Kriščn Perkz\\Desktop\\VSMTI-PROJEKT\\VSMTI-VIDEOTEKA\\VSMTI-VIDEOTEKA\\filmovi.xml", FileMode.Create, FileAccess.Write);
        }
        
        private static void obrisiFilm(List<Film> listaFilmova, XmlSerializer xs)
        {
            /*
             * --------------------------------------------
             * Pretraga filma po šifri te brisanje!
             * --------------------------------------------
             */
            Console.Clear();
            Console.WriteLine();
            Console.Write("Unesite sifru filma: ");
            string sifra = Console.ReadLine();
            Film filmZaBrisanje=new Film();
            foreach (Film trenutniFilm in listaFilmova)
            {
                if (trenutniFilm.sifra.ToString() == sifra)
                {
                   filmZaBrisanje=trenutniFilm;
                }
            }
            DateTime vrijeme = DateTime.Now;
            string brisanje = vrijeme.ToString() + " - Obrisan film "+ filmZaBrisanje.nazivFilma +".";
            pracenjeRadaPrograma(brisanje);
            listaFilmova.Remove(filmZaBrisanje);
            FileStream fs = dajFileStreamPisanje();
            xs.Serialize(fs, listaFilmova);
            fs.Close();
            Console.Write("Pritisnite ENTER za povratak u glavni izbornik ili ESC za izlaz iz programa");
            while (Console.ReadKey().Key != ConsoleKey.Enter) { }
        }

        private static void dodajFilm(List<Film> listaFilmova, XmlSerializer xs)
        {
            /*
             * ----------------------------------------------
             * Za dodavanje filma preko konzole u datoteku!
             * ----------------------------------------------
             */
            Console.Clear();           
            int brojElemenata=listaFilmova.Count();
            Film noviFilm = new Film();
            Console.Write("Unesite sifru: ");
            noviFilm.sifra = Console.ReadLine();
            noviFilm.redniBroj = brojElemenata;
            Console.Write("Unesite naziv: ");
            noviFilm.nazivFilma = Console.ReadLine();
            Console.Write("Unesite zanr: ");
            string zanr = Console.ReadLine();
            if(zanr=="")
            {
                noviFilm.zanrFilm = "Zanr nepoznat";
            }
            else
            {
                noviFilm.zanrFilm = zanr;
            }
           
            
            
            Console.Write("Unesite redatelja: ");
            noviFilm.redatelj = Console.ReadLine();
            //string nemaRedatelja = Console.ReadLine();
            //if (nemaRedatelja == noviFilm.redatelj)
            //{
            //    Console.WriteLine("Redatelj nepoznat");
            //}
            Console.Write("Unesite glumce: ");
            noviFilm.glumci = Console.ReadLine();
            
            
            listaFilmova.Add(noviFilm);
            FileStream fs = dajFileStreamPisanje();
            xs.Serialize(fs, listaFilmova);
            fs.Close();
            DateTime vrijeme = DateTime.Now;
            string bdodavanje = vrijeme.ToString() + " - Dodan film " + noviFilm.nazivFilma + ".";
            pracenjeRadaPrograma(bdodavanje);
            Console.Write("Pritisnite ENTER za povratak u glavni izbornik ili ESC za izlaz iz programa");
            while (Console.ReadKey().Key != ConsoleKey.Enter) { }                                 
        }

        private static void pretraziFilmove(List<Film> listaFilmova)
        {
            DateTime vrijeme = DateTime.Now;
            string izbornik2 = vrijeme.ToString() + " - Prikaz izbornika za pretraživanje po karakteristikama.";
            pracenjeRadaPrograma(izbornik2);
            /*
             * -------------------------------------------
             * Za pretraživanje filma po nazivu!
             * -------------------------------------------
             */
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("----PRETRAZITE FILM PO ZELJENOJ KARAKTERISTICI---- ");
            Console.WriteLine("1. Redni broj");
            Console.WriteLine("2. Sifra");
            Console.WriteLine("3. Naziv");
            Console.WriteLine("4. Zanr");
            Console.WriteLine("5. Redatelj");
            Console.WriteLine("6. Glumci");
            Console.Write("Vas odabir: ");
            string pretragaUStringu = Console.ReadLine();
            int pretraga;
            if (int.TryParse(pretragaUStringu, out pretraga))
            {
                switch (pretraga)
                {
                    case 1:
                        pretrazivanjePoRbr(listaFilmova);
                        break;
                    case 2:
                        pretrazivanjePoSifri(listaFilmova);
                        break;
                    case 3:
                        pretragaPoNazivu(listaFilmova);
                        break;
                    case 4:
                        pretragaPoZanru(listaFilmova);
                        break;
                    case 5:
                        pretragaPoRedatelju(listaFilmova);
                            break;
                    case 6:
                        pretragaPoGlumcima(listaFilmova);
                            break;

                }                
            }           
        }

        private static void pretragaPoGlumcima(List<Film> listaFilmova)
        {
            /*
             * ------------------------
             * Pretraživanje po glumcu!
             * ------------------------
             */
            Console.Clear();
            Console.WriteLine();
            List<Film> filtriraniFilmovi = new List<Film>();
            Console.Write("Unesite ime glumca: ");
            string glumac = Console.ReadLine();           
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("----Popis filmova----");
            string formatString = "{0,1}{1,20}{2,1}{3,20}{4,1}{5,20}{6,1}{7,20}{8,1}{9,20}{10,1}{11,20}{12,1}";
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------");
            string result = String.Format(formatString,"|",
                              "Redni broj", "|", "Sifra", "|", "Naziv", "|", "Zanr", "|", "Redatelj", "|", "Glumci", "|");
            Console.WriteLine(result);
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------");
            foreach (Film trenutniFilm in listaFilmova )
            {
                List<string> listaGlumci = trenutniFilm.glumci.Split(',').ToList();
                int brojac = 0;
                foreach (string kakocemisezvatvarijabla in listaGlumci)
                {
                    if (glumac == kakocemisezvatvarijabla.Trim())
                    {
                        filtriraniFilmovi.Add(trenutniFilm);
                        
                    } 
                    
                }
             
            }
            foreach(Film trenutniFilm in filtriraniFilmovi)
            {
                string sifra = trenutniFilm.sifra;
                string zanr = trenutniFilm.zanrFilm;
                string naziv = trenutniFilm.nazivFilma;
                string redatelj = trenutniFilm.redatelj;
                string glumci = trenutniFilm.glumci;
                if (trenutniFilm.nazivFilma.Length > 19)
                {
                    naziv = trenutniFilm.nazivFilma.Substring(0, 16) + "...";
                }
                if (trenutniFilm.glumci.Length > 19)
                {
                    glumci = trenutniFilm.glumci.Substring(0, 16) + "...";
                }
                if (trenutniFilm.redatelj.Length > 19)
                {
                    redatelj = trenutniFilm.redatelj.Substring(0, 16) + "...";
                }
                if (trenutniFilm.zanrFilm.Length > 19)
                {
                    zanr = trenutniFilm.zanrFilm.Substring(0, 16) + "...";
                }
                List<string> listaGlumci = trenutniFilm.glumci.Split(',').ToList();
                int brojac = 0;
                foreach (string kakocemisezvatvarijabla in listaGlumci)
                {
                    if (brojac == 0)
                    {
                        result = String.Format(formatString, "|",
                               trenutniFilm.redniBroj, "|", sifra, "|", naziv, "|", zanr, "|",
                               redatelj, "|", kakocemisezvatvarijabla, "|");
                    }
                    else

                    {
                        result = String.Format(formatString, "|",
                                                     "", "|", "", "|", "", "|", "", "|",
                                                      "", "|", kakocemisezvatvarijabla, "|");
                    }
                    Console.WriteLine(result);
                    Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------");
                    brojac++;
                }
            }
            Console.Write("Pritisnite ENTER za povratak u glavni izbornik ili ESC za izlaz iz programa");
            var tipka = Console.ReadKey();
            if (tipka.Key == ConsoleKey.Enter)
            {
                Console.Clear();    
            }
            if (tipka.Key == ConsoleKey.Escape)
            {
                DateTime vrijeme = DateTime.Now;
                string izlazIzPrograma = vrijeme.ToString() + " - Izlaz iz programa.";
                pracenjeRadaPrograma(izlazIzPrograma);
                return;
            }
        }

        private static void pretragaPoRedatelju(List<Film> listaFilmova)
        {
            /*
             * ---------------------------
             * Pretraživanje po redatelju!
             * ---------------------------
             */
            Console.Clear();
            Console.WriteLine();
            Console.Write("Unesite redatelja filma: ");
            string redatelji = Console.ReadLine();
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("----Popis filmova----");
            string formatString = "{0,1}{1,20}{2,1}{3,20}{4,1}{5,20}{6,1}{7,20}{8,1}{9,20}{10,1}{11,20}{12,1}";
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------");
            string result = String.Format(formatString, "|",
                              "Redni broj", "|", "Sifra", "|", "Naziv", "|", "Zanr", "|", "Redatelj", "|", "Glumci", "|");
            Console.WriteLine(result);
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------");
            foreach (Film trenutniFilm in listaFilmova)  
            {
                if (redatelji == trenutniFilm.redatelj)
                {
                    string sifra = trenutniFilm.sifra;
                    string zanr = trenutniFilm.zanrFilm;
                    string naziv = trenutniFilm.nazivFilma;
                    string redatelj = trenutniFilm.redatelj;
                    string glumci = trenutniFilm.glumci;
                    if (trenutniFilm.nazivFilma.Length > 19)
                    {
                        naziv = trenutniFilm.nazivFilma.Substring(0, 16) + "...";
                    }
                    if (trenutniFilm.glumci.Length > 19)
                    {
                        glumci = trenutniFilm.glumci.Substring(0, 16) + "...";
                    }
                    if (trenutniFilm.redatelj.Length > 19)
                    {
                        redatelj = trenutniFilm.redatelj.Substring(0, 16) + "...";
                    }
                    if (trenutniFilm.zanrFilm.Length > 19)
                    {
                        zanr = trenutniFilm.zanrFilm.Substring(0, 16) + "...";
                    }
                    List<string> listaGlumci = trenutniFilm.glumci.Split(',').ToList();
                    int brojac = 0;
                    foreach (string kakocemisezvatvarijabla in listaGlumci)
                    {
                        if (brojac == 0)
                        {
                            result = String.Format(formatString, "|",
                                   trenutniFilm.redniBroj, "|", sifra, "|", naziv, "|", zanr, "|",
                                   redatelj, "|", kakocemisezvatvarijabla, "|");
                        }
                        else

                        {
                            result = String.Format(formatString, "|",
                                                         "", "|", "", "|", "", "|", "", "|",
                                                          "", "|", kakocemisezvatvarijabla, "|");
                        }
                        Console.WriteLine(result);
                        Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------");
                        brojac++;
                    }
                }
                
            }
            Console.Write("Pritisnite ENTER za povratak u glavni izbornik ili ESC za izlaz iz programa");
            var tipka = Console.ReadKey();
            if (tipka.Key == ConsoleKey.Enter)
            {
                Console.Clear();

            }
            if (tipka.Key == ConsoleKey.Escape)
            {
                DateTime vrijeme = DateTime.Now;
                string izlazIzPrograma = vrijeme.ToString() + " - Izlaz iz programa.";
                pracenjeRadaPrograma(izlazIzPrograma);
                return;
            }
        }

        private static void pretragaPoZanru(List<Film> listaFilmova)
        {
            /*
             * -----------------------
             * Pretraživanje po zanru!
             * -----------------------
             */
            Console.Clear();
            Console.WriteLine();
            Console.Write("Unesite zanr filma: ");
            string zanrFilma = Console.ReadLine();
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("----Popis filmova----");
            string formatString = "{0,1}{1,20}{2,1}{3,20}{4,1}{5,20}{6,1}{7,20}{8,1}{9,20}{10,1}{11,20}{12,1}";
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------");
            string result = String.Format(formatString, "|",
                              "Redni broj", "|", "Sifra", "|", "Naziv", "|", "Zanr", "|", "Redatelj", "|", "Glumci", "|");
            Console.WriteLine(result);
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------");
            foreach (Film trenutniFilm in listaFilmova)
            {
                if (zanrFilma == trenutniFilm.zanrFilm)
                {
                    string sifra = trenutniFilm.sifra;
                    string zanr = trenutniFilm.zanrFilm;
                    string naziv = trenutniFilm.nazivFilma;
                    string redatelj = trenutniFilm.redatelj;
                    string glumci = trenutniFilm.glumci;
                    if (trenutniFilm.nazivFilma.Length > 19)
                    {
                        naziv = trenutniFilm.nazivFilma.Substring(0, 16) + "...";
                    }
                    if (trenutniFilm.glumci.Length > 19)
                    {
                        glumci = trenutniFilm.glumci.Substring(0, 16) + "...";
                    }
                    if (trenutniFilm.redatelj.Length > 19)
                    {
                        redatelj = trenutniFilm.redatelj.Substring(0, 16) + "...";
                    }
                    if (trenutniFilm.zanrFilm.Length > 19)
                    {
                        zanr = trenutniFilm.zanrFilm.Substring(0, 16) + "...";
                    }
                    List<string> listaGlumci = trenutniFilm.glumci.Split(',').ToList();
                    int brojac = 0;
                    foreach (string kakocemisezvatvarijabla in listaGlumci)
                    {
                        if (brojac == 0)
                        {
                            result = String.Format(formatString, "|",
                                   trenutniFilm.redniBroj, "|", sifra, "|", naziv, "|", zanr, "|",
                                   redatelj, "|", kakocemisezvatvarijabla, "|");
                        }
                        else

                        {
                            result = String.Format(formatString, "|",
                                                         "", "|", "", "|", "", "|", "", "|",
                                                          "", "|", kakocemisezvatvarijabla, "|");
                        }
                        Console.WriteLine(result);
                        Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------");
                        brojac++;
                    }
                }
               
            }
            Console.Write("Pritisnite ENTER za povratak u glavni izbornik ili ESC za izlaz iz programa");
            var tipka = Console.ReadKey();
            if (tipka.Key == ConsoleKey.Enter)
            {
                Console.Clear();

            }
            if (tipka.Key == ConsoleKey.Escape)
            {
                
                DateTime vrijeme = DateTime.Now;
                string izlazIzPrograma = vrijeme.ToString() + " - Izlaz iz programa.";
                pracenjeRadaPrograma(izlazIzPrograma);
                return;
            }
        }

        private static void pretragaPoNazivu(List<Film> listaFilmova)
        {
            /*
             * ------------------------------
             * Pretraživanje po nazivu filma!
             * ------------------------------
             */
            Console.Clear();
            Console.WriteLine();
            Console.Write("Unesite naziv filma: ");
            string nazivFilma = Console.ReadLine();
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("----Popis filmova----");
            string formatString = "{0,1}{1,20}{2,1}{3,20}{4,1}{5,20}{6,1}{7,20}{8,1}{9,20}{10,1}{11,20}{12,1}";
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------");
            string result = String.Format(formatString, "|",
                              "Redni broj", "|", "Sifra", "|", "Naziv", "|", "Zanr", "|", "Redatelj", "|", "Glumci", "|");
            Console.WriteLine(result);
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------");
            foreach (Film trenutniFilm in listaFilmova)
            {
                if (nazivFilma == trenutniFilm.nazivFilma)
                {
                    string sifra = trenutniFilm.sifra;
                    string zanr = trenutniFilm.zanrFilm;
                    string naziv = trenutniFilm.nazivFilma;
                    string redatelj = trenutniFilm.redatelj;
                    string glumci = trenutniFilm.glumci;
                    if (trenutniFilm.nazivFilma.Length > 19)
                    {
                        naziv = trenutniFilm.nazivFilma.Substring(0, 16) + "...";
                    }
                    if (trenutniFilm.glumci.Length > 19)
                    {
                        glumci = trenutniFilm.glumci.Substring(0, 16) + "...";
                    }
                    if (trenutniFilm.redatelj.Length > 19)
                    {
                        redatelj = trenutniFilm.redatelj.Substring(0, 16) + "...";
                    }
                    if (trenutniFilm.zanrFilm.Length > 19)
                    {
                        zanr = trenutniFilm.zanrFilm.Substring(0, 16) + "...";
                    }
                    List<string> listaGlumci = trenutniFilm.glumci.Split(',').ToList();
                    int brojac = 0;
                    foreach (string kakocemisezvatvarijabla in listaGlumci)
                    {
                        if (brojac == 0)
                        {
                            result = String.Format(formatString, "|",
                                   trenutniFilm.redniBroj, "|", sifra, "|", naziv, "|", zanr, "|",
                                   redatelj, "|", kakocemisezvatvarijabla, "|");
                        }
                        else

                        {
                            result = String.Format(formatString, "|",
                                                         "", "|", "", "|", "", "|", "", "|",
                                                          "", "|", kakocemisezvatvarijabla, "|");
                        }
                        Console.WriteLine(result);
                        Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------");
                        brojac++;
                    }
                }
                
            }
            Console.Write("Pritisnite ENTER za povratak u glavni izbornik ili ESC za izlaz iz programa");
            var tipka = Console.ReadKey();
            if (tipka.Key == ConsoleKey.Enter)
            {
                Console.Clear();

            }
            if (tipka.Key == ConsoleKey.Escape)
            {
                
                DateTime vrijeme = DateTime.Now;
                string izlazIzPrograma = vrijeme.ToString() + " - Izlaz iz programa.";
                pracenjeRadaPrograma(izlazIzPrograma);
                return;
            }
        }

        private static void pretrazivanjePoSifri(List<Film> listaFilmova)
        {
            /*
             * -----------------------
             * Pretraživanje po sifri!
             * -----------------------
             */
            Console.Clear();
            Console.WriteLine();
            Console.Write("Unesite sifru: ");
            string sifre = Console.ReadLine();
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("----Popis filmova----");
            string formatString = "{0,1}{1,20}{2,1}{3,20}{4,1}{5,20}{6,1}{7,20}{8,1}{9,20}{10,1}{11,20}{12,1}";
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------");
            string result = String.Format(formatString, "|",
                              "Redni broj", "|", "Sifra", "|", "Naziv", "|", "Zanr", "|", "Redatelj", "|", "Glumci", "|");
            Console.WriteLine(result);
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------");
            foreach (Film trenutniFilm in listaFilmova)
            {
                if (sifre == trenutniFilm.sifra)
                {
                    string sifra = trenutniFilm.sifra;
                    string zanr = trenutniFilm.zanrFilm;
                    string naziv = trenutniFilm.nazivFilma;
                    string redatelj = trenutniFilm.redatelj;
                    string glumci = trenutniFilm.glumci;
                    if (trenutniFilm.nazivFilma.Length > 19)
                    {
                        naziv = trenutniFilm.nazivFilma.Substring(0, 16) + "...";
                    }
                    if (trenutniFilm.glumci.Length > 19)
                    {
                        glumci = trenutniFilm.glumci.Substring(0, 16) + "...";
                    }
                    if (trenutniFilm.redatelj.Length > 19)
                    {
                        redatelj = trenutniFilm.redatelj.Substring(0, 16) + "...";
                    }
                    if (trenutniFilm.zanrFilm.Length > 19)
                    {
                        zanr = trenutniFilm.zanrFilm.Substring(0, 16) + "...";
                    }
                    List<string> listaGlumci = trenutniFilm.glumci.Split(',').ToList();
                    int brojac = 0;
                    foreach (string kakocemisezvatvarijabla in listaGlumci)
                    {
                        if (brojac == 0)
                        {
                            result = String.Format(formatString, "|",
                                   trenutniFilm.redniBroj, "|", sifra, "|", naziv, "|", zanr, "|",
                                   redatelj, "|", kakocemisezvatvarijabla, "|");
                        }
                        else

                        {
                            result = String.Format(formatString, "|",
                                                         "", "|", "", "|", "", "|", "", "|",
                                                          "", "|", kakocemisezvatvarijabla, "|");
                        }
                        Console.WriteLine(result);
                        Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------");
                        brojac++;
                    }
                }
                
            }
            Console.Write("Pritisnite ENTER za povratak u glavni izbornik ili ESC za izlaz iz programa");
            var tipka = Console.ReadKey();
            if (tipka.Key == ConsoleKey.Enter)
            {
                Console.Clear();

            }
            if (tipka.Key == ConsoleKey.Escape)
            {
                
                DateTime vrijeme = DateTime.Now;
                string izlazIzPrograma = vrijeme.ToString() + " - Izlaz iz programa.";
                pracenjeRadaPrograma(izlazIzPrograma);
                return;
            }
        }

        private static void pretrazivanjePoRbr(List<Film> listaFilmova)
        {
            /*
             * ------------------------------------
             * Pretraživanje po rednom broju filma!
             * ------------------------------------
             */
            Console.Clear();
            Console.WriteLine();
            Console.Write("Unesite redni broj: ");
            string redniBroj = Console.ReadLine();
            Console.Clear();
            int redniBrojPretrazivanje;
            if (int.TryParse(redniBroj, out redniBrojPretrazivanje))
            {
                Console.WriteLine();
                Console.WriteLine("----Popis filmova----");
                string formatString = "{0,1}{1,20}{2,1}{3,20}{4,1}{5,20}{6,1}{7,20}{8,1}{9,20}{10,1}{11,20}{12,1}";
                Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------");
                string result = String.Format(formatString, "|",
                              "Redni broj", "|", "Sifra", "|", "Naziv", "|", "Zanr", "|", "Redatelj", "|", "Glumci", "|");
                Console.WriteLine(result);
                Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------");
                foreach (Film trenutniFilm in listaFilmova)
                {
                    if (redniBrojPretrazivanje == trenutniFilm.redniBroj)
                    {
                        string sifra = trenutniFilm.sifra;
                        string zanr = trenutniFilm.zanrFilm;
                        string naziv = trenutniFilm.nazivFilma;
                        string redatelj = trenutniFilm.redatelj;
                        string glumci = trenutniFilm.glumci;
                        if (trenutniFilm.nazivFilma.Length > 19)
                        {
                            naziv = trenutniFilm.nazivFilma.Substring(0, 16) + "...";
                        }
                        if (trenutniFilm.glumci.Length > 19)
                        {
                            glumci = trenutniFilm.glumci.Substring(0, 16) + "...";
                        }
                        if (trenutniFilm.redatelj.Length > 19)
                        {
                            redatelj = trenutniFilm.redatelj.Substring(0, 16) + "...";
                        }
                        if (trenutniFilm.zanrFilm.Length > 19)
                        {
                            zanr = trenutniFilm.zanrFilm.Substring(0, 16) + "...";
                        }
                        List<string> listaGlumci = trenutniFilm.glumci.Split(',').ToList();
                        int brojac = 0;
                        foreach (string kakocemisezvatvarijabla in listaGlumci)
                        {
                            if (brojac == 0)
                            {
                                result = String.Format(formatString, "|",
                                       trenutniFilm.redniBroj, "|", sifra, "|", naziv, "|", zanr, "|",
                                       redatelj, "|", kakocemisezvatvarijabla, "|");
                            }
                            else

                            {
                                result = String.Format(formatString, "|",
                                                             "", "|", "", "|", "", "|", "", "|",
                                                              "", "|", kakocemisezvatvarijabla, "|");
                            }
                            Console.WriteLine(result);
                            Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------");
                            brojac++;
                        }

                    }
                    
                }
                Console.Write("Pritisnite ENTER za povratak u glavni izbornik ili ESC za izlaz iz programa");
                var tipka = Console.ReadKey();
                if (tipka.Key == ConsoleKey.Enter)
                {
                    Console.Clear();

                }
                if (tipka.Key == ConsoleKey.Escape)
                {
                    
                    DateTime vrijeme = DateTime.Now;
                    string izlazIzPrograma = vrijeme.ToString() + " - Izlaz iz programa.";
                    pracenjeRadaPrograma(izlazIzPrograma);
                    return;
                }
            }
        }

        private static void prikaziFilmove(List<Film> listaFilmova)
        {
            /*
             * --------------------------------------------------
             * Popis filmova koji se nalaze upisani u datoteci
             * s pratečim elementima, ali u tabličnom obliku!
             * --------------------------------------------------
             */
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("----Popis filmova----");
            string formatString = "{0,1}{1,20}{2,1}{3,20}{4,1}{5,20}{6,1}{7,20}{8,1}{9,20}{10,1}{11,20}{12,1}";
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------");
            string result = String.Format(formatString,"|", 
                              "Redni broj", "|", "Sifra", "|", "Naziv", "|", "Zanr", "|", "Redatelj", "|", "Glumci", "|");
            Console.WriteLine(result);
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------");
            foreach (Film trenutniFilm in listaFilmova)
            {
                string sifra = trenutniFilm.sifra;
                string zanr= trenutniFilm.zanrFilm;
                string naziv = trenutniFilm.nazivFilma;
                string redatelj = trenutniFilm.redatelj;
                string glumci=trenutniFilm.glumci;
                if (trenutniFilm.nazivFilma.Length > 19) {
                    naziv = trenutniFilm.nazivFilma.Substring(0, 16) + "...";
                }
                if (trenutniFilm.glumci.Length > 19)
                {
                    glumci = trenutniFilm.glumci.Substring(0, 16) + "...";
                }
                if (trenutniFilm.redatelj.Length > 19)
                {
                    redatelj=trenutniFilm.redatelj.Substring(0, 16) + "...";
                }
                if (trenutniFilm.zanrFilm.Length > 19)
                {
                    zanr = trenutniFilm.zanrFilm.Substring(0, 16) + "...";
                }
                List<string> listaGlumci = trenutniFilm.glumci.Split(',').ToList();
                int brojac = 0;
                foreach (string kakocemisezvatvarijabla in listaGlumci)
                {
                    if (brojac == 0)
                    {
                        result = String.Format(formatString, "|",
                               trenutniFilm.redniBroj, "|", sifra, "|", naziv, "|", zanr, "|",
                               redatelj, "|", kakocemisezvatvarijabla, "|");
                    }
                    else
                    {
                        result = String.Format(formatString, "|",
                                                     "", "|", "", "|", "", "|", "", "|",
                                                      "", "|", kakocemisezvatvarijabla, "|");
                    }
                    Console.WriteLine(result);
                    Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------");
                    brojac++;
                }              
            }
            Console.Write("Pritisnite ENTER za povratak u glavni izbornik ili ESC za izlaz iz programa");
            var tipka = Console.ReadKey();
            if (tipka.Key == ConsoleKey.Enter)
            {
                Console.Clear();

            }
            if (tipka.Key == ConsoleKey.Escape)
            {
               
                DateTime vrijeme = DateTime.Now;
                string izlazIzPrograma = vrijeme.ToString() + " - Izlaz iz programa.";
                pracenjeRadaPrograma(izlazIzPrograma);
                return;
            }           
        }

        private static FileStream dajFileStreamCitanjeKorisnici()
        {

            return new FileStream("C:\\Users\\Kriščn Perkz\\Desktop\\VSMTI-PROJEKT\\VSMTI-VIDEOTEKA\\VSMTI-VIDEOTEKA\\config.xml", FileMode.Open, FileAccess.Read);
        }

        private static FileStream dajFileStreamPisanjeKorisnici()
        {

            return new FileStream("C:\\Users\\Kriščn Perkz\\Desktop\\VSMTI-PROJEKT\\VSMTI-VIDEOTEKA\\VSMTI-VIDEOTEKA\\config.xml", FileMode.Create, FileAccess.Write);
        }

        private static bool dodajKorisnika(List<Korisnik> listaKorisnika, XmlSerializer xs)
        {
            /*
             * ------------------------------------------------------
             * Za dodavanje novog korisnika preko konzole u datoteku!
             * ------------------------------------------------------
             */
            Console.Clear();
            Console.WriteLine();
            Korisnik noviKorisnik = new Korisnik();
            Console.Write("Unesite ime: ");
            noviKorisnik.ime = Console.ReadLine();
            Console.Write("Unesite prezime: ");
            noviKorisnik.prezime = Console.ReadLine();
            Console.Write("Unesite korisnicko ime: ");
            noviKorisnik.korisnickoIme = Console.ReadLine();
            Console.Write("Unesite lozinku: ");
            noviKorisnik.lozinka = Console.ReadLine();
            bool provjeraKorisnika = provjeriPostojanjeKorisnika(listaKorisnika, noviKorisnik.korisnickoIme);

            if (!provjeraKorisnika) {
                listaKorisnika.Add(noviKorisnik);
                FileStream fs = dajFileStreamPisanjeKorisnici();
                xs.Serialize(fs, listaKorisnika);
                fs.Close();
                DateTime vrijeme = DateTime.Now;
                string bdodavanje = vrijeme.ToString() + " - Dodan korisnik " + noviKorisnik.korisnickoIme + ".";
                pracenjeRadaPrograma(bdodavanje);
            }

            return !provjeraKorisnika;
            
        }

        private static bool provjeriPostojanjeKorisnika(List<Korisnik> listaKorisnika, string korisnickoIme)
        {
            /*
             * ------------------------------------------
             * Provjera dali korisnik postoji u datoteci!
             * ------------------------------------------
             */
            foreach (Korisnik k in listaKorisnika){
                if (k.korisnickoIme == korisnickoIme) {
                    return true;
                }
            }
            return false;
        }
    }
}
