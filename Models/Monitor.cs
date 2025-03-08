namespace Models
{
    //  Monitor must is assinged to one User
    //  Monitor must be assigned to one groupsreis.
    public class Monitor
    {
        // Unique Monitor ID
        [Key]
        public int Id { get; set; }

        // Gebruiker navigation property
        [Required(ErrorMessage = "Gebruiker is verplicht.")]
        public CustomUser? Gebruiker { get; set; }

        // Foreign Key of Gebruiker
        [Required(ErrorMessage = "Gebruiker is verplicht.")]
        public required string GebruikerId { get; set; }

        // Groepsreis navigation property
        [Required(ErrorMessage = "Groepsreis is verplicht.")]
        public Groepsreis? Groepsreis { get; set; }

        // Foreign Key of Groepsreis
        [Required(ErrorMessage = "Groepsreis is verplicht.")]
        public int GroepsreisDetailsId { get; set; }

        [Required(ErrorMessage = "Status van monitor is verplicht.")]
        public bool IsHoofdMonitor { get; set; } = false;

        [Required(ErrorMessage = "Toewijzing van monitor is verplicht.")]
        public bool IsToegewezen { get; set; } = false;
    }
}
