namespace WestCoastEducation.Models;

/// <summary>
/// En kurs som ges på distans, exempelvis en förinspedad
/// On-Demand-kurs som köps eller prenumereras på.
/// </summary>
public class Distanskurs : Kurs
{
    public string MötesLänk { get; set; }

    public Distanskurs(
        string kursnummer,
        string titel,
        int längdIVeckor,
        DateTime startdatum,
        DateTime slutdatum,
        string mötesLänk)
        : base(kursnummer, titel, längdIVeckor, startdatum, slutdatum)
    {
        MötesLänk = mötesLänk;
    }

    public override string KursTypNamn => "Distanskurs";

    public override string PresenteraInfo()
    {
        return $"{base.PresenteraInfo()} | Länk: {MötesLänk}";
    }
}
