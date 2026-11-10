namespace WestCoastEducation.Models;

/// <summary>
/// Utbildningsledare hanterar samma information som Lärare (därför
/// ärver den från Lärare istället för att duplicera fälten) plus
/// ett anställningsdatum.
/// </summary>
public class Utbildningsledare : Lärare
{
    public DateTime Anställningsdatum { get; set; }

    public Utbildningsledare(
        string förnamn,
        string efternamn,
        string telefon,
        string personnummer,
        string adress,
        string postnummer,
        string ort,
        string kunskapsområde,
        DateTime anställningsdatum)
        : base(förnamn, efternamn, telefon, personnummer, adress, postnummer, ort, kunskapsområde)
    {
        Anställningsdatum = anställningsdatum;
    }

    public override string PresenteraInfo()
    {
        return $"{base.PresenteraInfo()} | Roll: Utbildningsledare | " +
               $"Anställd sedan: {Anställningsdatum:yyyy-MM-dd}";
    }
}
