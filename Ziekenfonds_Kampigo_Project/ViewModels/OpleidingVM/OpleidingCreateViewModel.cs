using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Models;


namespace Ziekenfonds_Kampigo_Project.ViewModels.OpleidingVM
{
    public class OpleidingCreateViewModel : IValidatableObject
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Naam is verplicht.")]
        [StringLength(200, ErrorMessage = "Naam kan maximaal 200 karakters bevatten.")]
        public required string Naam { get; set; }

        [Required(ErrorMessage = "Beschrijving is verplicht.")]
        [StringLength(2500, ErrorMessage = "Beschrijving kan maximaal 2500 karakters bevatten.")]
        public required string Beschrijving { get; set; }

        [Required(ErrorMessage = "Begindatum is verplicht.")]
        [DataType(DataType.Date)]
        public DateTime BeginDatum { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Einddatum is verplicht.")]
        [DataType(DataType.Date)]
        public DateTime EindDatum { get; set; } = DateTime.Now.AddDays(1);

        [Required(ErrorMessage = "Aantal plaatsen is verplicht.")]
        [Range(1, int.MaxValue, ErrorMessage = "Aantal plaatsen moet groter zijn dan 0.")]
        public int AantalPlaatsen { get; set; }

        [Required(ErrorMessage = "Locatie is verplicht.")]
        [StringLength(200, ErrorMessage = "Locatie kan maximaal 200 karakters bevatten.")]
        public required string Locatie { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Opleiding Vereist moet een positief getal zijn.")]
        public int? OpleidingVereist { get; set; }

        public List<Opleiding>? Opleidingen { get; set; }

        // Helper function om te valideren dat eindDatum na beginDatum valt
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EindDatum <= BeginDatum)
            {
                yield return new ValidationResult(
                    "Einddatum moet na de begindatum zijn.",
                    new[] { nameof(EindDatum) });
            }
        }
    }
}
