using Ziekenfonds_Kampigo_Project.ViewModels.Bestemming;
using Ziekenfonds_Kampigo_Project.ViewModels.Gebruiker;
using Ziekenfonds_Kampigo_Project.ViewModels.Monitor;

namespace Ziekenfonds_Kampigo_Project.ViewModels.Groepsreis
{
    public class GroepsreisDetailsWithMonitors
    {
        public int Id { get; set; }
        public DateTime? Begindatum { get; set; }
        public DateTime? Einddatum { get; set; }
        public float? Prijs { get; set; }
        public required BestemmingDetailViewModel Bestemming { get; set; }
        public List<MonitorViewModel>? Monitoren { get; set; } = [];
        public List<GebruikerWithOpleidingenViewModel>? AllUsers { get; set; } = [];
    }
}
