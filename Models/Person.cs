using WestCoastEducation.Interfaces;

namespace WestCoastEducation.Models;

/// <summary>
/// Abstrakt basklass för alla personer i systemet (Studerande, Lärare,
/// Utbildningsledare, Administratör). Samlar de gemensamma uppgifterna
/// så att de inte dupliceras i varje underklass (arv, VG-krav).
/// </summary>
public abstract class Person : IPresenterbar
{
    public string Förnamn { get; set; }
    public string Efternamn { get; set; }
    public string Telefon { get; set; }
    public string Personnummer { get; set; }
    public string Adress { get; set; }
    public string Postnummer { get; set; }
    public string Ort { get; set; }

    protected Person(
        string förnamn,
        string efternamn,
        string telefon,
        string personnummer,
        string adress,
        string postnummer,
        string ort)
    {
        Förnamn = förnamn;
        Efternamn = efternamn;
        Telefon = telefon;
        Personnummer = personnummer;
        Adress = adress;
        Postnummer = postnummer;
        Ort = ort;
    }

    public virtual string PresenteraInfo()
    {
        return $"{Förnamn} {Efternamn} | Tel: {Telefon} | Personnr: {Personnummer} | " +
               $"Adress: {Adress}, {Postnummer} {Ort}";
    }

    public override string ToString() => PresenteraInfo();
}
