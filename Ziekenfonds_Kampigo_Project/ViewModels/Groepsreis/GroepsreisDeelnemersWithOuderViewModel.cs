using Ziekenfonds_Kampigo_Project.ViewModels.Bestemming;
using Ziekenfonds_Kampigo_Project.ViewModels.Deelnemer;

namespace Ziekenfonds_Kampigo_Project.ViewModels.Groepsreis
{
    public class GroepsreisDeelnemersWithOuderViewModel
    {
        public required GroepsreisViewModel Groepsreis { get; set; }
        public required BestemmingDetailViewModel Bestemming { get; set; }
        public List<DeelnemerViewModel> Deelnemers { get; set; } = [];
    }
}
