namespace Ziekenfonds_Kampigo_Project.ViewModels.OpleidingVM
{
    public class OpleidingBijMonitorViewModel
    {
        public int Id { get; set; }
        public string Naam { get; set; }
        public string Beschrijving { get; set; }
        public DateTime BeginDatum { get; set; }
        public DateTime EindDatum { get; set; }
        public int AantalPlaatsen { get; set; }
        public int? OpleidingVereist { get; set; }
        public bool IsVoltooid { get; set; }
        public string Status { get; set; }
    }
}
