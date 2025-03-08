using System.ComponentModel.DataAnnotations;
using Models;
using Ziekenfonds_Kampigo_Project.ViewModels.Activiteit;
using Ziekenfonds_Kampigo_Project.ViewModels.Foto;

namespace Ziekenfonds_Kampigo_Project.ViewModels.Groepsreis
{
    public class GroepsreisDetailViewModel
    {
      
            public int Id { get; set; }
            public List<ActiviteitDetailViewModel>? Activiteiten { get; set; }
            public DateTime? BeginDatum { get; set; }
            public required string Beschrijving { get; set; }
            public DateTime? EindDatum { get; set; }
            public List<FotoViewModel>? Fotos { get; set; }
            public int? MaxLeeftijd { get; set; }
            public int? MinLeeftijd { get; set; }
            public required string Naam { get; set; }
            public float? Prijs { get; set; }

            [Required(ErrorMessage = "The UserId field is required.")]
            public required string UserId { get; set; }

            public int AantalDeelnemers { get; set; }
        
    }
}