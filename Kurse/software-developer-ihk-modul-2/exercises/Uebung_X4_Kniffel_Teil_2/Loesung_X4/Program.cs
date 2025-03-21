using System;

namespace Kniffel;

internal class Program
{
    private static void Main()
    {
        do
        {
            Console.Write("Anzahl Spieler? ");
            if (int.TryParse(Console.ReadLine(), out int numberOfPlayers))
            {
                for (int i = 0; i < numberOfPlayers; i++)
                {
                    Console.Write($"Name Spieler {i + 1}: ");
                    new Player(Console.ReadLine());
                }

                Game.StartGame();
                Game.Players.Clear();
                Console.WriteLine("\nNoch eine Runde? j/n");
                Console.Clear();
            }
            else
            {
                Console.WriteLine("Es wurde keine passende Spieleranzahl eingeben. Programm wird beim drücken einer Taste beendet");
                Console.ReadKey();
                Environment.Exit(0);
            }
        } while (Console.ReadKey().KeyChar.ToString() == "j");

        Console.ReadKey();
    }
}