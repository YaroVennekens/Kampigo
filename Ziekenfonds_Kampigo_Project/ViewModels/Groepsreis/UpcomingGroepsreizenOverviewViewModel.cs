namespace Ziekenfonds_Kampigo_Project.ViewModels.Groepsreis
{
    public class UpcomingGroepsreizenOverviewViewModel
    {
        public required string BestemmingNaam { get; set; }
        public DateTime BeginDatum { get; set; }
        public DateTime EindDatum { get; set; }
        public int AantalDeelnemers { get; set; }
        public int AantalMonitors { get; set; }
        public List<string>? HoofdmonitorNamen { get; set; } = [];
    }
}
