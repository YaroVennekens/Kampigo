namespace Models
{
    // One to many relationship with destination and grouptravel
    // One to many relationship with destination and Foto.
    // - Destination can have many grouptravels.
    // - Destination can have many photo's.

    public class Bestemming
    {
        // Beschrijving with maximum 2500 character validation and is required
        [Required(ErrorMessage = "Beschrijving is verplicht.")]
        [StringLength(2500, ErrorMessage = "Beschrijving kan maximaal 2500 karakters bevatten.")]
        public string? Beschrijving { get; set; }

        //Alternate Key
        [Required(ErrorMessage = "Code is verplicht.")]
        [StringLength(50, ErrorMessage = "Beschrijving kan maximaal 50 karakters bevatten.")]
        public string Code { get; set; }

        public List<Foto>? Fotos { get; set; } = [];

        public List<Groepsreis>? Groepsreizen { get; set; } = [];

        // Unique ID
        [Key]
        public int Id { get; set; }

        // Maxleeftijd that must be between 0 and 200 and is required (More validation outside Model needed)
        [Required(ErrorMessage = "Maximum leeftijd is verplicht.")]
        [Range(0, 200, ErrorMessage = "Maximum leeftijd moet tussen 0 en 200 zijn.")]
        public int MaxLeeftijd { get; set; }

        // Maxleeftijd that must be between 0 and 200 and is required (More validation outside Model needed)
        [Required(ErrorMessage = "Minimum leeftijd is verplicht.")]
        [Range(0, 200, ErrorMessage = "Minimum leeftijd moet tussen 0 en 200 zijn.")]
        public int MinLeeftijd { get; set; }

        // Naam with maximum 100 character validation and is required
        [Required(ErrorMessage = "Naam is verplicht.")]
        [StringLength(100, ErrorMessage = "Naam kan maximaal 100 karakters bevatten.")]
        public string Naam { get; set; }

        public List<Review>? Reviews { get; set; } = [];
    }
}