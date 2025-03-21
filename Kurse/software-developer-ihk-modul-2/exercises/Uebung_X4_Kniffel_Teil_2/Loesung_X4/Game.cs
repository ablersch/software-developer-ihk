using System;
using System.Collections.Generic;
using System.Linq;

namespace Kniffel;

internal static class Game
{
    // aktuelle Punkte des Wurfes
    private static int[] points;

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

    // aktuell gewürfelt
    public static int[] Dice { get; set; } = new int[5];

    // Alle Spieler
    internal static List<Player> Players { get; set; } = new List<Player>();

    /// <summary>
    /// ALle möglichen Punkte eines wurfs berechnen
    /// </summary>
    public static void CalcPoints()
    {
        int[] diceCounts = new int[6];
        int sumOfAllDice = 0;
        int straight = 0;
        int maxStraight = 0;
        bool full = false;
        bool house = false;
        points = new int[Enum.GetValues(typeof(Scores)).Length];

        for (int i = 0; i < Dice.Length; ++i)
        {
            // Würfelwerte dem Array zuweisen und hochzählen zB Array Stelle 3 = 2  --> 2 4er
            diceCounts[Dice[i] - 1]++;
            sumOfAllDice += Dice[i];
        }

        // Die fünf "Würfel" durchlaufen
        for (int i = 0; i < diceCounts.Length; i++)
        {
            // Werte zusammenzählen
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
    }

    /// <summary>
    /// Würfeln indem Zufallszahlen erzeugt werden
    /// </summary>
    public static void DiceAllRandom()
    {
        Random random = new();
        for (int i = 0; i < Dice.Length; ++i)
        {
            Dice[i] = random.Next(1, 7); // Werte von einschließlich 1 bis einschließlich 6
        }
    }

    /// <summary>
    /// Zu einem Feld die Punkte abrufen
    /// </summary>
    /// <param name="field"></param>
    /// <returns></returns>
    public static int GetPoint(int field)
    {
        CalcPoints();
        return points[field];
    }

    /// <summary>
    /// Alternative
    /// </summary>
    /// <param name="dice"></param>
    /// <returns></returns>
    public static int[] GetPoints2(int[] dice)
    {
        int[] result = new int[Enum.GetValues(typeof(Scores)).Length];

        var getCount = new Func<int, int>((i) => dice.Count((d) => d == dice[i]));

        Array.Sort(dice);

        result[(int)Scores.Einser] = dice.Count((d) => d == 1) * 1;
        result[(int)Scores.Zweier] = dice.Count((d) => d == 2) * 2;
        result[(int)Scores.Dreier] = dice.Count((d) => d == 3) * 3;
        result[(int)Scores.Vierer] = dice.Count((d) => d == 4) * 4;
        result[(int)Scores.Fuenfer] = dice.Count((d) => d == 5) * 5;
        result[(int)Scores.Sechser] = dice.Count((d) => d == 6) * 6;
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

    public static void StartGame()
    {
        Console.WriteLine("Spiel wird gestartet.");

        // Durchlaufen bis alle Felder durchlaufen wurde -> alles ausgefüllt -> Spiel beendet
        for (int i = 0; i < Enum.GetValues(typeof(Scores)).Length; i++)
        {
            // Eine Runde
            foreach (var player in Players)
            {
                player.Round();
            }
        }
        ShowGameStats();
    }

    /// <summary>
    /// Am Ende Spielerübersicht mit Punkte anzeigen
    /// </summary>
    private static void ShowGameStats()
    {
        // Für Gewinnerberechnung
        Player tempPlayer = null;
        Console.WriteLine();

        foreach (Player player in Players)
        {
            Console.WriteLine($"Spieler {player.Name} hat {player.GetPlayerScore()} Punkte.");

            if (tempPlayer == null)
            {
                tempPlayer = player;
            }

            if (tempPlayer.GetPlayerScore() < player.GetPlayerScore())
            {
                tempPlayer = player;
            }
        }

        Console.WriteLine($"Spieler {tempPlayer.Name} hat gewonnen.");
    }
}