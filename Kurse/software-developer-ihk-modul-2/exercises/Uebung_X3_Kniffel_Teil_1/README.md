# Übung - Kniffel Teil 1

Es soll eine Konsolenanwendung erstellt werden welche die Würfelergebnisse von fünf Würfeln zusammenzählt und die möglichen [Kniffelpunkte](https://de.wikipedia.org/wiki/Kniffel) anzeigt.

* Fünf Zahlen im Bereich von 1-6 akzeptieren
    * Manuelle Eingabe
    * Zufallszahlen 
* Ablauf in einer Schleife bis explizit beendet wird

## Vorgaben

### Punkte Enum

```csharp
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
```

Der Enum definiert die Indexwerte für das zurückgelieferte Array. Das zurückgelieferte Array hat also eine Länge von `Enum.GetValues(typeof(Scores)).Length` und enthält als Werte die Punkte, die durch den übergebenen Wurf in jeder der Kategorien erzielt werden können.

### Methode

Die Signatur der Methode welche die Punkte ermittelt ist ebenfalls vorgegeben:

`public static int[] GetPoints(int[] dice)`

## Beispiele

```bash
Wuerfelwerte:  1 3 6 3 4

Punktzahlen:
         Einser 1
         Zweier 0
         Dreier 6
         Vierer 4
        Fuenfer 0
        Sechser 6
    Dreierpasch 0
    Viererpasch 0
      FullHouse 0
  KleineStrasse 0
  GrosseStrasse 0
        Kniffel 0
         Chance 17

Fünf Zahlen von 1-6 eingeben
'z' für Zufallszahlen
'q' zum beenden
```

```bash
Wuerfelwerte:  2 3 1 4 4

Punktzahlen:
         Einser 1
         Zweier 2
         Dreier 3
         Vierer 8
        Fuenfer 0
        Sechser 0
    Dreierpasch 0
    Viererpasch 0
      FullHouse 0
  KleineStrasse 30
  GrosseStrasse 0
        Kniffel 0
         Chance 14

Fünf Zahlen von 1-6 eingeben
'z' für Zufallszahlen
'q' zum beenden
```

```bash
Wuerfelwerte:  4 6 6 6 4

Punktzahlen:
         Einser 0
         Zweier 0
         Dreier 0
         Vierer 8
        Fuenfer 0
        Sechser 18
    Dreierpasch 26
    Viererpasch 0
      FullHouse 25
  KleineStrasse 0
  GrosseStrasse 0
        Kniffel 0
         Chance 26

Fünf Zahlen von 1-6 eingeben
'z' für Zufallszahlen
'q' zum beenden
```

Es werden also alle Punkte angezeigt, die bei diesem Wurf möglich sind.