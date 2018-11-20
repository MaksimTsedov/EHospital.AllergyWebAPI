using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using EHospital.Allergies.WebAPI.Views;
using EHospital.Allergies.BusinesLogic.Contracts;
using EHospital.Allergies.Model;
using System.Linq;
using System.Collections.Generic;

namespace EHospital.Allergies.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientAllergiesController : ControllerBase
    {
        private static readonly log4net.ILog log = log4net.LogManager
                                                          .GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IPatientAllergyService _patientAllergy;

        public PatientAllergiesController(IPatientAllergyService patientAllergy)
        {
            this._patientAllergy = patientAllergy;
        }

        [HttpGet("patientId={patientId}")]
        public IActionResult GetAllPatientAllergies(int patientId)
        {
            log.Info($"Receiving allergies of patient with its symptoms by {patientId} Id.");
            var allergies = _patientAllergy.GetAllPatientAllergies(patientId);

            if (!allergies.Any())
            {
                log.Warn("Not found any allergy of chosen patient.");
                return NotFound("Not found any allergy of chosen patient.");
            }

            return Ok(Mapper.Map<IEnumerable<PatientAllergiesSymptomsView>>(allergies));
        }

        [HttpGet("id={id}", Name = "PatientAllergyById")]
        public IActionResult GetPatientAllergy(int id)
        {
            log.Info($"Getting patient-allergy pair by id = {id}.");
            try
            {
                var patientAllergy = _patientAllergy.GetPatientAllergy(id);
                log.Info($"Got patient-allergy pair by id = {id}.");
                return Ok(Mapper.Map<PatientAllergyView>(patientAllergy));
            }
            catch (ArgumentNullException ex)
            {
                log.Warn($"Such patient-allergy pair by id = {id} doesn`t exist.", ex);
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePatientAllergy([FromBody]PatientAllergyRequest patientAllergy)
        {
            log.Info("Assignment allergy to a patient.");
            if (ModelState.IsValid)
            {
                log.Error($"Invalid allergy assignment: {ModelState}.");
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _patientAllergy.CreatePatientAllergyAsync(Mapper.Map<PatientAllergy>(patientAllergy));
                log.Info("Successfull allergy assignment to a patient.");
                return Created("patientallergies", Mapper.Map<PatientAllergyView>(result));
            }
            catch (ArgumentNullException ex)
            {
                log.Error($"Cannot assign allergy to patient due to {ex.Message}", ex);
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                log.Error("Cannot assign allergy to patient due to its duplicate.", ex);
                return Conflict(ex.Message);
            }
        }

        [HttpPut("id={id}", Name = "UpdatePatientAllergy")]
        public async Task<IActionResult> UpdatePatientAllergy(int id, [FromBody]PatientAllergyRequest patientAllergy)
        {
            log.Info("Updating patient allergy information.");
            if (ModelState.IsValid)
            {
                log.Error($"Invalid patient-allergy pair updating: {ModelState}.");
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _patientAllergy.UpdatePatientAllergyAsync(id, Mapper.Map<PatientAllergy>(patientAllergy));
                log.Info("Successfull updating patient allergy information.");
                return Ok(Mapper.Map<PatientAllergyView>(result));
            }
            catch (ArgumentNullException ex)
            {
                log.Error(ex.Message, ex);
                return NotFound(ex.Message);
            }
        }

        [HttpPut("idToUpdateNotes={idToUpdateNotes}", Name = "UpdateNotes")]
        public async Task<IActionResult> UpdatePatientAllergyNotes(int idToUpdateNotes, [FromBody]string notes)
        {
            log.Info("Change notes about patient allergy.");
            if (ModelState.IsValid)
            {
                log.Error($"Invalid notes updating: {ModelState}.");
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _patientAllergy.UpdateNotesAsync(idToUpdateNotes, notes);
                log.Info("Rewriting of notes is successful.");
                return Ok(Mapper.Map<PatientAllergyView>(result));
            }
            catch (ArgumentNullException ex)
            {
                log.Error(ex.Message, ex);
                return NotFound(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePatientAllergy(int id)
        {
            log.Info("Deleting patient allergy.");
            try
            {
                await _patientAllergy.DeletePatientAllergyAsync(id);
                log.Info($"Patient-allergy pair with {id} id was deleted successfully.");
                return Ok("Cascade deleting success.");
            }
            catch (ArgumentNullException ex)
            {
                log.Error(ex.Message, ex);
                return NotFound(ex.Message);
            }
        }
    }
}
