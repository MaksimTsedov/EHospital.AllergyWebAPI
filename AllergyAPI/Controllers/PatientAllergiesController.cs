using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EHospital.AllergyDA.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using EHospital.AllergyAPI.Views;
using EHospital.AllergyDomain.Contracts;

namespace EHospital.AllergyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientAllergiesController : ControllerBase
    {
        IPatientAllergyRepository _patientAllergy;

        public PatientAllergiesController(IPatientAllergyRepository patientAllergy)
        {
            this._patientAllergy = patientAllergy;
            AutoMapper.Mapper.Reset();
            Mapper.Initialize(cfg => {
                cfg.CreateMap<PatientAllergy, PatientAllergyView>();
                cfg.CreateMap<PatientAllergyRequest, PatientAllergy>().ConvertUsing(arg =>
                {
                    return new PatientAllergy()
                    {
                        AllergyId = arg.AllergyId,
                        PatientId = arg.PatientId,
                        Duration = arg.Duration,
                        Notes = arg.Notes
                    };
                });
                cfg.CreateMap<PatientAllergy, PatientAllergyNotesView>().
                ForMember(desc => desc.Notes, opt => opt.MapFrom(c => c.Notes));
            });
        }

        [HttpGet("patientId={patientId}")]
        public IActionResult GetAllPatientAllergies(int patientId)
        {
            try
            {
                var allergies = _patientAllergy.GetAllPatientAllergies(patientId);
                return Ok(Mapper.Map<IEnumerable<AllergyView>>(allergies));
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("id={id}", Name = "PatientAllergyById")]
        public IActionResult GetPatientAllergy(int id)
        {
            try
            {
                var patientAllergy = _patientAllergy.GetPatientAllergy(id);
                return Ok(Mapper.Map<PatientAllergyView>(patientAllergy));
            }
            catch (NullReferenceException ex)
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
                catch (NullReferenceException ex)
                {
                    return NotFound(ex.Message);
                }
                catch (DuplicateWaitObjectException ex)
                {
                    return Conflict(ex.Message);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
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
                catch (NullReferenceException ex)
                {
                    return NotFound(ex.Message);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut("id={id}/notes={notes}", Name = "UpdateNotes")]
        public async Task<IActionResult> UpdatePatientAllergyNotes(int id, string notes)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _patientAllergy.UpdateNotesAsync(id, notes);
                    return Ok(Mapper.Map<PatientAllergyView>(result));
                }
                catch (NullReferenceException ex)
                {
                    return NotFound(ex.Message);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
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
                catch (NullReferenceException ex)
                {
                    return NotFound(ex.Message);
                }
                catch (Exception ex)
                {
                    return UnprocessableEntity(ex.Message);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

    }
}
