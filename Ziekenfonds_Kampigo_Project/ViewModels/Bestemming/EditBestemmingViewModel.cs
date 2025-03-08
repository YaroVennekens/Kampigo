using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using Ziekenfonds_Kampigo_Project.ViewModels.Foto;

namespace Ziekenfonds_Kampigo_Project.ViewModels.Bestemming
{
    public class EditBestemmingViewModel
    {
        // Beschrijving with maximum 2500 character validation and is required
        [Required(ErrorMessage = "Beschrijving is verplicht.")]
        [StringLength(2500, ErrorMessage = "Beschrijving kan maximaal 2500 karakters bevatten.")]
        public required string Beschrijving { get; set; }

        [BindNever]
        public List<FotoViewModel>? Fotos { get; set; } = [];

        public int Id { get; set; }

        // Maxleeftijd that must be between 6 and 21 and is required (More validation outside Model needed)
        [Required(ErrorMessage = "Maximum leeftijd is verplicht.")]
        [Range(6, 21, ErrorMessage = "Maximum leeftijd moet tussen 6 en 21 zijn.")]
        [DataType(DataType.Text, ErrorMessage = "Ongeldig type invoer. Vul een geheel getal in.")]
        public int MaxLeeftijd { get; set; }

        // Maxleeftijd that must be between 6 and 21 and is required (More validation outside Model needed)
        [Required(ErrorMessage = "Minimum leeftijd is verplicht.")]
        [Range(6, 21, ErrorMessage = "Minimum leeftijd moet tussen 6 en 21 zijn.")]
        [DataType(DataType.Text, ErrorMessage = "Ongeldig type invoer. Vul een geheel getal in.")]
        public int MinLeeftijd { get; set; }

        // Naam with maximum 100 character validation and is required
        [Required(ErrorMessage = "Naam is verplicht.")]
        [StringLength(100, ErrorMessage = "Naam kan maximaal 100 karakters bevatten.")]
        public required string Naam { get; set; }
    }
}