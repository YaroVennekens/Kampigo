using System.ComponentModel.DataAnnotations;
using Ziekenfonds_Kampigo_Project.ViewModels.Monitor;

namespace Ziekenfonds_Kampigo_Project.ViewModels.OpleidingVM
{
    public class OpleidingMetMonitorsViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Naam is verplicht.")]
        public required string Naam { get; set; }

        [Required(ErrorMessage = "Aantal plaatsen is verplicht.")]
        public required int AantalPlaatsen { get; set; }

        public List<MonitorBijOpleidingViewModel>? IngeschrevenMonitors { get; set; }
        public List<MonitorBijOpleidingViewModel>? GeaccepteerdeMonitors { get; set; }
        public List<MonitorBijOpleidingViewModel>? NietIngeschrevenMonitors { get; set; }
    }
}
