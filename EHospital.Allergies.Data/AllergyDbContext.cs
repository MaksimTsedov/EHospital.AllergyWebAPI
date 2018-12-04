using EHospital.Allergies.Model;
using Microsoft.EntityFrameworkCore;

namespace EHospital.Allergies.Data
{
    /// <summary>
    /// Database context for allergy service
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.DbContext" />
    public class AllergyDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AllergyDbContext"/> class.
        /// </summary>
        /// <param name="options">The options for this context.</param>
        public AllergyDbContext(DbContextOptions options) : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the allergies set.
        /// </summary>
        /// <value>
        /// The allergies.
        /// </value>
        public virtual DbSet<Allergy> Allergies { get; set; }

        /// <summary>
        /// Gets or sets the allergy-symptom pairs.
        /// </summary>
        /// <value>
        /// The allergy symptoms.
        /// </value>
        public virtual DbSet<AllergySymptom> AllergySymptoms { get; set; }

        /// <summary>
        /// Gets or sets the patient-allergy pairs.
        /// </summary>
        /// <value>
        /// The patient allergies.
        /// </value>
        public virtual DbSet<PatientAllergy> PatientAllergies { get; set; }

        /// <summary>
        /// Gets or sets the symptoms.
        /// </summary>
        /// <value>
        /// The symptoms.
        /// </value>
        public virtual DbSet<Symptom> Symptoms { get; set; }

        /// <summary>
        /// Gets or sets the patients.
        /// </summary>
        /// <value>
        /// The patients.
        /// </value>
        public virtual DbSet<PatientInfo> PatientInfo { get; }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PatientAllergy>()
                .HasOne(pa => pa.Allergy)
                .WithMany(a => a.PatientAllergies)
                .HasForeignKey(pa => pa.AllergyId);

            modelBuilder.Entity<PatientAllergy>()
                .HasOne(pa => pa.Patient)
                .WithMany(p => p.PatientAllergies)
                .HasForeignKey(pa => pa.PatientId);

            modelBuilder.Entity<AllergySymptom>()
               .HasOne(als => als.PatientAllergy)
               .WithMany(pa => pa.AllergySymptoms)
               .HasForeignKey(als => als.PatientAllergyId);

            modelBuilder.Entity<AllergySymptom>()
                .HasOne(als => als.Symptom)
                .WithMany(s => s.AllergySymptoms)
                .HasForeignKey(als => als.SymptomId);

            modelBuilder.Entity<Symptom>().HasQueryFilter(s => !s.IsDeleted);
            modelBuilder.Entity<AllergySymptom>().HasQueryFilter(s => !s.IsDeleted);
            modelBuilder.Entity<Allergy>().HasQueryFilter(s => !s.IsDeleted);
            modelBuilder.Entity<PatientAllergy>().HasQueryFilter(s => !s.IsDeleted);
        }
    }
}
