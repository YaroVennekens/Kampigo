using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using Ziekenfonds_Kampigo_Project.ViewModels.Bestemming;
using Ziekenfonds_Kampigo_Project.ViewModels.Custom_Validators;

namespace Ziekenfonds_Kampigo_Project.ViewModels.Groepsreis
{
    public class EditGroepsreisViewModel
    {
        public List<SelectListItem>? Activiteiten { get; set; } = [];
        public List<int>? ActiviteitenIds { get; set; } = [];

        [Required(ErrorMessage = "Begindatum is verplicht.")]
        [DataType(DataType.Date, ErrorMessage = "Ongeldige datum.")]
        [Display(Name = "Begindatum")]
        [NotInThePast(ErrorMessage = "Begindatum mag niet in het verleden liggen.")]
        public DateTime Begindatum { get; set; }

        // Destination details
        public required EditBestemmingViewModel Bestemming { get; set; }

        [Required(ErrorMessage = "Einddatum is verplicht.")]
        [DataType(DataType.Date, ErrorMessage = "Ongeldige datum.")]
        [Display(Name = "Einddatum")]
        [NotInThePast(ErrorMessage = "Einddatum mag niet in het verleden liggen.")]
        public DateTime Einddatum { get; set; }

        public int Id { get; set; }

        //Prijs is required and must be between 0.01 and 10000.00
        [Required(ErrorMessage = "Prijs is verplicht.")]
        [Range(0.01, 10000.00, ErrorMessage = "Prijs moet tussen 0.01 en 10000.00 liggen.")]
        public float Prijs { get; set; }
    }
}