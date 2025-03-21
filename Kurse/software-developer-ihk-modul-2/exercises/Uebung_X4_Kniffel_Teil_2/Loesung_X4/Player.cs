using System;
using System.Collections.Generic;
using System.Threading;
using static Kniffel.Game;

namespace Kniffel;

internal class Player
{
    private Dictionary<Game.Scores, int?> playerScore = new();

    /// <summary>
    /// Konstruktor
    /// </summary>
    /// <param name="name">Name des Spielers</param>
    public Player(string name)
    {
        Name = name;
        Game.Players.Add(this);
        InitPlayerScoreDic();
    }

    public string Name { get; set; }

    /// <summary>
    /// Punkte des Spielers abrufen
    /// </summary>
    /// <returns></returns>
    public int GetPlayerScore()
    {
        int result = 0;
        foreach (var item in playerScore)
        {
            // notwenig da int?
            result += item.Value.GetValueOrDefault();
        }

        return result;
    }

    /// <summary>
    /// Abhandeln der Runde des Spielers
    /// </summary>
    public void Round()
    {
        Console.WriteLine($"Spieler {Name} ist an der Reihe. Bitte würfeln...");
        Console.ReadLine();
        Thread.Sleep(1000);
        Game.DiceAllRandom();
        Console.Write("... es wurde folgendes gewürfelt: ");

        for (int i = 0; i < Game.Dice.Length; i++)
        {
            Console.Write(Game.Dice[i] + " ");
        }

        Console.WriteLine();
        PrintFields();

        while (!IsUserInputValid()) { }
    }

    /// <summary>
    /// Werte vorbelegen damit die Reihenfolge der Einträge passt und es eine Übersicht gibt was bereits belegt und was noch frei ist.
    /// </summary>
    private void InitPlayerScoreDic()
    {
        foreach (Game.Scores field in Enum.GetValues(typeof(Game.Scores)))
        {
            playerScore.Add(field, null);
        }
    }

    /// <summary>
    /// Prüfen ob das Feld bereits gesetzt bzw belegt ist
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    private bool IsFieldFree(Game.Scores key)
    {
        return !playerScore[key].HasValue;
    }

    /// <summary>
    /// Prüfen ob:
    /// Eingabe eine Zahl ist
    /// Eingabe in Enmu umgewandelt werden kann
    /// Gewähltes Feld noch frei ist um darin die Punkte zu speichern
    /// </summary>
    /// <returns></returns>
    private bool IsUserInputValid()
    {
        if (!int.TryParse(Console.ReadLine(), out int fieldChoose))
        {
            Console.WriteLine("Eingabe ungültig. Nochmals eingeben");
            return false;
        }

        fieldChoose--;

        // Enum Name in Enum Typ konvertieren
        if (!Enum.TryParse<Scores>(Enum.GetName(typeof(Scores), fieldChoose), out Scores tempScores))
        {
            Console.WriteLine("Eingabe ungültig. Nochmals eingeben");
            return false;
        }

        if (!IsFieldFree(tempScores))
        {
            Console.WriteLine("Feld ist bereits mit Punkten belegt");
            return false;
        }

        int fieldPoints = Game.GetPoint(fieldChoose);

        Console.WriteLine($"In das Feld {tempScores} wurden {fieldPoints} Punkte eingetragen.");

        playerScore[tempScores] = fieldPoints;

        return true;
    }

    /// <summary>
    /// Die alle Felder mit Punkte ausgeben
    /// </summary>
    private void PrintFields()
    {
        if (playerScore.Count != 0)
        {
            Console.WriteLine("\nBitte Feld wählen in welches die Punkte eingetragen werden soll:");
            int i = 1;
            foreach (var item in playerScore)
            {
                Console.WriteLine("({0}): {1,-20} Punkte: {2}", i, item.Key, item.Value);
                i++;
            }
        }
    }
}