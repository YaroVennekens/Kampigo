namespace Ziekenfonds_Kampigo_Project.ViewModels.Groepsreis
{
    public class RegisteredGroepsreisViewModel
    {
        public int Id { get; set; }
        public required string Naam { get; set; } // Bestemming naam
        public DateTime BeginDatum { get; set; }
        public DateTime EindDatum { get; set; }
        public float Prijs { get; set; }
        public string? Opmerking { get; set; }
        public required string KindNaam { get; set; } 
    }
}
