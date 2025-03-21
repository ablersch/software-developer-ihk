using System;
using System.Linq;

namespace Kniffel;

internal class Program
{
    // https://de.wikipedia.org/wiki/Kniffel

    public enum Scores
    {
        Einser,          // Summe der Einser
        Zweier,          // Summe der Zweier
        Dreier,          // Summe der Dreier
        Vierer,          // Summe der Vierer
        Fuenfer,         // Summe der Fuenfer
        Sechser,         // Summe der Sechser
        Dreierpasch,     // Summe aller Würfel
        Viererpasch,     // Summe aller Würfel
        FullHouse,       // 25 (drei gleiche und zwei gleiche oder fünf gleiche)
        KleineStrasse,   // 30 (vier aufeinanderfolgende Augenzahlen)
        GrosseStrasse,   // 40 (fünf aufeinanderfolgende Augenzahlen)
        Kniffel,         // 50 (fünf gleiche Würfel)
        Chance           // Summe aller Würfel
    }

    /// <summary>
    /// Alternative
    /// </summary>
    /// <param name="dice"></param>
    /// <returns></returns>
    public static int[] GetPoints2(int[] dice)
    {
        int[] result = new int[Enum.GetValues(typeof(Scores)).Length];

        var getCount = new Func<int, int>((i) => dice.Where((d) => d == dice[i]).Count());

        Array.Sort(dice);

        result[(int)Scores.Einser] = dice.Where((d) => d == 1).Count() * 1;
        result[(int)Scores.Zweier] = dice.Where((d) => d == 2).Count() * 2;
        result[(int)Scores.Dreier] = dice.Where((d) => d == 3).Count() * 3;
        result[(int)Scores.Vierer] = dice.Where((d) => d == 4).Count() * 4;
        result[(int)Scores.Fuenfer] = dice.Where((d) => d == 5).Count() * 5;
        result[(int)Scores.Sechser] = dice.Where((d) => d == 6).Count() * 6;
        result[(int)Scores.Dreierpasch] = getCount(0) >= 3 || getCount(2) >= 3 || getCount(4) >= 3 ? dice.Sum((d) => d + 1) : 0;
        result[(int)Scores.Viererpasch] = getCount(0) >= 4 || getCount(4) >= 4 ? dice.Sum((d) => d + 1) : 0;
        result[(int)Scores.FullHouse] = result[(int)Scores.Dreierpasch] > 0 ? (getCount(0) == 2 || getCount(4) == 2 ? 25 : 0) : 0;

        bool largeStraight = true;
        var num = dice[0];

        for (int i = 0; i < 5; i++)
        {
            largeStraight = largeStraight && (dice[i] == i + num);
        }

        result[(int)Scores.GrosseStrasse] = largeStraight ? 40 : 0;

        bool smallStraight = false;
        var arr = dice.Distinct().ToArray();

        if (arr.Length >= 4)
        {
            var slice = arr.Take(4).ToArray();

            num = slice[0];
            smallStraight = true;

            for (int i = 0; i < 4; i++)
            {
                smallStraight = smallStraight && (slice[i] == i + num);
            }

            if (!smallStraight && arr.Length == 5)
            {
                slice = arr.Skip(1).ToArray();
                smallStraight = true;
                num = slice[0];

                for (int i = 0; i < 4; i++)
                {
                    smallStraight = smallStraight && (slice[i] == i + num);
                }
            }
        }

        result[(int)Scores.KleineStrasse] = smallStraight ? 30 : 0;
        result[(int)Scores.Kniffel] = getCount(0) == 5 ? 50 : 0;
        result[(int)Scores.Chance] = dice.Sum((d) => d + 1);

        return result;
    }

    // Liefert die Punkte welche gewürfelt wurden
    // Dice beinhaltet alle gewürfelten Zahlen
    public static int[] GetPunkte(int[] dice)
    {
        // Liste der möglichen Ergebnisse
        int[] points = new int[Enum.GetValues(typeof(Scores)).Length];
        // Zahlen Würfe erfassen
        int[] diceCounts = new int[6];
        // Summe des gesamten Wurfes
        int sumOfAllDice = 0;
        int straight = 0;
        int maxStraight = 0;
        bool full = false;
        bool house = false;

        for (int i = 0; i < dice.Length; ++i)
        {
            // Würfelwerte dem Array zuweisen und hochzählen zB Array Stelle 3 (4er) = 2  --> 2 4er
            // Was wurde gewürfelt, wieviel 1er, wieveil 2er, ...
            diceCounts[dice[i] - 1]++;
            // Summe des gesamten Wurfs (Chance)
            sumOfAllDice += dice[i];
        }

        // Die fünf "Würfel" durchlaufen
        for (int i = 0; i < diceCounts.Length; i++)
        {
            // Werte zusammenzählen für Punkte (Zahlen 1-6)
            points[i] = diceCounts[i] * (i + 1);

            // Bei jeder vorkommenden Würfelzahl hochzählen
            if (diceCounts[i] > 0)
            {
                straight++;
            }
            else
            {
                // Den aktuellen Wert speichern und den Counter wieder auf 0 setzen
                if (straight > maxStraight)
                {
                    maxStraight = straight;
                }
                straight = 0;
            }

            if (diceCounts[i] == 2)
            {
                full = true;
            }

            if (diceCounts[i] >= 3)
            {
                points[(int)Scores.Dreierpasch] = sumOfAllDice;
                house = true;
            }

            if (diceCounts[i] >= 4)
            {
                points[(int)Scores.Viererpasch] = sumOfAllDice;
            }

            if (diceCounts[i] >= 5)
            {
                points[(int)Scores.Kniffel] = 50;
                points[(int)Scores.FullHouse] = 25;
            }
        }

        if (full && house)
        {
            points[(int)Scores.FullHouse] = 25;
        }

        if (straight > maxStraight)
        {
            maxStraight = straight;
        }

        if (maxStraight >= 4)
        {
            points[(int)Scores.KleineStrasse] = 30;
        }

        if (maxStraight >= 5)
        {
            points[(int)Scores.GrosseStrasse] = 40;
        }

        points[(int)Scores.Chance] = sumOfAllDice;

        return points;
    }

    private static void Main(string[] args)
    {
        string input = string.Empty;
        int[] gewuerfelteWerte = new int[5];
        bool inputError;

        while (input != "q")
        {
            inputError = false;

            Console.WriteLine("Fünf Zahlen von 1-6 eingeben");
            Console.WriteLine("'z' für Zufallszahlen");
            Console.WriteLine("'q' zum beenden");
            input = Console.ReadLine();
            Console.Clear();

            // Zufallszahlen
            if (input == "z")
            {
                Random random = new Random();
                //5x würfeln
                for (int i = 0; i < gewuerfelteWerte.Length; ++i)
                {
                    gewuerfelteWerte[i] = random.Next(1, 7); // Werte von einschließlich 1 bis einschließlich 6
                }
            }
            // manuelle Eingabe
            else if (input.Length == 5 && Int32.TryParse(input, out int temp))
            {
                // TODO Prüfen ob Zahlen im Zahlenbereich 1-6 liegen

                for (int i = 0; i < input.Length; i++)
                {
                    // String aufteilen und von den chars den numerischen Wert speichern
                    gewuerfelteWerte[i] = (int)Char.GetNumericValue(input[i]);
                }
            }
            else
            {
                Console.WriteLine("Falsche oder nicht genügend Zahlen eingegeben!");
                inputError = true;
            }

            if (!inputError)
            {
                input = string.Empty;

                foreach (var item in gewuerfelteWerte)
                {
                    input = input + " " + item;
                }

                Console.WriteLine("Wuerfelwerte: " + input);

                int[] punkte = GetPunkte(gewuerfelteWerte);

                Console.WriteLine("\nPunktzahlen:");
                foreach (Scores score in Enum.GetValues(typeof(Scores)))
                {
                    Console.WriteLine("{0,15} {1}", score, punkte[(int)score]);
                }
                Console.WriteLine("");
            }
        }
    }
}