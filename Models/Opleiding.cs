namespace Models
{   // Multiple monitors can participate in a training course.
    // One course may be required for multiple courses. (Recursive relation)
    // Opleiding must is assinged to one opleidingVereiste.
    public class Opleiding
    {
        // Unique Opleiding ID
        [Key]
        public int Id { get; set; }

        // Naam with maximum 200 character validation and is required
        [Required(ErrorMessage = "Naam is verplicht.")]
        [StringLength(200, ErrorMessage = "Naam kan maximaal 200 karakters bevatten.")]
        public required string Naam { get; set; }

        // Beschrijving with maximum 2500 character validation and is required
        [Required(ErrorMessage = "Beschrijving is verplicht.")]
        [StringLength(2500, ErrorMessage = "Beschrijving kan maximaal 2500 karakters bevatten.")]
        public required string Beschrijving { get; set; }

        // BeginDatum is required
        [Required(ErrorMessage = "BeginDatum is verplicht.")]
        public DateTime BeginDatum { get; set; } = DateTime.Now;

        // EindDatum is required
        [Required(ErrorMessage = "EindDatum is verplicht.")]
        public DateTime EindDatum { get; set; } = DateTime.Now.AddDays(1);

        // AantalPlaatsen is required and must be a positive number
        [Required(ErrorMessage = "AantalPlaatsen is verplicht.")]
        [Range(1, int.MaxValue, ErrorMessage = "Aantal plaatsen moet een positief getal zijn.")]
        public int AantalPlaatsen { get; set; }

        // OpleidingVereist must be a positive number
        [Range(1, int.MaxValue, ErrorMessage = "Opleiding Vereist moet een positief getal zijn.")]
        public int? OpleidingVereist { get; set; }

        // Locatie with maximum 200 character validation and is required
        [StringLength(200, ErrorMessage = "Locatie kan maximaal 200 karakters bevatten.")]
        public required string Locatie { get; set; }

        // Navigation property for the Opleidingen that require this Opleiding as a prerequisite
        public List<Opleiding>? Opleidingen { get; set; }
        
        public List<OpleidingPersoon>? OpleidingPersonen  { get; set; }
       
        public virtual Opleiding? VoorwaardeOpleiding { get; set; } 

        public virtual List<Opleiding>? AfhankelijkeOpleidingen { get; set; } 
    }
}