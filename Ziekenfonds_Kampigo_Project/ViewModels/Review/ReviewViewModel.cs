using Ziekenfonds_Kampigo_Project.ViewModels.Groepsreis;

namespace Ziekenfonds_Kampigo_Project.ViewModels.Review
{
    public class ReviewViewModel
    {
        public int? GroepsreisId { get; set; }
        public string? GroepsreisNaam { get; set; }
        public int BestemmingId { get; set; }
        public int Score { get; set; } 
        public string? Tekst { get; set; }
        public List<GroepsreisViewModel>? Groepsreizen { get; set; }

    }
}
