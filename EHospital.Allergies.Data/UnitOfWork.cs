using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using EHospital.Allergies.Model;
using Microsoft.EntityFrameworkCore;

namespace EHospital.Allergies.Data
{
    /// <inheritdoc />
    /// <summary>
    /// Unit of Work allows you to simplify work with different repositories
    /// and makes sure that all repositories will use the same data context.
    /// </summary>
    /// <seealso cref="!:Allergies.Data.IUnitOfWork" />
    public sealed class UnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// The allergy context
        /// </summary>
        private static AllergyDbContext _context;

        private readonly Lazy<Repository<Allergy>> _allergyRepository 
                   = new Lazy<Repository<Allergy>>(() => new Repository<Allergy>(_context));
        private readonly Lazy<Repository<Symptom>> _symptomRepository 
                   = new Lazy<Repository<Symptom>>(() => new Repository<Symptom>(_context));
        private readonly Lazy<Repository<PatientAllergy>> _patientAllergyRepository
                   = new Lazy<Repository<PatientAllergy>>(() => new Repository<PatientAllergy>(_context));
        private readonly Lazy<Repository<AllergySymptom>> _allergySymptomRepository
                   = new Lazy<Repository<AllergySymptom>>(() => new Repository<AllergySymptom>(_context));
        private readonly Lazy<Repository<PatientInfo>> _patientInfoRepository
                   = new Lazy<Repository<PatientInfo>>(() => new Repository<PatientInfo>(_context));

        /// <summary>
        /// The disposed state
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="context">The allergy context.</param>
        public UnitOfWork(AllergyDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets the allergies.
        /// </summary>
        /// <value>
        /// The allergies.
        /// </value>
        public IRepository<Allergy> Allergies
        {
            get
            {
                return _allergyRepository.Value;
            }
        }

        /// <summary>
        /// Gets the symptoms.
        /// </summary>
        /// <value>
        /// The symptoms.
        /// </value>
        public IRepository<Symptom> Symptoms
        {
            get
            {
                return _symptomRepository.Value;
            }
        }

        /// <summary>
        /// Gets the patient allergies.
        /// </summary>
        /// <value>
        /// The patient allergies.
        /// </value>
        public IRepository<PatientAllergy> PatientAllergies
        {
            get
            {
                return _patientAllergyRepository.Value;
            }
        }

        /// <summary>
        /// Gets the allergy symptoms.
        /// </summary>
        /// <value>
        /// The allergy symptoms.
        /// </value>
        public IRepository<AllergySymptom> AllergySymptoms
        {
            get
            {
                return _allergySymptomRepository.Value;
            }
        }

        /// <summary>
        /// Gets the patient information repository.
        /// </summary>
        /// <value>
        /// The patient information repository.
        /// </value>
        public IRepository<PatientInfo> PatientInfo
        {
            get
            {
                return _patientInfoRepository.Value;
            }
        }

        /// <summary>
        /// Soft cascade delete patient-allergy pair. Related on stored procedure.
        /// </summary>
        /// <param name="id">The identifier of patient-allergy pair.</param>
        public void CascadeDeletePatientAllergy(int id)
        {
            var procId = new SqlParameter("@Id", id);
            _context.Database.ExecuteSqlCommand("CascadeDeletePatientAllergy @Id", parameters: procId);
            
        }

        /// <summary>
        /// Saves changes into db.
        /// </summary>
        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            UnitOfWork obj = this;
            // ReSharper disable once GCSuppressFinalizeForTypeWithoutDestructor
            GC.SuppressFinalize(obj);
        }
    }
}
