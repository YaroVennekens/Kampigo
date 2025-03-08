using Microsoft.AspNetCore.Identity;

namespace Models
{
	// Custom validation attribute to ensure Geboortedatum is not in the future
	public class MaxDateAttribute : ValidationAttribute
	{
		public override bool IsValid(object? value)
		{
			if (value is DateTime dateTime)
			{
				return dateTime <= DateTime.Now;
			}
			return false;
		}
	}

	// A user can follow multiple courses.
	// A monitor (user) can supervise multiple group trips.
	// A user can have multiple children.
	public class CustomUser: IdentityUser
    {
        // Naam with maximum 100 character validation, is required and is marked as personalData
        [Required(ErrorMessage = "Naam is verplicht.")]
        [StringLength(100, ErrorMessage = "Naam kan maximaal 100 karakters bevatten.")]
        [PersonalData]
        public required string Naam { get; set; }

        // Voornaam with maximum 100 character validation, is required and is marked as personalData
        [Required(ErrorMessage = "Voornaam is verplicht.")]
        [StringLength(100, ErrorMessage = "Voornaam kan maximaal 100 karakters bevatten.")]
        [PersonalData]
        public required string Voornaam { get; set; }

        // Straat with maximum 100 character validation, is required and is marked as personalData
        [Required(ErrorMessage = "Straat is verplicht.")]
        [StringLength(100, ErrorMessage = "Straat kan maximaal 100 karakters bevatten.")]
        [PersonalData]
        public required string Straat { get; set; }

        // Huisnummer with maximum 10 character validation, is required and is marked as personalData
        [Required(ErrorMessage = "Huisnummer is verplicht.")]
        [StringLength(10, ErrorMessage = "Huisnummer kan maximaal 10 karakters bevatten.")]
        [PersonalData]
        public required string Huisnummer { get; set; }

        // Gemeente with maximum 50 character validation, is required and is marked as personalData
        [Required(ErrorMessage = "Gemeente is verplicht.")]
        [StringLength(100, ErrorMessage = "Gemeente kan maximaal 100 karakters bevatten.")]
        [PersonalData]
        public required string Gemeente { get; set; }

        // Postcode with maximum 10 character validation, is required and is marked as personalData
        [Required(ErrorMessage = "Postcode is verplicht.")]
        [StringLength(10, ErrorMessage = "Postcode kan maximaal 10 karakters bevatten.")]
        [PersonalData]
        public required string Postcode { get; set; }

        // Geboortedatum is required and is marked as personalData
        [Required(ErrorMessage = "Geboortedatum is verplicht.")]
		[MaxDate(ErrorMessage = "Geboortedatum kan niet in de toekomst liggen.")]
		[PersonalData]
        public DateTime GeboorteDatum { get; set; } = DateTime.Now.AddYears(-12);

        // Huisdokter with maximum 100 character validation and is required
        [Required(ErrorMessage = "Huisdokter is verplicht.")]
        [StringLength(100, ErrorMessage = "Huisdokter kan maximaal 100 karakters bevatten.")]
        public required string Huisdokter { get; set; }

        // Contract with maximum 50 character validation
        [StringLength(50, ErrorMessage = "Contract nummer kan maximaal 50 karakters bevatten.")]
        public string? ContractNummer { get; set; }

        public bool IsMonitor { get; set; } = false;

        // Telefoon with maximum 20 character validation, is required and is marked as personalData
        [Required(ErrorMessage = "Telefoonnummer is verplicht.")]
        [StringLength(20, ErrorMessage = "Telefoon kan maximaal 20 karakters bevatten.")]
        [PersonalData]
        public required string TelefoonNummer { get; set; }

        // Rekeningnummer with maximum 30 character validation and marked as personalData
        [StringLength(30, ErrorMessage = "Rekeningnummer kan maximaal 30 karakters bevatten.")]
        [PersonalData]
        public string? RekeningNummer { get; set; }

        // IsActief with required validation
        [Required(ErrorMessage = "Is actief is verplicht.")]
        public bool IsActief { get; set; } = true;

        // Navigation property of opleidingPersoon
        public List<OpleidingPersoon>? OpleidingPersonen { get; set; } = [];

        // Navigation property of Monitors
        public List<Monitor>? Monitors { get; set; } = [];

        // Navigation property of Kinderen
        public List<Kind>? Kinderen { get; set; } = [];

        public List<Review>? Reviews { get; set; } = [];
    }
}