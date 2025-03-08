namespace Models
{   // One to many relationship with Photo and Destination.
    // Photo must have one Destination
    public class Foto
    {
        // Unique Id
        [Key]
        public int Id { get; set; }

        // Bestemming navigation property
        [Required(ErrorMessage = "Bestemming is verplicht.")]
        public Bestemming? Bestemming { get; set; }
        

        // Foreign Key of Bestemming
        [Required(ErrorMessage = "Bestemming is verplicht.")]
        public int BestemmingId { get; set; }

        // Name with maximum 500 character validation and is required
        [Required(ErrorMessage = "Naam is verplicht.")]
        [StringLength(500, ErrorMessage = "Naam kan maximaal 500 karakters bevatten.")]
        public required string Naam { get; set; }
    }
}