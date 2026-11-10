namespace WestCoastEducation.Models;

/// <summary>
/// Representerar en studerande. Har en historik över bokade kurser
/// (aggregation - en Studerande "känner till" sina Kurs-objekt, men
/// äger dem inte; kursen finns kvar även om studenten tas bort).
/// </summary>
public class Studerande : Person
{
    public List<Kurs> BokadeKurser { get; } = new();

    public Studerande(
        string förnamn,
        string efternamn,
        string telefon,
        string personnummer,
        string adress,
        string postnummer,
        string ort)
        : base(förnamn, efternamn, telefon, personnummer, adress, postnummer, ort)
    {
    }

    /// <summary>
    /// Bokar en kurs åt studenten, lägger till studenten i kursens
    /// deltagarlista och sparar kursen i studentens historik.
    /// </summary>
    public void BokaKurs(Kurs kurs)
    {
        if (kurs is null)
        {
            throw new ArgumentNullException(nameof(kurs));
        }

        bool finnsRedan = false;
        foreach (var bokad in BokadeKurser)
        {
            if (bokad.Kursnummer == kurs.Kursnummer)
            {
                finnsRedan = true;
                break;
            }
        }

        if (!finnsRedan)
        {
            BokadeKurser.Add(kurs);
            kurs.LäggTillDeltagare(this);
        }
    }

    public override string PresenteraInfo()
    {
        return $"{base.PresenteraInfo()} | Roll: Studerande | Antal bokade kurser: {BokadeKurser.Count}";
    }
}
