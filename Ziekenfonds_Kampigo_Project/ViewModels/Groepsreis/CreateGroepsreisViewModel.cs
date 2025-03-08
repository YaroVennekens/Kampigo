using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using Ziekenfonds_Kampigo_Project.ViewModels.Custom_Validators;

namespace Ziekenfonds_Kampigo_Project.ViewModels.Groepsreis
{
    public class CreateGroepsreisViewModel
    {
        public List<SelectListItem> Activiteiten { get; set; } = [];
        public List<int> ActiviteitenIds { get; set; } = [];

        // Existing fields for Groepsreis creation
        [Required(ErrorMessage = "Begindatum is verplicht.")]
        [NotInThePast(ErrorMessage = "Begindatum mag niet in het verleden liggen.")]
        public DateTime Begindatum { get; set; } = DateTime.Now;

        // List of possible destinations and activities (for the select dropdowns)
        public List<SelectListItem> Bestemmingen { get; set; } = [];

        public int? BestemmingId { get; set; }

        [Required(ErrorMessage = "Einddatum is verplicht.")]
        [NotInThePast(ErrorMessage = "Einddatum mag niet in het verleden liggen.")]
        public DateTime Einddatum { get; set; } = DateTime.Now.AddDays(1);

        public int Id { get; set; }

        [BindNever]
        public bool IsBestemmingDisabled { get; set; }

        public bool IsCreatingNewBestemming { get; set; }

        public string? NieuweBestemmingBeschrijving { get; set; }

        public int NieuweBestemmingMaxLeeftijd { get; set; }

        public int NieuweBestemmingMinLeeftijd { get; set; }

        public string? NieuweBestemmingNaam { get; set; }

        // Foreign Key to Bestemming
        // Photo upload (multiple photos)
        public List<IFormFile> Photos { get; set; } = [];

        [Required(ErrorMessage = "Prijs is verplicht.")]
        [Range(0.01, 10000.00, ErrorMessage = "Prijs moet tussen 0.01 en 10000.00 liggen.")]
        public float Prijs { get; set; }

        // List of selected activity IDs
    }
}