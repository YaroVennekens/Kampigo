namespace Ziekenfonds_Kampigo_Project.ViewModels.Bestemming
{
    public class BestemmingDetailViewModel
    {
        public required string Naam { get; set; }
        public required string Beschrijving { get; set; }
        public required string Code { get; set; }
        public int MinLeeftijd { get; set; }
        public int MaxLeeftijd { get; set; }
    }
}
