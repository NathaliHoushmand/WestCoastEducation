# WestCoast Education – Inlämningsuppgift 1

.NET 8 konsolapplikation enligt kravspecen för WestCoast Education.

## Köra programmet

```bash
dotnet run
```

Detta bygger och kör `Program.cs`, som skapar lärare, en utbildningsledare,
en administratör, två kurser (en klassrumskurs och en distanskurs) samt två
studerande, bokar kurser, listar allt, kontrollerar vilka kurser som
riskerar att ställas in, och sparar/läser in kurserna som JSON
(`kurser.json`).

## Struktur

```
WestCoastEducation.csproj
Program.cs                     – demo/Main
Interfaces/
  IPresenterbar.cs              – PresenteraInfo()-kontraktet
Models/
  Person.cs                     – abstrakt basklass (G: gemensam data)
  Studerande.cs                 – ärver Person
  Larare.cs                     – ärver Person
  Utbildningsledare.cs          – ärver Lärare (VG: minimerar dubblering)
  Administratör.cs              – ärver Utbildningsledare
  Kurs.cs                       – abstrakt basklass, JSON-polymorfism
  Klassrumskurs.cs               – ärver Kurs (lärarledd, i klassrum/på distans)
  Distanskurs.cs                 – ärver Kurs (inspelad/on-demand)
Services/
  KursRegister.cs                – administrationsklass: lägg till/ta bort/
                                    lista kurser, lärare, studerande m.fl.,
                                    affärslogik för "risk att ställas in",
                                    JSON-export/import
```

## Hur kraven uppfylls

**G**
- Alla efterfrågade klasser finns: Kurs, Studerande, Lärare,
  Utbildningsledare, Administratör.
- Aggregation/composition: `Kurs` har en lista med `Studerande`
  (deltagare) och en referens till en ansvarig `Lärare`; `Lärare` har
  en lista med `Kurs` den är ansvarig för; `Studerande` har en lista
  med bokade `Kurs` (historik).
- Två fullt fungerande klasser med logik: **`Kurs`** (betygssnitt,
  dagar till start, kontroll om kursen riskerar att ställas in,
  dubblettkontroll av deltagare) och **`KursRegister`** (lägg
  till/ta bort, listning, riskkontroll över alla kurser, JSON
  spara/läsa). Båda använder villkor (`if`) och loopar (`foreach`).
- Metoder för att lägga till kurser, lärare, studerande,
  utbildningsledare och administratörer samt för att lista kurser
  och studerande finns i `KursRegister`.

**VG**
- Arv används genomgående för att undvika dubblerad kod: `Person` →
  `Studerande`/`Lärare` → `Utbildningsledare` → `Administratör`, samt
  `Kurs` → `Klassrumskurs`/`Distanskurs`.
- `IPresenterbar`-interfacet används av både `Person`- och
  `Kurs`-hierarkin för `PresenteraInfo()`.
- Kurser kan skrivas ner till och läsas upp från JSON
  (`KursRegister.SparaKurserTillJson` / `LäsKurserFrånJson`), med
  bevarad information om konkret kurstyp via
  `[JsonPolymorphic]`/`[JsonDerivedType]`.

## OBS – ej testkört i den här miljön

Koden är skriven och genomläst noggrant, men kunde inte byggas/köras
här eftersom .NET SDK inte finns installerat i den här sandlådan.
Kör `dotnet build` och `dotnet run` lokalt (eller i din IDE) som ett
första steg innan ni lämnar in – om något fel dyker upp, klistra in
felmeddelandet så hjälper jag er felsöka direkt.
