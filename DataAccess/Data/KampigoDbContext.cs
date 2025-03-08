using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;
using Monitor = Models.Monitor;

namespace DataAccess.Data
{
    public class KampigoDbContext : IdentityDbContext<IdentityUser>
    {
        
        public KampigoDbContext(DbContextOptions<KampigoDbContext> options)
            : base(options) { }
        
        
        // DB Sets
        public DbSet<Activiteit> Activiteiten { get; set; }
        public DbSet<Bestemming> Bestemmingen { get; set; }
        public DbSet<Deelnemer> Deelnemers { get; set; }
        public DbSet<Foto> Fotos { get; set; }
        public DbSet<Groepsreis> Groepreizen { get; set; }
        public DbSet<Kind> Kinderen { get; set; }
        public DbSet<Monitor> Monitors { get; set; }
        public DbSet<Onkosten> Onkosten { get; set; }
        public DbSet<Opleiding> Opleidingen  { get; set; }
        public DbSet<OpleidingPersoon> OpleidingPersonen { get; set; }
        public DbSet<Programma> Programmas { get; set; }
        public DbSet<Review> Reviews { get; set; }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);
            
            // Table names in database
            modelBuilder.Entity<Activiteit>().ToTable("Activiteit");
            modelBuilder.Entity<Bestemming>().ToTable("Bestemming");
            modelBuilder.Entity<Deelnemer>().ToTable("Deelnemer");
            modelBuilder.Entity<Foto>().ToTable("Foto");
            modelBuilder.Entity<Groepsreis>().ToTable("Groepreis").Property(p=>p.Prijs).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Kind>().ToTable("Kind");
            modelBuilder.Entity<Monitor>().ToTable("Monitor");
            modelBuilder.Entity<Onkosten>().ToTable("Onkost").Property(p=>p.Bedrag).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Opleiding>().ToTable("Opleiding");
            modelBuilder.Entity<OpleidingPersoon>().ToTable("OpleidingPersoon");
            modelBuilder.Entity<Programma>().ToTable("Programma");
            modelBuilder.Entity<Review>().ToTable("Review");
            
            // Relation R1:  DTC
            modelBuilder.Entity<Programma>()
                .HasOne(programma => programma.Activiteit)
                .WithMany(activiteit => activiteit.Programmas)
                .HasForeignKey(programma => programma.ActiviteitId)
                .OnDelete(DeleteBehavior.Cascade); 
            
            // Relation R2: DTC
            modelBuilder.Entity<Programma>()
                .HasOne(programma => programma.Groepsreis)
                .WithMany(groepsreis => groepsreis.Programmas)
                .HasForeignKey(programma => programma.GroepsreisId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // Relation R3: DTR

            modelBuilder.Entity<Groepsreis>()
                .HasOne(groepreis => groepreis.Bestemming)
                .WithMany(bestemming => bestemming.Groepsreizen)
                .HasForeignKey(groepreis => groepreis.BestemmingId)
                .OnDelete(DeleteBehavior.Restrict); 


            // Relation R4: DTC
            modelBuilder.Entity<Foto>()
                .HasOne(foto => foto.Bestemming)
                .WithMany(bestemming => bestemming.Fotos)
                .HasForeignKey(foto => foto.BestemmingId)
                .OnDelete(DeleteBehavior.Cascade); 
            
            // Relation R5: DTR
            modelBuilder.Entity<Onkosten>()
                .HasOne(onkost => onkost.Groepsreis)
                .WithMany(groepreis => groepreis.Onkosten)
                .HasForeignKey(onkost => onkost.GroepsreisId)
                .OnDelete(DeleteBehavior.Restrict); 
            
            // Relation R6: DTC
            modelBuilder.Entity<Deelnemer>()
                .HasOne(deelnemer => deelnemer.Groepsreis)
                .WithMany(groepreis => groepreis.Deelnemers)
                .HasForeignKey(deelnemer => deelnemer.GroepsreisDetailsId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // Relation R7: DTR
            modelBuilder.Entity<Monitor>()
                .HasOne(monitor => monitor.Groepsreis)
                .WithMany(groepreis => groepreis.Monitors)
                .HasForeignKey(monitor => monitor.GroepsreisDetailsId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // Relation R8: DTC
            modelBuilder.Entity<Deelnemer>()
                .HasOne(deelnemer => deelnemer.Kind)
                .WithMany(kind => kind.Deelnemers)
                .HasForeignKey(deelnemer => deelnemer.KindId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // Relation R9 : DTN
            modelBuilder.Entity<Opleiding>()
                .HasOne(o => o.VoorwaardeOpleiding)  
                .WithMany(o => o.AfhankelijkeOpleidingen)  
                .HasForeignKey(o => o.OpleidingVereist) 
                .OnDelete(DeleteBehavior.NoAction); 
            
            
            
            // Relation R10: DTR
            modelBuilder.Entity<Monitor>()
                .HasOne(monitor => monitor.Gebruiker)
                .WithMany(gebruiker => gebruiker.Monitors)
                .HasForeignKey(monitor => monitor.GebruikerId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // Relation R11: DTC
            modelBuilder.Entity<Kind>()
                .HasOne(kind => kind.Gebruiker)
                .WithMany(gebruiker => gebruiker.Kinderen)
                .HasForeignKey(kind => kind.GebruikerId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // Relation R12: DTR
            modelBuilder.Entity<OpleidingPersoon>()
                .HasOne(opleidingPersoon => opleidingPersoon.Opleiding)
                .WithMany(opleiding => opleiding.OpleidingPersonen)
                .HasForeignKey(opleidingPersoon => opleidingPersoon.OpleidingId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // Relation R13: DTC
            modelBuilder.Entity<OpleidingPersoon>()
                .HasOne(opleidingPersoon => opleidingPersoon.Gebruiker)
                .WithMany(gebruiker => gebruiker.OpleidingPersonen)
                .HasForeignKey(opleidingPersoon => opleidingPersoon.GebruikerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relation R14: DTC
            modelBuilder.Entity<Review>()
                .HasOne(review => review.Bestemming)
                .WithMany(bestemming => bestemming.Reviews)
                .HasForeignKey(review => review.BestemmingId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // Relation R15 DTC
            modelBuilder.Entity<Review>()
                .HasOne(review => review.Persoon)
                .WithMany(persoon => persoon.Reviews)
                .HasForeignKey(review => review.PersoonId)
                .OnDelete(DeleteBehavior.Cascade);

        }  
        
    }
}
