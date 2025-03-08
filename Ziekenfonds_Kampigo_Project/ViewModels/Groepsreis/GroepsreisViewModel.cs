using Models;
using Ziekenfonds_Kampigo_Project.ViewModels.Activiteit;
using Ziekenfonds_Kampigo_Project.ViewModels.Review;

namespace Ziekenfonds_Kampigo_Project.ViewModels.Groepsreis
{
    public class GroepsreisViewModel
    {
        public int Id { get; set; }
        public required string Naam { get; set; }
        public DateTime BeginDatum { get; set; }
        public DateTime EindDatum { get; set; }
        public float? Prijs { get; set; }
        public ActiviteitListViewModel? Activiteiten { get; set; }

    }
}
