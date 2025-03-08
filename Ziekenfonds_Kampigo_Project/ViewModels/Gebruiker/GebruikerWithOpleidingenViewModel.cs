using Ziekenfonds_Kampigo_Project.ViewModels.OpleidingVM;

namespace Ziekenfonds_Kampigo_Project.ViewModels.Gebruiker
{
    public class GebruikerWithOpleidingenViewModel
    {
        public required GebruikerViewmodel Gebruiker { get; set; }
        public List<OpleidingViewModel>? Opleidingen { get; set; } = []; 
    }
}
