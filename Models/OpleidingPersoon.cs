namespace Models
{
    //  OpleidingPersoon must is assinged to one User
    //  OpleidingPersoon must be assigned to one Opleiding.
    public class OpleidingPersoon
    {
        // Unique OpleidingPersoon ID
        [Key]
        public int Id { get; set; }

        // Gebruiker navigation property
        [Required(ErrorMessage = "Gebruiker is verplicht.")]
        public CustomUser? Gebruiker { get; set; }

        // Foreign Key of Gebruiker
        [Required(ErrorMessage = "Gebruiker is verplicht.")]
        public required string GebruikerId { get; set; }

        // Indicates acceptance (default: false)
        public bool IsGeaccepteerd { get; set; } = false;

        // Opleiding navigation property
        [Required(ErrorMessage = "Opleiding is verplicht.")]
        public Opleiding? Opleiding { get; set; }

        // Foreign Key of Opleiding
        [Required(ErrorMessage = "Opleiding is verplicht.")]
        public int OpleidingId { get; set; }
    }
}
