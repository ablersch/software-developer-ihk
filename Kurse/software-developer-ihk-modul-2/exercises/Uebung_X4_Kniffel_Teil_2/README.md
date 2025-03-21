# Übung - Kniffel Teil 2

Aus der vorherigen Aufgabe, welche aktuell nur die Punkte eines Wurfes berechnet, soll ein komplettes Spiel werden. Bitte folgendes beachten und implementieren.

## Zum Spiel

* Es können x menschliche Spieler an einem Computer mitspielen
* Die Würfel können nur einmal pro Runde zufällig gewürfelt werden
* Der Spieler kann auswählen in welches Feld er die Punkte speichern möchte
    * Prüfen ob Eingabe sinnvoll bzw. erlaubt ist
    * Er soll in jeder Runde eine Übersicht haben welche Felder frei sind bzw. wo er schon Punkte vergeben hat
* Am Ende werden die Punkte aller Spieler angezeigt und der Sieger wird angezeigt
* Es soll die Möglichkeit bestehen nach einem fertigen Spiel ein neues zu starten

## Zur Umsetzung:
* Es sollen sinnvoll Klassen und Methoden erstellt werden
* Das Enum und die GetPoints Methode sollen weiter benutzt werden

## Beispiel

```bash
Anzahl Spieler? 2
Name Spieler 1: Andreas
Name Spieler 2: PC
Spiel wird gestartet.
Spieler Andreas ist an der Reihe. Bitte würfeln...

... es wurde folgendes gewürfelt: 1 5 2 4 5

Bitte Feld wählen in welches die Punkte eingetragen werden soll:
(1): Einser               Punkte:
(2): Zweier               Punkte:
(3): Dreier               Punkte:
(4): Vierer               Punkte:
(5): Fuenfer              Punkte:
(6): Sechser              Punkte:
(7): Dreierpasch          Punkte:
(8): Viererpasch          Punkte:
(9): FullHouse            Punkte:
(10): KleineStrasse        Punkte:
(11): GrosseStrasse        Punkte:
(12): Kniffel              Punkte:
(13): Chance               Punkte:
1
In das Feld Einser wurden 1 Punkte eingetragen.
Spieler PC ist an der Reihe. Bitte würfeln...
```
