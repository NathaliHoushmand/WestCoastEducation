namespace WestCoastEducation.Interfaces;

/// <summary>
/// Gemensamt kontrakt för alla objekt som ska kunna presentera
/// sig själva som en läsbar sträng (krav: "Presentera varje objekt
/// som en sträng med intressant information").
/// </summary>
public interface IPresenterbar
{
    string PresenteraInfo();
}
