namespace Ziekenfonds_Kampigo_Project.ViewModels.OpleidingVM
{
    public class OpleidingViewModel
    {
        public int Id { get; set; }
        public required string Naam { get; set; }
        public required string Beschrijving { get; set; }
        public DateTime BeginDatum { get; set; }
        public DateTime EindDatum { get; set; }
        public required string Locatie { get; set; }
        public int? OpleidingVereist { get; set; }
        public bool IsVoltooid { get; set; }
        public bool IsUitgebreid { get; set; }
        public bool IsBezig { get; set; }
        public bool IsGeaccepteerd { get; set; }
    }
}