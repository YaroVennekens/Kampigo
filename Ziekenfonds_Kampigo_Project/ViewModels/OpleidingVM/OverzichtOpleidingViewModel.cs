using System.ComponentModel.DataAnnotations;

namespace Ziekenfonds_Kampigo_Project.ViewModels.OpleidingVM
{
    public class OverzichtOpleidingenViewModel
    {
        public int Id { get; set; }
        public string Naam { get; set; }
        public string Beschrijving { get; set; }
        public DateTime BeginDatum { get; set; }
        public DateTime EindDatum { get; set; }
        public int AantalPlaatsen { get; set; }
        public string Locatie { get; set; }
        public int? OpleidingVereist { get; set; }
        public bool IsAangeraden { get; set; } = false;
        public bool IsBezig { get; set; } = false;
        public bool IsUitgebreid { get; set; } = false;
        public bool IsVoltooid { get; set; } = false;
        public bool IsBijnaVol { get; set; } = false;
        public string? VereisteOpleidingNaam { get; set; }
    }
}
