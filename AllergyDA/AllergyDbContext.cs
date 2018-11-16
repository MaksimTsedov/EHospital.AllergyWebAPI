using Microsoft.EntityFrameworkCore;
using EHospital.Allergies.DAL.Entities;

namespace EHospital.Allergies.DAL
{
    /// <summary>
    /// Database context for allergy
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.DbContext" />
    public class AllergyDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AllergyDbContext"/> class.
        /// </summary>
        public AllergyDbContext()
        {
        }

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
        /// Gets or sets the allergy symptoms.
        /// </summary>
        /// <value>
        /// The allergy symptoms.
        /// </value>
        public virtual DbSet<AllergySymptom> AllergySymptoms { get; set; }

        /// <summary>
        /// Gets or sets the patient allergies.
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Initial Catalog=EHospital;Integrated Security=True");
            }
        }
    }
}
