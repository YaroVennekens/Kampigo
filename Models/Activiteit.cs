namespace Models
{
    // Many to Many relationship with Activiteit and Groepsreis
    // Activity can be done in many group travels
    public class Activiteit
    {
        // Unique activiteit ID
        [Key]
        public int Id { get; set; }

        // beschrijving with maximum 5000 character validation and is required
        [Required(ErrorMessage = "Beschrijving is verplicht.")]
        [StringLength(5000, ErrorMessage = "Beschrijving kan maximaal 5000 karakters bevatten.")]
        public required string Beschrijving { get; set; }

        // Naam with maxium 250 character validation and is required
        [Required(ErrorMessage = "Naam is verplicht.")]
        [StringLength(250, ErrorMessage = "Naam kan maximaal 250 karakters bevatten.")]
        public required string Naam { get; set; }

        // Activeit can have no or multiple programs
        public List<Programma>? Programmas { get; set; }
    }
}