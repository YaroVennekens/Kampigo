namespace Ziekenfonds_Kampigo_Project.ViewModels.Groepsreis
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class OnkostenViewModel
    {
        public int Id { get; set; }

        public int GroepsreisId { get; set; }
        
        public global::Models.Groepsreis Groepsreis { get; set; }
        public List<OnkostenViewModel> Onkosten { get; set; } = new List<OnkostenViewModel>();
        
        public String Titel { get; set; }
        
        public String Foto { get; set; }
        
        public String Omschrijving { get; set; }
        
        public float Bedag { get; set; }
        
        public DateTime DatumOnkost { get; set; }
        
    }
}