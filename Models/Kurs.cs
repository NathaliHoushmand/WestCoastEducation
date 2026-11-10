using System.Text.Json.Serialization;
using WestCoastEducation.Interfaces;

namespace WestCoastEducation.Models;

/// <summary>
/// Abstrakt basklass för en kurs. Klassrumskurs och Distanskurs ärver
/// härifrån (arv, VG-krav) istället för att duplicera kursfälten.
///
/// JsonPolymorphic/JsonDerivedType gör att en lista av (abstrakta) Kurs
/// kan skrivas till och läsas från JSON och ändå återskapas som rätt
/// konkret typ (Klassrumskurs/Distanskurs) - VG-kravet om att kunna
/// skriva ner och läsa upp kurser som JSON.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "Typ")]
[JsonDerivedType(typeof(Klassrumskurs), typeDiscriminator: "Klassrumskurs")]
[JsonDerivedType(typeof(Distanskurs), typeDiscriminator: "Distanskurs")]
public abstract class Kurs : IPresenterbar
{
    public string Kursnummer { get; set; }
    public string Titel { get; set; }
    public int LängdIVeckor { get; set; }
    public DateTime Startdatum { get; set; }
    public DateTime Slutdatum { get; set; }

    /// <summary>
    /// Aggregation: kursen "känner till" sin ansvariga lärare men äger
    /// inte läraren (läraren finns kvar även om kursen tas bort).
    /// JsonIgnore för att undvika cirkulär referens mot Lärare.AnsvarigaKurser.
    /// </summary>
    [JsonIgnore]
    public Lärare? AnsvarigLärare { get; set; }

    /// <summary>
    /// Composition-liknande relation: deltagarlistan hör tydligt till
    /// just den här kursen. JsonIgnore för att undvika cirkulär
    /// referens mot Studerande.BokadeKurser.
    /// </summary>
    [JsonIgnore]
    public List<Studerande> Deltagare { get; } = new();

    public List<int> Betyg { get; } = new();

    [JsonIgnore]
    public abstract string KursTypNamn { get; }

    protected Kurs(string kursnummer, string titel, int längdIVeckor, DateTime startdatum, DateTime slutdatum)
    {
        Kursnummer = kursnummer;
        Titel = titel;
        LängdIVeckor = längdIVeckor;
        Startdatum = startdatum;
        Slutdatum = slutdatum;
    }

    /// <summary>
    /// Lägger till en deltagare om den inte redan är anmäld
    /// (kontrollerar via personnummer, inte objektreferens).
    /// </summary>
    public void LäggTillDeltagare(Studerande studerande)
    {
        if (studerande is null)
        {
            throw new ArgumentNullException(nameof(studerande));
        }

        bool redanAnmäld = false;
        foreach (var s in Deltagare)
        {
            if (s.Personnummer == studerande.Personnummer)
            {
                redanAnmäld = true;
                break;
            }
        }

        if (!redanAnmäld)
        {
            Deltagare.Add(studerande);
        }
    }

    public void LäggTillBetyg(int betyg)
    {
        if (betyg < 1 || betyg > 5)
        {
            throw new ArgumentOutOfRangeException(nameof(betyg), "Betyg måste vara mellan 1 och 5.");
        }

        Betyg.Add(betyg);
    }

    public double GenomsnittligtBetyg()
    {
        if (Betyg.Count == 0)
        {
            return 0;
        }

        int summa = 0;
        foreach (var b in Betyg)
        {
            summa += b;
        }

        return Math.Round((double)summa / Betyg.Count, 2);
    }

    public int DagarTillStart()
    {
        return (Startdatum.Date - DateTime.Now.Date).Days;
    }

    /// <summary>
    /// Affärsregel från kravspecen: om det är 3 veckor (21 dagar) eller
    /// mindre kvar till kursstart och färre än 5 deltagare är anmälda
    /// riskerar kursen att behöva ställas in av ekonomiska skäl.
    /// </summary>
    public bool RiskereraInställning()
    {
        return DagarTillStart() <= 21 && Deltagare.Count < 5;
    }

    public virtual string PresenteraInfo()
    {
        string lärareInfo = AnsvarigLärare is null
            ? "Ej tillsatt"
            : $"{AnsvarigLärare.Förnamn} {AnsvarigLärare.Efternamn}";

        return $"[{Kursnummer}] {Titel} ({KursTypNamn}) | Längd: {LängdIVeckor} veckor | " +
               $"Start: {Startdatum:yyyy-MM-dd} | Slut: {Slutdatum:yyyy-MM-dd} | " +
               $"Lärare: {lärareInfo} | Antal deltagare: {Deltagare.Count} | " +
               $"Snittbetyg: {GenomsnittligtBetyg()}";
    }

    public override string ToString() => PresenteraInfo();
}
