using Models;
using System.ComponentModel.DataAnnotations;
using Ziekenfonds_Kampigo_Project.ViewModels.Gebruiker;

namespace Ziekenfonds_Kampigo_Project.ViewModels.Kind
{
    public class KindViewModel
    {
        public int Id { get; set; }

        public GebruikerViewmodel? Ouder { get; set; }

        // Naam with maximum 100 character validation and is required
        [Required(ErrorMessage = "Naam is verplicht.")]
        [StringLength(100, ErrorMessage = "Naam kan maximaal 100 karakters bevatten.")]
        public string? Naam { get; set; }

        // Voornaam with maximum 100 character validation and is required
        [Required(ErrorMessage = "Voornaam is verplicht.")]
        [StringLength(100, ErrorMessage = "Voornaam kan maximaal 100 karakters bevatten.")]
        public string? Voornaam { get; set; }

        // Geboortedatum is required
        [Required(ErrorMessage = "Geboortedatum is verplicht.")]
        [MaxDate(ErrorMessage = "Geboortedatum kan niet in de toekomst liggen.")]
        public DateTime Geboortedatum { get; set; }

        // Allergieën with maximum 200 character validation
        [StringLength(200, ErrorMessage = "Allergieën kan maximaal 200 karakters bevatten.")]
        [Required(ErrorMessage = "Allergieën invullen is verplicht.")]
        public string? Allergieen { get; set; }

        // Medicatie with maximum 500 character validation
        [StringLength(500, ErrorMessage = "Medicatie kan maximaal 500 karakters bevatten.")]
        [Required(ErrorMessage = "Medicatie invullen is verplicht.")]
        public string? Medicatie { get; set; }
    }
}
