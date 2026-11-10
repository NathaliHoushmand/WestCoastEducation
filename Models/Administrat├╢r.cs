namespace WestCoastEducation.Models;

/// <summary>
/// Administratör behöver samma information som Utbildningsledare
/// (spec: "Behöver samma hantera samma information som utbildningsledare"),
/// så den ärver direkt från Utbildningsledare istället för att
/// duplicera fälten igen.
/// </summary>
public class Administratör : Utbildningsledare
{
    public Administratör(
        string förnamn,
        string efternamn,
        string telefon,
        string personnummer,
        string adress,
        string postnummer,
        string ort,
        string kunskapsområde,
        DateTime anställningsdatum)
        : base(förnamn, efternamn, telefon, personnummer, adress, postnummer, ort, kunskapsområde, anställningsdatum)
    {
    }

    public override string PresenteraInfo()
    {
        return $"{Förnamn} {Efternamn} | Tel: {Telefon} | Personnr: {Personnummer} | " +
               $"Adress: {Adress}, {Postnummer} {Ort} | Roll: Administratör | " +
               $"Kunskapsområde: {Kunskapsområde} | Anställd sedan: {Anställningsdatum:yyyy-MM-dd}";
    }
}
