using Ziekenfonds_Kampigo_Project.ViewModels.Gebruiker;
using Ziekenfonds_Kampigo_Project.ViewModels.OpleidingVM;
using Ziekenfonds_Kampigo_Project.ViewModels.Groepsreis;

namespace Ziekenfonds_Kampigo_Project.ViewModels.Monitor
{
    public class MonitorViewModel
    {
        public DateTime ToegewezenOp { get; set; }  
        public bool IsToegewezen { get; set; }
        public bool IsHoofdmonitor { get; set; }
        public required GebruikerViewmodel Gebruiker { get; set; }
        public List<OpleidingViewModel>? Opleidingen { get; set; } = [];
        public List<GroepsreisViewModel> Groepsreizen { get; set; }
    }
}
