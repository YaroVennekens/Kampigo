using System.ComponentModel.DataAnnotations;

namespace Ziekenfonds_Kampigo_Project.ViewModels.Groepsreis
{
    public class GroepsreisInschrijvingViewModel
    {
        public int Id { get; set; }
        public string Naam { get; set; }
        public string Beschrijving { get; set; }
        public DateTime? BeginDatum { get; set; }
        public DateTime? EindDatum { get; set; }
        public float? Prijs { get; set; }
    }

}
