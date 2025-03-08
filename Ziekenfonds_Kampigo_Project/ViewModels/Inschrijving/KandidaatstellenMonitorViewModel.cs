using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Ziekenfonds_Kampigo_Project.ViewModels.Inschrijving
{
    public class KandidaatstellenMonitorViewModel
    {
        [Required]
        public int GroepsreisId { get; set; }

        [Required]
        public required string UserId { get; set; }
        public bool IsHoofdMonitor { get; set; } = false;
        public bool IsToegewezen { get; set; } = false;
    }
}
