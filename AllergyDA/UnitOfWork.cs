using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using EHospital.Allergies.Model;

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
        private AllergyDbContext _context = new AllergyDbContext();

        private Repository<Allergy> _allergyRepository;
        private Repository<Symptom> _symptomRepository;
        private Repository<PatientAllergy> _patientAllergyRepository;
        private Repository<AllergySymptom> _allergySymptomRepository;
        private Repository<PatientInfo> _patientInfoRepository;

        /// <summary>
        /// The disposed state
        /// </summary>
        private bool disposed = false;

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
                if (_allergyRepository == null)
                {
                    _allergyRepository = new Repository<Allergy>(_context);
                }

                return _allergyRepository;
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
                if (_symptomRepository == null)
                {
                    _symptomRepository = new Repository<Symptom>(_context);
                }

                return _symptomRepository;
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
                if (_patientAllergyRepository == null)
                {
                    _patientAllergyRepository = new Repository<PatientAllergy>(_context);
                }

                return _patientAllergyRepository;
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
                if (_allergySymptomRepository == null)
                {
                    _allergySymptomRepository = new Repository<AllergySymptom>(_context);
                }

                return _allergySymptomRepository;
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
                if (_patientInfoRepository == null)
                {
                    _patientInfoRepository = new Repository<PatientInfo>(_context);
                }

                return _patientInfoRepository;
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
