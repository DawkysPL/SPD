using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPD1
{
    class Algorytmy
    { 
        public static int ObliczSilnie(int liczbaZadan)
        {
            if (liczbaZadan == 1)
            {
                return 1;
            }
            else
            {
                return liczbaZadan * ObliczSilnie(liczbaZadan - 1);
            }
        }
        /*1. Poruszamy się od prawej strony do lewej, tak długo aż napotkamy element
   mniejszy od swojego prawego sąsiada. W tym przypadku elementem tym będzie
   5 o ideksie i=5 (liczonym od zera);

2. Poruszając się ponownie od prawej strony szukamy pierwszego elementu
   większego niż ten znaleziony na pozycji "i", czyli 5. Elementem tym będzie
   7 o indeksie j=7.

3. Zamieniamy miejscami elementy o indeksach "i" oraz "j". Otrzymamy szereg:
   1 4 6 2 9 7 8 5 3.

4. Odbijamy lustrzanie szereg elementów od indeksu i+1 do końca szeregu.
   Otrzymamy szereg: 1 4 6 2 9 7 3 5 8. Wyznaczony szereg jest szukanym
   przez nas rozwiązaniem. */
        public static void Permutacje(int liczbaZadan, List<Maszyna> Maszyny)
        {
            int[] tablicaPermutacji = new int[liczbaZadan];
            for (int i = 0; i < liczbaZadan; i++)
            {
                tablicaPermutacji[i] = i + 1;
            }

            for (int iterationIndex = 0; iterationIndex < (ObliczSilnie(liczbaZadan) - 1); iterationIndex++)
            {
                int i, j, k;

                if (liczbaZadan < 2)
                {
                    return;
                }

                i = liczbaZadan - 1;
                while ((i > 0) && (tablicaPermutacji[i - 1] >= tablicaPermutacji[i]))
                {
                    i--;
                }

                if (i > 0)
                {
                    j = liczbaZadan;
                    while ((j > 0) && (tablicaPermutacji[j - 1] <= tablicaPermutacji[i - 1]))
                    {
                        j--;
                    }
                }
                else
                {
                    j = -69;
                }

                if ((i > 0) && (j > 0))
                {
                    k = tablicaPermutacji[i - 1];
                    tablicaPermutacji[i - 1] = tablicaPermutacji[j - 1];
                    tablicaPermutacji[j - 1] = k;
                }

                for (i++, j = liczbaZadan; i < j; i++ , j--)
                {
                    k = tablicaPermutacji[i - 1];
                    tablicaPermutacji[i - 1] = tablicaPermutacji[j - 1];
                    tablicaPermutacji[j - 1] = k;
                }
            }
        }


    }
}
