using WestCoastEducation.Models;
using WestCoastEducation.Services;

var register = new KursRegister();

// --- Lärare, utbildningsledare och administratör ---
var lärare1 = new Lärare(
    "Anna", "Karlsson", "0701234567", "19800101-1234",
    "Storgatan 1", "41101", "Göteborg", "Webbutveckling");

var ledare1 = new Utbildningsledare(
    "Björn", "Nilsson", "0709876543", "19750505-5678",
    "Kungsgatan 5", "41102", "Göteborg", "Projektledning", new DateTime(2015, 8, 1));

var admin1 = new Administratör(
    "Cecilia", "Ström", "0701112233", "19900303-4321",
    "Vasagatan 2", "41103", "Göteborg", "Systemadministration", new DateTime(2019, 1, 15));

register.LäggTillLärare(lärare1);
register.LäggTillUtbildningsledare(ledare1);
register.LäggTillAdministratör(admin1);

// --- Kurser (en klassrumskurs och en distanskurs) ---
var klassrumskurs = new Klassrumskurs(
    "WEB101", "Grundläggande C#", 5,
    DateTime.Now.AddDays(10), DateTime.Now.AddDays(45), "Sal A203");
klassrumskurs.AnsvarigLärare = lärare1;
lärare1.LäggTillAnsvarigKurs(klassrumskurs);

var distanskurs = new Distanskurs(
    "WEB202", "ASP.NET Core On-Demand", 3,
    DateTime.Now.AddDays(5), DateTime.Now.AddDays(26), "https://teams.example.com/kurs202");
distanskurs.AnsvarigLärare = lärare1;
lärare1.LäggTillAnsvarigKurs(distanskurs);

register.LäggTillKurs(klassrumskurs);
register.LäggTillKurs(distanskurs);

// --- Studerande ---
var studerande1 = new Studerande(
    "David", "Petersson", "0731122334", "19950612-2211",
    "Lindvägen 3", "41104", "Göteborg");

var studerande2 = new Studerande(
    "Elin", "Svensson", "0739988776", "19960910-3322",
    "Björkallén 7", "41105", "Göteborg");

register.LäggTillStuderande(studerande1);
register.LäggTillStuderande(studerande2);

studerande1.BokaKurs(klassrumskurs);
studerande2.BokaKurs(klassrumskurs);

klassrumskurs.LäggTillBetyg(5);
klassrumskurs.LäggTillBetyg(4);

// --- Listning ---
Console.WriteLine("=== Alla kurser ===");
register.ListaKurser();

Console.WriteLine();
Console.WriteLine("=== Alla studerande ===");
register.ListaStuderande();

// --- Affärsregel: kurser som riskerar att ställas in ---
Console.WriteLine();
Console.WriteLine("=== Kurser som riskerar att ställas in (<5 deltagare, <=3 veckor kvar) ===");
var riskKurser = register.HämtaKurserSomRiskererarInställning();

if (riskKurser.Count == 0)
{
    Console.WriteLine("Inga kurser riskerar att ställas in just nu.");
}
else
{
    foreach (var kurs in riskKurser)
    {
        Console.WriteLine($"- {kurs.Titel} ({kurs.Kursnummer})");
    }
}

// --- JSON-export/import av kurser (VG-krav) ---
string jsonSökväg = "kurser.json";
register.SparaKurserTillJson(jsonSökväg);
Console.WriteLine();
Console.WriteLine($"Kurser sparade till {jsonSökväg}");

var nyttRegister = new KursRegister();
nyttRegister.LäsKurserFrånJson(jsonSökväg);

Console.WriteLine();
Console.WriteLine("=== Kurser inlästa från JSON (nytt register) ===");
nyttRegister.ListaKurser();

Console.WriteLine();
Console.WriteLine("Klart.");
