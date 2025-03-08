using Ziekenfonds_Kampigo_Project.ViewModels.Groepsreis;
using Ziekenfonds_Kampigo_Project.ViewModels.Kind;

namespace Ziekenfonds_Kampigo_Project.ViewModels.Deelnemer
{
    public class DeelnemerViewModel
    {
        public int Id { get; set; }

        public required KindViewModel Kind { get; set; }
        public string? Opmerking { get; set; }

        public string Naam { get; set; }

        public string Voornaam { get; set; }

    }
}
