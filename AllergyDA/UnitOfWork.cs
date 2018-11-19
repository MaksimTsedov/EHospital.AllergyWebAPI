using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using EHospital.Allergies.Model;
using System.Linq;
using System.Collections.Generic;

namespace EHospital.Allergies.Data
{
    
    // TODO: Add logger
    /// <summary>
    /// Unit of Work allows you to simplify work with different repositories
    /// and makes sure that all repositories will use the same data context.
    /// </summary>
    /// <seealso cref="Allergies.Data.IUnitOfWork" />
    public class UnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// The allergy context
        /// </summary>
        private static AllergyDbContext _context;

        // How about this approach?
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
        private bool disposed = false;

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
        /// Soft cascade delete patient-allergy pair.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public void CascadeDeletePatientAllergy(int id)
        {
            var procId = new SqlParameter("@Id", id);
            _context.Database.ExecuteSqlCommand("CascadeDeletePatientAllergy @Id", parameters: procId);
            
        }

        /// <summary>
        /// Saves this instance into db.
        /// </summary>
        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
