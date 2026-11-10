using System.Linq;
using System.Text.Json;
using WestCoastEducation.Models;

namespace WestCoastEducation.Services;

/// <summary>
/// Samlar och administrerar alla kurser, studerande, lärare,
/// utbildningsledare och administratörer. Motsvarar systemets
/// "administrationsdel" - lägg till/ta bort/lista, samt affärslogik
/// som kontroll av inställda kurser och JSON-export/import av kurser.
/// </summary>
public class KursRegister
{
    public List<Kurs> Kurser { get; } = new();
    public List<Studerande> Studerande { get; } = new();
    public List<Lärare> Lärare { get; } = new();
    public List<Utbildningsledare> Utbildningsledare { get; } = new();
    public List<Administratör> Administratörer { get; } = new();

    public void LäggTillKurs(Kurs kurs)
    {
        if (kurs is null)
        {
            throw new ArgumentNullException(nameof(kurs));
        }

        foreach (var befintlig in Kurser)
        {
            if (befintlig.Kursnummer == kurs.Kursnummer)
            {
                throw new InvalidOperationException($"En kurs med kursnummer {kurs.Kursnummer} finns redan.");
            }
        }

        Kurser.Add(kurs);
    }

    public bool TaBortKurs(string kursnummer)
    {
        var kurs = Kurser.FirstOrDefault(k => k.Kursnummer == kursnummer);
        if (kurs is null)
        {
            return false;
        }

        Kurser.Remove(kurs);
        return true;
    }

    public void LäggTillStuderande(Studerande studerande) => LäggTillPerson(Studerande, studerande);

    public void LäggTillLärare(Lärare lärare) => LäggTillPerson(Lärare, lärare);

    public void LäggTillUtbildningsledare(Utbildningsledare ledare) => LäggTillPerson(Utbildningsledare, ledare);

    public void LäggTillAdministratör(Administratör admin) => LäggTillPerson(Administratörer, admin);

    private static void LäggTillPerson<T>(List<T> lista, T person) where T : Person
    {
        if (person is null)
        {
            throw new ArgumentNullException(nameof(person));
        }

        bool finnsRedan = false;
        foreach (var befintlig in lista)
        {
            if (befintlig.Personnummer == person.Personnummer)
            {
                finnsRedan = true;
                break;
            }
        }

        if (!finnsRedan)
        {
            lista.Add(person);
        }
    }

    public void ListaKurser()
    {
        if (Kurser.Count == 0)
        {
            Console.WriteLine("Inga kurser registrerade.");
            return;
        }

        foreach (var kurs in Kurser)
        {
            Console.WriteLine(kurs.PresenteraInfo());
        }
    }

    public void ListaStuderande()
    {
        if (Studerande.Count == 0)
        {
            Console.WriteLine("Inga studerande registrerade.");
            return;
        }

        foreach (var s in Studerande)
        {
            Console.WriteLine(s.PresenteraInfo());
        }
    }

    /// <summary>
    /// Går igenom alla kurser och returnerar de som enligt kravspecen
    /// riskerar att behöva ställas in (&lt;= 3 veckor kvar och färre
    /// än 5 anmälda deltagare).
    /// </summary>
    public List<Kurs> HämtaKurserSomRiskererarInställning()
    {
        var riskKurser = new List<Kurs>();

        foreach (var kurs in Kurser)
        {
            if (kurs.RiskereraInställning())
            {
                riskKurser.Add(kurs);
            }
        }

        return riskKurser;
    }

    /// <summary>
    /// Skriver ner alla kurser som ett JSON-dokument (VG-krav).
    /// Tack vare JsonPolymorphic/JsonDerivedType på Kurs bevaras
    /// informationen om vilken konkret typ (Klassrumskurs/Distanskurs)
    /// varje kurs har.
    /// </summary>
    public void SparaKurserTillJson(string sökväg)
    {
        var alternativ = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        string json = JsonSerializer.Serialize(Kurser, alternativ);
        File.WriteAllText(sökväg, json);
    }

    /// <summary>
    /// Läser in kurser från ett JSON-dokument (VG-krav) och ersätter
    /// den aktuella kurslistan.
    /// </summary>
    public void LäsKurserFrånJson(string sökväg)
    {
        if (!File.Exists(sökväg))
        {
            Console.WriteLine($"Filen {sökväg} hittades inte.");
            return;
        }

        string json = File.ReadAllText(sökväg);
        var inläsdaKurser = JsonSerializer.Deserialize<List<Kurs>>(json);

        if (inläsdaKurser is null)
        {
            return;
        }

        Kurser.Clear();
        foreach (var kurs in inläsdaKurser)
        {
            Kurser.Add(kurs);
        }
    }
}
