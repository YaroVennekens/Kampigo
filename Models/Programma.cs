namespace Models
{   //  Programma must is assigned to one Activiteit
    //  Programma must be assigned to one Groepsreis.
    public class Programma
	{
        // Unique Deelnemer ID
        [Key]
		public int Id { get; set; }

		// Activiteit navigation property
		[Required(ErrorMessage = "Activiteit is verplicht.")]
        public Activiteit? Activiteit { get; set; }

        // Activiteit foreign key
        [Required(ErrorMessage = "Activiteit is verplicht.")]
        public int ActiviteitId { get; set; }

        // Groepsreis navigation property
        [Required(ErrorMessage = "Groepsreis is verplicht.")]
        public Groepsreis? Groepsreis { get; set; }

        // Groepsreis foreign key
        [Required(ErrorMessage = "Groepsreis is verplicht.")]
        public int GroepsreisId { get; set; }
    }
}