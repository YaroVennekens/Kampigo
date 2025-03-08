namespace Models
{
    //  Deelnemer must is assinged to one Kind
    //  Deelnemer must be assigned to one Groepsreis.
    public class Deelnemer
    {
        // Unique Deelnemer ID
        [Key]
        public int Id { get; set; }

        // Kind navigation Property
        [Required(ErrorMessage = "Kind is verplicht.")]
        public Kind Kind { get; set; }

        // Kind foreign key
        [Required(ErrorMessage = "Kind is verplicht.")]
        public int KindId { get; set; }


        // Groepsreis navigation property
        [Required(ErrorMessage = "Groepsreis is verplicht.")]
        public Groepsreis? Groepsreis { get; set; }

        // Groepsreis foreign key
        [Required(ErrorMessage = "Groepsreis is verplicht.")]
        public int GroepsreisDetailsId { get; set; }

        // Opmerkingen with maximum 1000 character validation 
        [StringLength(1000, ErrorMessage = "Opmerking kan maximaal 1000 karakters bevatten.")]
        public string? Opmerking { get; set; }
    }
}