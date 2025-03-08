namespace Ziekenfonds_Kampigo_Project.ViewModels.Groepsreis
{
    public class MijnMonitoredGroepsreisViewModel
    {
        public int Id { get; set; }
        public required string Naam { get; set; }
        public DateTime BeginDatum { get; set; }
        public DateTime EindDatum { get; set; }
        public float Prijs { get; set; }
        public bool IsHoofdMonitor { get; set; }
        public bool IsToegewezen { get; set; }
    }
}
