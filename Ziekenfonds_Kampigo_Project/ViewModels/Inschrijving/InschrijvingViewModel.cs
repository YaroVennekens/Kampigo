using AutoMapper.Configuration.Annotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ziekenfonds_Kampigo_Project.ViewModels.Groepsreis;
using Ziekenfonds_Kampigo_Project.ViewModels.Kind;

namespace Ziekenfonds_Kampigo_Project.ViewModels.Inschrijving
{
    public class InschrijvingViewModel
    {

        public int GroepsreisDetailsId { get; set; }

        public List<SelectListItem> KinderenOfUser { get; set; } = new List<SelectListItem>();

        public List<KindNameViewModel> KindNamen { get; set; } = new List<KindNameViewModel>();

        // Selected children IDs
        [Required(ErrorMessage = "Selecteer ten minste één kind.")]
        public List<int> SelectedKindIds { get; set; } = new List<int>();

        // Opmerking
        [StringLength(1000, ErrorMessage = "Opmerking kan maximaal 1000 karakters bevatten.")]
        public string? Opmerking { get; set; }
    }

}
