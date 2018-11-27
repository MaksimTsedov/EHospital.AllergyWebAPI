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
        private readonly IPatientAllergyService _patientAllergy;

        public PatientAllergiesController(IPatientAllergyService patientAllergy)
        {
            this._patientAllergy = patientAllergy;
        }

        [HttpGet("patientId={patientId}")]
        public IActionResult GetAllPatientAllergies(int patientId)
        {
            var allergies = _patientAllergy.GetAllPatientAllergies(patientId);

            if (allergies.Count() != 0)
            {
                return Ok(Mapper.Map<IEnumerable<PatientAllergiesSymptomsView>>(allergies));
            }

            return NotFound("Not found any allergy of chosen patient.");
        }

        [HttpGet("id={id}", Name = "PatientAllergyById")]
        public IActionResult GetPatientAllergy(int id)
        {
            try
            {
                var patientAllergy = _patientAllergy.GetPatientAllergy(id);
                return Ok(Mapper.Map<PatientAllergyView>(patientAllergy));
            }
            catch (ArgumentNullException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePatientAllergy([FromBody]PatientAllergyRequest patientAllergy)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _patientAllergy.CreatePatientAllergyAsync(Mapper.Map<PatientAllergy>(patientAllergy));
                    return Created("patientallergies", Mapper.Map<PatientAllergyView>(result));
                }
                catch (ArgumentNullException ex)
                {
                    return NotFound(ex.Message);
                }
                catch (ArgumentException ex)
                {
                    return Conflict(ex.Message);
                }
            }

            return BadRequest(ModelState);
        }

        [HttpPut("id={id}", Name = "UpdatePatientAllergy")]
        public async Task<IActionResult> UpdatePatientAllergy(int id, [FromBody]PatientAllergyRequest patientAllergy)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _patientAllergy.UpdatePatientAllergyAsync(id, Mapper.Map<PatientAllergy>(patientAllergy));
                    return Ok(Mapper.Map<PatientAllergyView>(result));
                }
                catch (ArgumentNullException ex)
                {
                    return NotFound(ex.Message);
                }
            }

            return BadRequest(ModelState);
        }

        [HttpPut("idToUpdateNotes={idToUpdateNotes}", Name = "UpdateNotes")]
        public async Task<IActionResult> UpdatePatientAllergyNotes(int idToUpdateNotes, [FromBody]string notes)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _patientAllergy.UpdateNotesAsync(idToUpdateNotes, notes);
                    return Ok(Mapper.Map<PatientAllergyView>(result));
                }
                catch (ArgumentNullException ex)
                {
                    return NotFound(ex.Message);
                }
            }

            return BadRequest(ModelState);
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePatientAllergy(int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _patientAllergy.DeletePatientAllergyAsync(id);
                    return Ok("Cascade deleting success.");
                }
                catch (ArgumentNullException ex)
                {
                    return NotFound(ex.Message);
                }
            }

            return BadRequest(ModelState);
        }
    }
}
