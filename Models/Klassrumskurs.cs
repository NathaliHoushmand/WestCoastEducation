namespace WestCoastEducation.Models;

/// <summary>
/// En lärarledd kurs som körs i ett fysiskt klassrum eller via
/// länk (distans-lärarledd), dvs. inte en förinspelad On-Demand-kurs.
/// </summary>
public class Klassrumskurs : Kurs
{
    public string Lokal { get; set; }

    public Klassrumskurs(
        string kursnummer,
        string titel,
        int längdIVeckor,
        DateTime startdatum,
        DateTime slutdatum,
        string lokal)
        : base(kursnummer, titel, längdIVeckor, startdatum, slutdatum)
    {
        Lokal = lokal;
    }

    public override string KursTypNamn => "Klassrumskurs";

    public override string PresenteraInfo()
    {
        return $"{base.PresenteraInfo()} | Lokal: {Lokal}";
    }
}
