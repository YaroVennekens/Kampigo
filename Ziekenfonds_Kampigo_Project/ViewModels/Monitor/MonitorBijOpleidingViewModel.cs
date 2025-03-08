namespace Ziekenfonds_Kampigo_Project.ViewModels.Monitor
{
    public class MonitorBijOpleidingViewModel
    {
        public required string Id { get; set; }
        public required string Naam { get; set; }
        public int VolgendeOpleidingId { get; set; }
        public bool IsGeaccepteerd { get; set; }
    }
}
