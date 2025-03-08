namespace Models
{
    // One to many relationship with Onkosten and Groepsreis.
    // An expense can only belong to one group trip.
    public class Onkosten
    {
        // Unique Onkosten ID
        [Key]
        public int Id { get; set; }

        // Titel with maximum 250 character validation and is required
        [Required(ErrorMessage = "Titel is verplicht.")]
        [StringLength(250, ErrorMessage = "Titel kan maximaal 250 karakters bevatten.")]
        public required string Titel { get; set; }

        // Omschrijving with maximum 500 character validation and is required
        [Required(ErrorMessage = "Omschrijving is verplicht.")]
        [StringLength(500, ErrorMessage = "Omschrijving kan maximaal 500 karakters bevatten.")]
        public required string Omschrijving { get; set; }

        // Bedrag is required
        // Min bedrag : 0.01
        // Max bedrag : 50000.00
        [Required(ErrorMessage = "Bedrag is verplicht.")]
        [Range(0.01, 50000.00, ErrorMessage = "Bedrag moet tussen 0.01 en 50000.00 liggen.")]
        public float Bedrag { get; set; }

        // Date is required
        [Required(ErrorMessage = "Datum is verplicht.")]
        public DateTime Datum { get; set; } = DateTime.Now;


		[StringLength(500, ErrorMessage = "Foto url kan maximaal 500 karakters bevatten.")]
		public string? Foto { get; set; }

        // Groepsreis navigation property
        [Required(ErrorMessage = "Groepsreis is verplicht.")]
        public Groepsreis? Groepsreis { get; set; }

		// Groepsreis ID
		public int GroepsreisId { get; set; }
		
		
    }
}