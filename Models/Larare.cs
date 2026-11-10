namespace WestCoastEducation.Models;

/// <summary>
/// Representerar en lärare. Ärver all personinformation från Person
/// och lägger bara till det som är unikt för lärarrollen.
/// </summary>
public class Lärare : Person
{
    public string Kunskapsområde { get; set; }

    /// <summary>
    /// De kurser läraren är ansvarig för (aggregation).
    /// </summary>
    public List<Kurs> AnsvarigaKurser { get; } = new();

    public Lärare(
        string förnamn,
        string efternamn,
        string telefon,
        string personnummer,
        string adress,
        string postnummer,
        string ort,
        string kunskapsområde)
        : base(förnamn, efternamn, telefon, personnummer, adress, postnummer, ort)
    {
        Kunskapsområde = kunskapsområde;
    }

    public void LäggTillAnsvarigKurs(Kurs kurs)
    {
        if (kurs is null)
        {
            throw new ArgumentNullException(nameof(kurs));
        }

        if (!AnsvarigaKurser.Contains(kurs))
        {
            AnsvarigaKurser.Add(kurs);
        }
    }

    public override string PresenteraInfo()
    {
        return $"{base.PresenteraInfo()} | Roll: Lärare | Kunskapsområde: {Kunskapsområde} | " +
               $"Antal ansvariga kurser: {AnsvarigaKurser.Count}";
    }
}
