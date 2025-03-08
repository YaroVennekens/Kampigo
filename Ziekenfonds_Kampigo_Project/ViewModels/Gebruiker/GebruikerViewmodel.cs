
using System.ComponentModel.DataAnnotations;

namespace Ziekenfonds_Kampigo_Project.ViewModels.Gebruiker
{
    public class GebruikerViewmodel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Naam is verplicht.")]
        public string Naam { get; set; }

        [Required(ErrorMessage = "Voornaam is verplicht.")]
        public string Voornaam { get; set; }

        [Required(ErrorMessage = "Email is verplicht.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Geboortedatum is verplicht.")]
        public DateTime GeboorteDatum { get; set; }


        public bool IsActief { get; set; }
        public bool IsMonitor { get; set; }

        [Required(ErrorMessage = "Rol is verplicht.")]
        public string Role { get; set; }

        [Required(ErrorMessage = "Telefoonnummer is verplicht.")]
        public string TelefoonNummer { get; set; }

        [Required(ErrorMessage = "Gemeente is verplicht.")]
        public string Gemeente { get; set; }

        [Required(ErrorMessage = "Straat is verplicht.")]
        public string Straat { get; set; }

        [Required(ErrorMessage = "Postcode is verplicht.")]
        public string Postcode { get; set; }

        [Required(ErrorMessage = "Huisnummer is verplicht.")]
        public string Huisnummer { get; set; }

        [Required(ErrorMessage = "Geboortedatum is verplicht.")]
        public DateTime Geboortedatum { get; set; }
    }
}