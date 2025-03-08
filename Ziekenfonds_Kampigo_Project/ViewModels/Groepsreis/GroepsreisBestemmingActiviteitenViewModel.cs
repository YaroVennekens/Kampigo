using Ziekenfonds_Kampigo_Project.ViewModels.Activiteit;
using Ziekenfonds_Kampigo_Project.ViewModels.Bestemming;

namespace Ziekenfonds_Kampigo_Project.ViewModels.Groepsreis
{
    public class GroepsreisBestemmingActiviteitenViewModel
    {
        public int Id { get; set; }
        public string Naam { get; set; }
        public DateTime? BeginDatum { get; set; }
        public DateTime? EindDatum { get; set; }
        public float? Prijs { get; set; }
        public BestemmingViewModel Bestemming { get; set; }
        public List<ActiviteitViewModel> Activiteiten { get; set; }
    }
}
