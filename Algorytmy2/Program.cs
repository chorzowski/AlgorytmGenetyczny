using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorytmy
{
    class Program
    {
        static void Main(string[] args)
        {
           
            int liczbaPopulacji = 40; // bedzie pewnie w tresci zadania, zwykle to 40

            int[,] tablicaZPliku = WczytajTabliceZPliku(@"D:\Users\Adrian\Desktop\liczby2.txt");// adres pliku

            int liczbaMiast = tablicaZPliku.GetLength(0); //liczba miast ktore ma odwiedzic nasz komiwojazer
  
            string[] ocenyTablic = new string[liczbaPopulacji]; // tutaj bedziemy trzymac oceny populacji czyli oceny dlugosci drogi danego osobnika

            int[,] nowaTablica = new int[liczbaPopulacji, liczbaMiast]; // tu tworzymy miejsce na nową populacje, bedzie to pierwsza populacja która wejdzie do pętli, taki adam i ewa, to od nich wszystko sie zaczenie, to ich w petli bedziemy selekcjonowac, krzyzowac i mutowac tak dlugo az bedzie kozak zadowolony

            string[] droga = new string[liczbaPopulacji]; // tutaj tworzymy miejsce na droge jaka przechodzi konkretny komiwojazer czyli osobnik

            Random r = new Random();
     
            

            nowaTablica = StworzTablice(liczbaPopulacji, liczbaMiast); // stwarzamy populacje poczatkowa

            nowaTablica = PomieszajTablice(nowaTablica); // mieszamy co by stworzona wyzej populacja roznila sie miedzy soba. To jest nasza pierwsza populacja ktora wchodzi do petli i bedziemy sie nia bawic

            droga = StworzOsobnika(nowaTablica); // zapisujemy drogi naszych jednostek

            ocenyTablic = OcenaTablic(liczbaPopulacji, tablicaZPliku, nowaTablica, droga); // oceniamy drogi czyli liczymy ile km przejdzie komiwojazer korzystajac z konkretnej drogii

            string temp = "";

            string[] wynikOstateczny = new string[1]; // tu bedzie wynik

                wynikOstateczny = wynik(liczbaMiast, liczbaPopulacji, ocenyTablic, tablicaZPliku); //tu zapisujemy wynik czyli wynik metody w ktorej wszystko sie dzieje
            temp = wynikOstateczny[0];

            temp = temp.Replace(' ', '-');// tu dodajemy te myslniki bo kozak chce tak wyglodajacy wynik
            Console.WriteLine(temp);
            
            Console.ReadLine();
        }

        public static string[] wynik(int liczbaMiast, int liczbaPopulacji, string[] pop1, int[,] tablicaZPliku)
        {
            string[] tablicaPoKrzyzowaniu = new string[pop1.Length];
            
            int[,] duzaTablica;
            string[] tablicaPoMutacji = new string[pop1.Length];
            string[] calkiemNowaTablica = new string[pop1.Length];
            string[] najlepszaTablica = new string[1];
            string[] mistrz = { "4000000" };
            string[] pierwsza = new string[pop1.Length];

            // to wyzej chyba nie wymaga komentarza

            for (int i = 0; i < 10000; i++)
            {
                string[] pop2 = Selekcja(pop1, liczbaPopulacji); // zaczynamy prawdziwa zabawe tutaj. Tutaj wpada nasza pierwsza populacja, tutaj będą iteracje np 10 tys, tutaj będziemy przetwarzac pop tak dlugo az bedzie kozak zadowolony. Pierwszym krokiem w tej petli jest selekcja. Czyli z dostarczonej populacji poczatkowej robimy turniej. losujemy 40 razy ( bo tyle chcemy miec osobnikow w populacji ) losowe jednostki, sprawdzamy ich dlugosc trasy i najlepszy z danej 3 wchodzi do nowej populacji
 
                tablicaPoKrzyzowaniu = Krzyzowanie(pop2);// normalnie sie krzyzuja mama i tata i z tego jest nowy czlowiek. U nas nie musi byc mamy i taty, jesteśmy nowocześni a poza tym nasze jednostki to obojniaki i same ze soba sie rozmnazaja. Ja zrobilem to w ten sposob ze biore losuje liczbe i od tej liczby do konca danego osobnika dokladnie odwrotnie ukladam jego trase. Mamy nowa trase ktora jest inna od poprzedniej a nadal mamy pewnosc ze zadne punkty sie nie powtarzaja bo zmieniamy tylko ich kolejnosc
                
                tablicaPoMutacji = Mutacja(tablicaPoKrzyzowaniu);// to juz prostsze bo losujemy dwa punkty w jednostce i wymieniamy je miejscami
              
                string[] pop;

                duzaTablica = zrobDuzaTablice(pop1.Length, liczbaMiast, tablicaPoMutacji); // to zjebana czesc stringow,aby powtorzyc caly proces potrzebujemy dwuwymiarowa tablice intow a my mamy stringi i w tym miejscu magicznie stringi zmieniaja sie w dwuwymiarowe inty by moc potworzyc caly proces
                                                                 
                pop = OcenaTablic(liczbaPopulacji, tablicaZPliku, duzaTablica, tablicaPoMutacji); // oceniamy nowa populacje ktora jest po selekcji, krzyzowaniu i mutacji
             
                pop1 = pop; // tutaj nasza nowa lepsza wersja populacji zamienia sie w pop1 ktora to trafi na początek naszej funkcji i wszystko wykona sie na nowo. dzieki temu po kazdej iteracji nasza populacja jest lepsza od poprzedniej
               
                najlepszaTablica = theBest(pop, mistrz); // tutaj sprawdzamy nasza najlepsza jednostke ktora ma najkrotsza trase
            
            }

            return najlepszaTablica;
    
        }

        public static int [,] WczytajTabliceZPliku(string path)
        {
            using (StreamReader sr = new StreamReader(path))
            {
                string liniaTekstuZPliku;
                string[] wczytanaTablicaZPliku;
                int wiersz = 0; //lub kolumna nie jestem teraz pewien
                
                liniaTekstuZPliku = sr.ReadLine(); // po to co by zniknąć pierwsza liczbe która oznacza liczbe wierszy.

                int[,] gotowaTablicaZPliku = new int[int.Parse(liniaTekstuZPliku), int.Parse(liniaTekstuZPliku)]; // tutaj tworze tablice która będzie gotową wczytaną tablicą z pliku, jej rozmiar to pierwsza linia w pliku

                while ((liniaTekstuZPliku = sr.ReadLine()) != null) //tutaj wczytuje tablice z odleglościami
                {

                    wczytanaTablicaZPliku = liniaTekstuZPliku.Trim().Split(' ');
                    for (int i = 0; i < wczytanaTablicaZPliku.Length; i++)
                    {
                        gotowaTablicaZPliku[wiersz, i] = int.Parse(wczytanaTablicaZPliku[i]);
                        gotowaTablicaZPliku[i, wiersz] = gotowaTablicaZPliku[wiersz, i];
                    }
                    wiersz++;
                }

                return gotowaTablicaZPliku;

            }
        }
        
        public static int [,] StworzTablice(int liczbaKolumn, int liczbaWierszy)
        {
            int[,] nowaTablica = new int[liczbaKolumn, liczbaWierszy];

            for (int i = 0; i < nowaTablica.GetLength(0); i++) // tutaj wypełniam ta nowa tablice 
            {
                for (int j = 0; j < nowaTablica.GetLength(1); j++)
                {
                    nowaTablica[i, j] = j;
                }
            }

            return nowaTablica;
        }

        public static int [,] PomieszajTablice(int [,] tablica)
        {
            Random r = new Random();
            for (int i = 0; i < tablica.GetLength(0); i++) // tutaj mieszam ta nowa tablice 
            {
                for (int j = 0; j < tablica.GetLength(1); j++)
                {
                    int randomowaPozycja = r.Next(0, 8);
                    int wartoscNaRandomowejPozycji = tablica[i, randomowaPozycja];
                    tablica[i, randomowaPozycja] = tablica[i, j];
                    tablica[i, j] = wartoscNaRandomowejPozycji;
                }
            }
            return tablica;
        }

        public static string[] StworzOsobnika(int[,] tablica)
        {
    
            string[] slownik2 = new string[tablica.GetLength(0)];

            string sumaDrogi = "";

            for (int i = 0; i < tablica.GetLength(0); i++)
            {
                for (int j = 0; j < tablica.GetLength(1); j++)
                {
                    sumaDrogi += tablica[i, j] + " ";
               
                }
                slownik2[i] = sumaDrogi;
                sumaDrogi = "";
            }

            return slownik2;
        }

        

        public static string[] OcenaTablic(int liczbaPopulacji, int[,] tablicaZPliku, int[,] nowaTablica, string[] indexDroga)
        {
            int pozycjaStartowa = 0;
            int pozycjaKoncowa = 0;
            int ocena = 0;
            string[] ocenyTablic3 = new string[liczbaPopulacji];


            for (int i = 0; i < liczbaPopulacji; i++)
            {
                for (int j = 0; j < tablicaZPliku.GetLength(1); j++)
                {
                    if (j + 1 < tablicaZPliku.GetLength(1))
                    {
                        pozycjaStartowa = nowaTablica[i, j];
                        pozycjaKoncowa = nowaTablica[i, j + 1];
                    }
                    else
                    {
                        pozycjaStartowa = nowaTablica[i, j];
                        pozycjaKoncowa = nowaTablica[i, 0];
                    }
                    ocena += tablicaZPliku[pozycjaStartowa, pozycjaKoncowa];
                }

                ocenyTablic3[i] = indexDroga[i];
          
                ocenyTablic3[i] = ocenyTablic3[i].Trim() + " " + ocena.ToString();   
                ocena = 0;
            }

            return ocenyTablic3;
        }

        public static string[] Selekcja(string[] tablica, int wielkosc)
        {
            int max = int.MaxValue;
            int licznik = 0;
            int losowyIndex = 0;
            int indexOsobnika = 0;
            int sumaDrogi = 0;


            int[] tymczasowaTablicaZOcenami = new int[wielkosc];
            int[] tymczasowaTablicaZIndexami = new int[wielkosc];
            string[] tymczasowaTablicaDroga = new string[wielkosc];

            string[] tymczasowaTablicaStringow = new string[wielkosc];

            Random r = new Random();
            for (int j = 0; j < wielkosc; j++)
            {
                for (int i = 0; i < 3; i++)
                {
                    losowyIndex = r.Next(0, 40); // tu musisz zmienic na uniwersalna wartosc
                    
                    tymczasowaTablicaStringow = tablica[losowyIndex].Split(' ');

                    sumaDrogi = int.Parse(tymczasowaTablicaStringow[tymczasowaTablicaStringow.Length - 1]);
                    tymczasowaTablicaStringow = tymczasowaTablicaStringow.Take(tymczasowaTablicaStringow.Count() - 1).ToArray();
                    string ciek = String.Join(" ", tymczasowaTablicaStringow);


                    if (max > sumaDrogi)
                    {
                        max = sumaDrogi;
                        indexOsobnika = losowyIndex;
                        tymczasowaTablicaDroga[j] = tablica[losowyIndex];
                        tymczasowaTablicaStringow = tablica[losowyIndex].Split(' ');
                        tymczasowaTablicaStringow = tymczasowaTablicaStringow.Take(tymczasowaTablicaStringow.Count() - 1).ToArray();

                        tymczasowaTablicaDroga[j] = String.Join(" ", tymczasowaTablicaStringow);

                    }
                }
                tymczasowaTablicaZOcenami[licznik] = max;
                tymczasowaTablicaZIndexami[licznik] = indexOsobnika;
              
                licznik++;

                max = int.MaxValue;
                indexOsobnika = 0;
            }

            return tymczasowaTablicaDroga;

        }

        

        public static string[] Krzyzowanie(string[] tablicaZIndeksamiPoSelekcji)
        {
            Random r = new Random();

            string droga2 = "";


            string[] w = new string[40];
            string[] nowySlownik2 = new string[tablicaZIndeksamiPoSelekcji.Length];

            for (int i = 0; i < tablicaZIndeksamiPoSelekcji.Length; i++)
            {
                if (r.Next(0, 100) < 50)
                {
                    string[] tymczasowa = new string[tablicaZIndeksamiPoSelekcji[0].Length];

                    string[] pierwszaPolowaSlowa = new string[tablicaZIndeksamiPoSelekcji[0].Length];

                    string[] drugaPolowaSlowa = new string[tablicaZIndeksamiPoSelekcji[0].Length];

                    string pierwszaPolowa = "";
                    string drugaPolowa = "";

                    tymczasowa = tablicaZIndeksamiPoSelekcji[i].Split(' ');

                    int polowa = r.Next(tymczasowa.Length - 1);

                    for (int ii = 0; ii <= polowa; ii++)
                    {

                        pierwszaPolowa += tymczasowa[ii] + " ";
                    }

                    for (int j = tymczasowa.Length - 1; j > polowa; j--)
                    {
                        drugaPolowa += tymczasowa[j] + " ";

                    }

                    nowySlownik2[i] = pierwszaPolowa.TrimStart() + drugaPolowa;
                    
                }
                else
                {                  
                    droga2 = tablicaZIndeksamiPoSelekcji[i];

                    nowySlownik2[i] = droga2;
                    
                }
            }

            return nowySlownik2;
        }

   
        public static string [] Mutacja(string [] indexyDroga2) 
        {
            Random r = new Random();
            string droga2 = "";
            string slowo3 = "";
            string slowo4 = "";
            string temp2 = "";
            string nowaDroga2 = "";
            string[] tablicaDrogi = new string[indexyDroga2.Length];
            string[] tablicaDrogi2 = new string[indexyDroga2.Length];


            int indexPierwszegoSlowa = 0;
            int indexDrugiegoSlowa = 0;

            string[] nowySlownik2 = new string[indexyDroga2.Length];


            for (int i = 0; i < indexyDroga2.Length; i++)
            {
                if (r.Next(0, 100) < 90)
                {
                    droga2 = indexyDroga2[i];
                    
                    tablicaDrogi2 = droga2.Split(' ');


                    indexPierwszegoSlowa = r.Next(0, 10);//trzeba zmienic na uniwersalne wartosci, albo i nie...
                    indexDrugiegoSlowa = r.Next(0, 10);
                   
                    slowo3 = tablicaDrogi2[indexPierwszegoSlowa];
                    slowo4 = tablicaDrogi2[indexDrugiegoSlowa];

                    temp2 = slowo3;
                    
                    tablicaDrogi2[indexPierwszegoSlowa] = slowo4;
                    
                    tablicaDrogi2[indexDrugiegoSlowa] = temp2;



                    nowaDroga2 = string.Join(" ", tablicaDrogi2);
         
                    nowySlownik2[i] = nowaDroga2;                  

                }
                else
                {
                    nowaDroga2 = indexyDroga2[i];
                    nowySlownik2[i] = nowaDroga2;

                }
            }

            return nowySlownik2;
        }
        public static int[,] zrobDuzaTablice(int liczbaPopulacji, int liczbaMiast, string[] tabllica)
        {
            int[,] nowaTablica = new int[liczbaPopulacji, liczbaMiast];
            string[] t = new string[liczbaMiast];
            int liczba = 0;

            for (int i = 0; i < nowaTablica.GetLength(0); i++)  
            {
                t = tabllica[i].Split(' ');
           
                for (int j = 0; j < nowaTablica.GetLength(1); j++)
                {
                    int.TryParse(t[j],out liczba);
                    nowaTablica[i, j] = liczba;
                }
            }
            return nowaTablica;
        }
        public static string[] theBest(string[] tablicaDoOceny, string[] mistrz)
        {
            int max = int.MaxValue;
          
            string[] zwyciezca = new string[1];

            int sumaDrogi = 0;
            int liczbaMistrza = 0;
            string stringPotrzebnyDoSumyDrogiMistrza = mistrz[0];

            liczbaMistrza = stringPotrzebnyDoSumyDrogiMistrza.Split(' ').Sum(x => int.Parse(x));
      
            for (int i = 0; i < tablicaDoOceny.Length; i++)
            {
                string stringPotrzebnyDoSumyDrogi = tablicaDoOceny[i];
        
                sumaDrogi += stringPotrzebnyDoSumyDrogi.Split(' ').Sum(x => int.Parse(x));
        
                if (max > sumaDrogi)
                {
                    zwyciezca[0] = tablicaDoOceny[i];
                    if(sumaDrogi > liczbaMistrza)
                    {
                        zwyciezca[0] = mistrz[0];
                    }
                    
                    max = sumaDrogi;
                }
                sumaDrogi = 0;
            }
            
            return zwyciezca;
          
        }
    }
}
