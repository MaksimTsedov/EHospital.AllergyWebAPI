using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using EHospital.Allergies.WebAPI.Views;
using EHospital.Allergies.BusinesLogic.Contracts;
using EHospital.Allergies.Model;
using System.Linq;
using System.Collections.Generic;
using EHospital.Logging;

namespace EHospital.Allergies.WebAPI.Controllers
{
    /// <summary>
    /// Controller for <see cref="BusinesLogic.Services.PatientAllergyService">
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]
    public class PatientAllergiesController : ControllerBase
    {
        private readonly IPatientAllergyService _patientAllergy;

        /// <summary>
        /// Initializes a new instance of the <see cref="PatientAllergiesController"/> class.
        /// </summary>
        /// <param name="patientAllergy">The patient allergy.</param>
        public PatientAllergiesController(IPatientAllergyService patientAllergy)
        {
            this._patientAllergy = patientAllergy;
        }

        /// <summary>
        /// Gets all patient allergies by patient id.
        /// </summary>
        /// <param name="patientId">The patient identifier.</param>
        /// <returns>HTTP response result.</returns>
        [HttpGet("patientId={patientId}")]
        public async Task<IActionResult> GetAllPatientAllergies(int patientId)
        {
            LoggingToFile.LoggingInfo($"Receiving allergies of patient with its symptoms by {patientId} Id.");
            var allergies = await _patientAllergy.GetAllPatientAllergies(patientId);

            if (!allergies.Any())
            {
                LoggingToFile.LoggingWarn("Not found any allergy of chosen patient.");
                return NotFound("Not found any allergy of chosen patient.");
            }

            return Ok(Mapper.Map<IEnumerable<PatientAllergiesSymptomsView>>(allergies));
        }

        /// <summary>
        /// Gets the patient-allergy record by id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>HTTP response result.</returns>
        [HttpGet("id={id}", Name = "PatientAllergyById")]
        public async Task<IActionResult> GetPatientAllergy(int id)
        {
            LoggingToFile.LoggingInfo($"Getting patient-allergy pair by id = {id}.");
            try
            {
                var patientAllergy = await _patientAllergy.GetPatientAllergy(id);
                LoggingToFile.LoggingInfo($"Got patient-allergy pair by id = {id}.");
                return Ok(Mapper.Map<PatientAllergyView>(patientAllergy));
            }
            catch (ArgumentNullException ex)
            {
                LoggingToFile.LoggingWarn($"Such patient-allergy pair by id = {id} doesn`t exist.", ex);
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Creates the patient allergy.
        /// </summary>
        /// <param name="patientAllergy">The patient-allergy data from client.</param>
        /// <returns>HTTP response result.</returns>
        [HttpPost]
        public async Task<IActionResult> CreatePatientAllergy([FromBody]PatientAllergyRequest patientAllergy)
        {
            if (!(patientAllergy is PatientAllergyRequest))
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _patientAllergy.CreatePatientAllergyAsync(Mapper.Map<PatientAllergy>(patientAllergy));
                LoggingToFile.LoggingInfo("Successful allergy assignment to a patient.");
                return Created("patientallergies", Mapper.Map<PatientAllergyView>(result));
            }
            catch (ArgumentNullException ex)
            {
                LoggingToFile.LoggingError($"Cannot assign allergy to patient due to {ex.Message}", ex);
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                LoggingToFile.LoggingError("Cannot assign allergy to patient due to its duplicate.", ex);
                return Conflict(ex.Message);
            }
        }

        /// <summary>
        /// Updates the patient allergy info.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="patientAllergy">The patient allergy new info.</param>
        /// <returns>HTTP response result.</returns>
        [HttpPut("id={id}", Name = "UpdatePatientAllergy")]
        public async Task<IActionResult> UpdatePatientAllergy(int id, [FromBody]PatientAllergyUpdateRequest patientAllergy)
        {            
            if (!(patientAllergy is PatientAllergyUpdateRequest))
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _patientAllergy.UpdatePatientAllergyAsync(id, Mapper.Map<PatientAllergy>(patientAllergy));
                LoggingToFile.LoggingInfo("Successful updating patient allergy information.");
                return Ok(Mapper.Map<PatientAllergyView>(result));
            }
            catch (ArgumentNullException ex)
            {
                LoggingToFile.LoggingError(ex.Message, ex);
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                LoggingToFile.LoggingError("Cannot update allergy to patient due to its duplicate.", ex);
                return Conflict(ex.Message);
            }
        }

        /// <summary>
        /// Updates the patient allergy notes.
        /// </summary>
        /// <param name="idToUpdateNotes">The identifier of patient allergy record.</param>
        /// <param name="notes">The notes.</param>
        /// <returns>HTTP response result.</returns>
        [HttpPut("idToUpdateNotes={idToUpdateNotes}", Name = "UpdateNotes")]
        public async Task<IActionResult> UpdatePatientAllergyNotes(int idToUpdateNotes, [FromBody]string notes)
        {
            LoggingToFile.LoggingInfo("Change notes about patient allergy.");
            if (notes.Length > 500)
            {
                LoggingToFile.LoggingError("Invalid build of PatientAllergyUpdateRequest :"
                                       + Environment.NewLine
                                       + "Notes : Too long note, please shorten it."
                                       + Environment.NewLine);
                return BadRequest();
            }

            try
            {
                var result = await _patientAllergy.UpdateNotesAsync(idToUpdateNotes, notes);
                LoggingToFile.LoggingInfo("Rewriting of notes is successful.");
                return Ok(notes);
            }
            catch (ArgumentNullException ex)
            {
                LoggingToFile.LoggingError(ex.Message, ex);
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Deletes the patient allergy record.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>HTTP response result.</returns>
        [HttpDelete]
        public async Task<IActionResult> DeletePatientAllergy(int id)
        {
            LoggingToFile.LoggingInfo("Deleting patient allergy.");
            try
            {
                await _patientAllergy.DeletePatientAllergyAsync(id);
                LoggingToFile.LoggingInfo($"Patient-allergy pair with {id} id was deleted successfully.");
                return Ok("Cascade deleting success.");
            }
            catch (ArgumentNullException ex)
            {
                LoggingToFile.LoggingError(ex.Message, ex);
                return NotFound(ex.Message);
            }
        }
    }
}
