namespace Models
{
	public class Review
	{
		// Unique Opleiding ID
		[Key]
		public int Id { get; set; }

		[Required(ErrorMessage = "Persoon is verplicht.")]
        public CustomUser? Persoon { get; set; }

		[Required(ErrorMessage = "Persoon is verplicht.")]
		public required string PersoonId { get; set; }

		[Required(ErrorMessage = "Bestemming is verplicht.")]
        public Bestemming? Bestemming { get; set; }

        [Required(ErrorMessage = "Bestemming is verplicht.")]
        public int BestemmingId { get; set; }

		// Review text with maximum 5000 character validation and is not required
		[StringLength(5000, ErrorMessage = "Tekst kan maximaal 5000 karakters bevatten.")]
		public string? Tekst { get; set; }

		[Required(ErrorMessage = "Score is verplicht.")]
		[Range(0, 5, ErrorMessage = "Score moet tussen 0 en 5 liggen.")]
		public int Score { get; set; }
	}
}
