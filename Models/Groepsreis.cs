namespace Models
{
    // One to many relationships with Groepsreis and Onkosten.
    // One to many relationships with Groepsreis and Bestemming

    // Many to many relationships with Groepsreis and Activiteit
    // Many to many relationships with Groepsreis and Kind
    // Many to many relationships with Groepsreis and User
    // ------------------------------------------------------------
    // Several expenses can be incurred for a group trip
    // A group trip has only one destination.

    // A group trip has several activities.
    // Multiple Users (that are monitor) can accompany you on a group trip
    // Several children can participate in a group trip
    public class Groepsreis
    {
        // Unique Groepsreis ID
        [Key]
        public int Id { get; set; }

        // Begindatum is required
        [Required(ErrorMessage = "Begindatum is verplicht.")]
        public DateTime Begindatum { get; set; } = DateTime.Now;

        // Bestemming navigation property
        [Required(ErrorMessage = "Bestemming is verplicht.")]
        public Bestemming? Bestemming { get; set; }

        // Bestemming foreign key
        [Required(ErrorMessage = "Bestemming is verplicht.")]
        public int BestemmingId { get; set; }

        // navigation property list of deelnemers
        public List<Deelnemer>? Deelnemers { get; set; } = [];

        // Einddatum is required
        [Required(ErrorMessage = "Einddatum is verplicht.")]
        public DateTime Einddatum { get; set; } = DateTime.Now.AddDays(1);

        [Required(ErrorMessage = "Archiefstatus is verplicht.")]
        public bool IsArchived { get; set; } = false;

        // navigation property list of monitors
        public List<Monitor>? Monitors { get; set; } = [];

        // navigation property list of onkosten
        public List<Onkosten>? Onkosten { get; set; } = [];

        //Prijs is required and must be between 0.01 and 10000.00
        [Required(ErrorMessage = "Prijs is verplicht.")]
        [Range(0.01, 10000.00, ErrorMessage = "Prijs moet tussen 0.01 en 10000.00 liggen.")]
        public float Prijs { get; set; }

        // navigation property list of programmas
        public List<Programma>? Programmas { get; set; } = [];
    }
}
