using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using EHospital.Allergies.WebAPI.Views;
using EHospital.Allergies.BusinesLogic.Contracts;
using EHospital.Allergies.Model;

namespace EHospital.Allergies.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AllergySymptomsController : ControllerBase
    {
        private readonly IAllergySymptomService _allergySymptom;

        public AllergySymptomsController(IAllergySymptomService allergySymptom, ISymptomService symptoms)
        {
            _allergySymptom = allergySymptom;
        }

        [HttpGet("allergyId={allegryId}")]
        public IActionResult GetAllAllergySymptoms(int allegryId)
        {
            try
            {
                var symptoms = _allergySymptom.GetAllAllergySymptoms(allegryId);
                return Ok(Mapper.Map<IEnumerable<SymptomView>>(symptoms));
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("allergySymptomId={allergySymptomId}", Name = "AllergySymptomById")]
        public IActionResult GetAllergySymptom(int allergySymptomId)
        {
            try
            {
                var allergySymptom = _allergySymptom.GetAllergySymptom(allergySymptomId);
                return Ok(Mapper.Map<AllergySymptomView>(allergySymptom));
            }
            catch (ArgumentNullException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAllergySymptom([FromBody]AllergySymptomRequest allergySymptom)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _allergySymptom.CreateAllergySymptomAsync(Mapper.Map<AllergySymptom>(allergySymptom));
                    return Created("allergysymptoms", Mapper.Map<AllergySymptomView>(result));
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

        [HttpDelete]
        public async Task<IActionResult> DeleteAllergySymptom(int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _allergySymptom.DeleteAllergySymptomAsync(id);
                    return Ok(Mapper.Map<AllergySymptomView>(result));
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
