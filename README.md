INA
=====================

#### **Modul(1)**

> **Was geht?**
> - GUI vorhanden
> - 1 Datei einlesen + zerteilen + pruefen
> - Datensaetze UND Header/Footer in die Queue 
> - Mehr als eine Datei gleichzeitig einlesen
> - Dateischnipsel in MSMQ laden
> - Inhalt der MSMQ wieder abrufen
> - Mehr als einen Buchungssatz
> - Fehlerhafte Buchungssaetze in Log-Datei speichen
> - Neues Pattern: Pipes + Filters
> - string[] words = stringname.Split(" ");

> **To Do**
> - Grundzuege Ladebalken
> - Fehlermeldungen: Log GUI (Observer?)

#### **Modul(2)**

> **Was geht?**
> Threads horchen an MSMQ, Abarbeitung nacheinander (leider)

> **To Do**
> - Threads sollen PARALLEL einlesen
> - Thema BackoutQueue

#### **Modul(3)**

> **Was geht?**
> - ...

> **To Do**
> - Verbindung zur Datenbank
> - Diverse Pruefungen
> - logging framework
> - Daten in Datenbank schreiben