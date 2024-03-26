using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProgramiranjeProjekat.Program;

namespace ProgramiranjeProjekat
{
    internal class Program
    {
        public class Tacka
        {
            public double x, y;
            public Tacka() { }
            public Tacka(double x, double y) 
            { 
                this.x = x;
                this.y = y;
            }

            public double R()
            {
                return Math.Sqrt(x * x + y * y);
            }
            public double ugao()
            {
                return Math.Atan2(y, x) * 180 / Math.PI;
            }

            public static bool jednakeTacke(Tacka A, Tacka B)
            {
                if ((A.x == B.x) && (A.y == B.y)) return true;
                else return false;
            }
        }

        public class Vektor
        {
            public Tacka pocetak;
            public Tacka kraj;
            public Vektor() { }
            public Vektor(Tacka pocetak, Tacka kraj)
            {
                this.pocetak = pocetak;
                this.kraj = kraj;
            }

            public Tacka centriraj()
            {
                return new Tacka(kraj.x - pocetak.x, kraj.y - pocetak.y);
            }

            public double duzina()
            {
                return centriraj().R();
            }

            public static double skalarni(Vektor a, Vektor b)
            {
                Tacka prva = a.centriraj();
                Tacka druga = b.centriraj();
                double SP = prva.x*druga.x + prva.y * druga.y;
                return SP;
            }

            public static double VP(Vektor a, Vektor b)
            {
                Tacka A = a.centriraj();
                Tacka B = b.centriraj();
                double k = A.x*B.y - A.y*B.x;
                return k;
            }

            static public int SIS(Vektor a, Tacka C, Tacka D)
            {
                Tacka A = a.pocetak;
                Tacka B = a.kraj;
                Vektor AB = new Vektor(A,B);
                Vektor AC = new Vektor(A, C);
                Vektor AD = new Vektor(A, D);
                double k1 = VP(AB, AC);
                double k2 = VP(AB, AD);
                if (k1 * k2 < 0) return 2;
                if (k1 * k2 > 0) return 0;
                return 1;
            }

            static public bool presek(Vektor a, Vektor b)
            {
                if (SIS(a, b.pocetak, b.kraj) * SIS(b, a.pocetak, a.kraj) > 0) return true;
                return false;
            }

            static public double uao(Vektor a, Vektor b)
            {
                Tacka A = a.centriraj();
                Tacka B = b.centriraj();
                double ugao = B.ugao() - A.ugao();
                return ugao;
            }
        }

        public class Poligon
        {
            public int brTemena;
            public Tacka[] Temena;
            public Poligon(int n) 
            { 
                Temena = new Tacka[n];
                brTemena = n;
            }

            public Vektor[] Vektori;

            public static Poligon unesi()
            {
                Console.WriteLine("Unesite broj temena");
                int n = Convert.ToInt32(Console.ReadLine());
                Poligon poligon = new Poligon(n);
                for (int i = 0; i < n; i++)
                {
                    Console.Write("Za teme " + (i+1) + " x =");
                    double x = Convert.ToDouble(Console.ReadLine());
                    Console.Write("Za teme " + (i+1) + " y =");
                    double y = Convert.ToDouble(Console.ReadLine());
                    poligon.Temena[i] = new Tacka(x, y);
                }
                return poligon;
            }

            public void save()
            {
                StreamWriter izlaz = new StreamWriter("poligon.txt");
                izlaz.WriteLine(brTemena);
                for (int i = 0; i < brTemena; i++)
                {
                    izlaz.WriteLine(Temena[i].x);
                    izlaz.WriteLine(Temena[i].y);
                }
                izlaz.Close();
            }

            public static Poligon load()
            {
                StreamReader ulaz = new StreamReader("poligon.txt");
                int n = Convert.ToInt32(ulaz.ReadLine());
                Poligon poligon = new Poligon(n);
                for (int i = 0; i < n; i++)
                {
                    double _x = Convert.ToDouble(ulaz.ReadLine());
                    double _y = Convert.ToDouble(ulaz.ReadLine());
                    poligon.Temena[i] = new Tacka(_x, _y);
                }
                ulaz.Close();
                return poligon;
            }


            public bool prost()
            {
                for (int i = 0; i < brTemena - 2; i++)
                {
                    Vektor a = new Vektor(Temena[i], Temena[i + 1]);

                    for (int j = i + 2; j < brTemena; j++)
                    {
                        if (i == 0 && j == brTemena - 1) continue;
                        Vektor b = new Vektor(Temena[j], Temena[(j + 1) % brTemena]);
                        if (Vektor.presek(a, b) == true) return false;
                    }
                }
                return true;
            }

            public double obim()
            {
                double obim = 0;

                for(int i = 0; i< brTemena; i++)
                {
                    if(i == brTemena - 1)
                    {
                        Vektor a = new Vektor(Temena[i], Temena[0]);
                        obim = obim + a.duzina();
                    }
                    else
                    {
                        Vektor a = new Vektor(Temena[i], Temena[i+1]);
                        obim = obim + a.duzina();
                    }
                }

                return obim;
            }

            public double povrsina()
            {
                double B1 = 0;
                double B2 = 0;

                for(int i = 0; i < brTemena; i++)
                {
                    if(i == brTemena-1)
                    {
                        B1 += Temena[i].x * Temena[0].y;
                        B2 += Temena[i].y * Temena[0].x;
                    }
                    else
                    {
                        B1 += Temena[i].x * Temena[i+1].y;
                        B2 += Temena[i].y * Temena[i+1].x;
                    }
                }

                double P = (B1 - B2) / 2;
                return P;
            }

            public void unosVektora()
            {
                
            }

            public bool konveksan()
            {
                /*
                double vp = 0;
                double trenutni;

                for(int i = 0; i< brTemena; i++)
                {
                    if (i == brTemena - 1)
                    {
                        Vektori[i] = new Vektor(Temena[i], Temena[0]);
                    }
                    else
                    {
                        Vektori[i] = new Vektor(Temena[i], Temena[i + 1]);
                    }
                }

                for (int i = 0; i < brTemena; i++)
                {
                    if(i == 0)
                    {
                        vp = Vektor.VP(Vektori[i], Vektori[i + 1]);
                        continue;
                    }
                    
                    if(i == brTemena-1)
                    {
                        trenutni = Vektor.VP(Vektori[i], Vektori[0]);
                        if (vp * trenutni < 0)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        trenutni = Vektor.VP(Vektori[i], Vektori[i + 1]);
                        if(vp * trenutni < 0) 
                        { 
                            return false; 
                        }
                    }
                } 

                return true;
                */

                int brojac = 0;
                int treba = brTemena;
                for (int i = 0; i < brTemena; i++)
                {
                    Vektor a = new Vektor(Temena[i], Temena[(i + 1) % brTemena]);
                    Vektor b = new Vektor(Temena[(i + 1) % brTemena], Temena[(i + 2) % brTemena]);
                    double ugao = Vektor.VP(a, b);
                    if (ugao > 0) brojac++;
                    if (ugao == 0) treba--;
                }
                if ((brojac == treba) || (brojac == 0)) return true;
                else return false;

            }

            public static bool izmedju(Tacka A, Tacka B, Tacka T)
            {
                double AB = Math.Abs(A.x - B.x);
                double AT = Math.Abs(A.x - T.x);
                double BT = Math.Abs(T.x - B.x);
                if (AT + BT == AB) return true;
                return false;
            }

            public static bool ST(Vektor AB, Tacka T)
            {
                if ((T.y == AB.pocetak.y) && (izmedju(AB.pocetak, AB.kraj, T))) return true;
                return false;
            }

            public bool tackaUnutar()
            {
                if (prost() == false)
                {
                    return false;
                }

                Console.WriteLine("Unesite Tacku");
                Console.Write("x = ");
                double x = Convert.ToDouble(Console.ReadLine());
                Console.Write("y = ");
                double y = Convert.ToDouble(Console.ReadLine());

                double max = Temena[0].x;
                for (int i = 1; i < brTemena; i++)
                {
                    if (max < Temena[i].x) max = Temena[i].x;
                }

                Tacka leva = new Tacka(x, y);
                Tacka desna = new Tacka(max + 1, y);

                Vektor poluprava = new Vektor(leva, desna);

                int presek = 0;

                for (int i = 0; i < brTemena; i++)
                {
                    Vektor stranica = new Vektor(Temena[i], Temena[(i + 1) % brTemena]);

                    if (ST(poluprava, Temena[(i + 1) % brTemena]))
                    {
                        if (Vektor.SIS(poluprava, Temena[i], Temena[(i + 2) % brTemena]) == 1) presek++;
                    }
                    if (Vektor.presek(poluprava, stranica)) presek++;
                }

                if ((presek % 2) == 0) return false;
                return true;
            }

        }

        
        static void Main(string[] args)
        {
            Poligon poligon = null;
            int opcija = 1;

            Console.Write("1.Unos poligona  ");
            Console.Write("2.Sacuvaj poligon  ");
            Console.Write("3.Ucitaj poligon  ");
            Console.Write("4.Da li je prost  ");
            Console.Write("5.Obim  ");
            Console.Write("6.Povrsina  ");
            Console.Write("7.Konveksan  ");
            Console.Write("8.Tacka Unutar poligona  ");
            Console.WriteLine("9.Kraj");

            while (opcija != 0)
            {
                Console.Write("Unesite opciju: ");

                opcija = Convert.ToInt32(Console.ReadLine());

                if(opcija == 1)
                {
                    poligon = Poligon.unesi();
                }
                else if(opcija == 2)
                {
                    poligon.save();
                }
                else if(opcija == 3)
                {
                    poligon = Poligon.load();
                }
                else if(opcija == 4)
                {
                    if(poligon.prost() == true)
                    {
                        Console.WriteLine("Poligon je prost");
                    }
                    else
                    {
                        Console.WriteLine("Poligon nije prost");
                    }
                }
                else if(opcija == 5)
                {
                    double obim = poligon.obim();
                    Console.WriteLine("Obim je " +  obim);
                }
                else if(opcija == 6)
                {
                    double povrsina = poligon.povrsina();
                    if (povrsina < 0)
                    {
                        povrsina = povrsina * -1;
                    }
                    Console.WriteLine("Povrsina je " + povrsina);
                }
                else if(opcija == 7)
                {
                    if(poligon.konveksan() == true)
                    {
                        Console.WriteLine("Poligon je konveksan");
                    }
                    else
                    {
                        Console.WriteLine("Poligon nije konveksan");
                    }
                }
                else if(opcija == 8)
                {
                    if(poligon.tackaUnutar() == true)
                    {
                        Console.WriteLine("Tacka je unutar poligona");
                    }
                    else
                    {
                        Console.WriteLine("Tacka je izvan poligona");
                    }
                }
                else if(opcija == 9)
                {
                    opcija = 0;
                }
            }
        }
    }
}
