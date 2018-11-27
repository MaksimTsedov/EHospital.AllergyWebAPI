using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using EHospital.Allergies.WebAPI.Views;
using EHospital.Allergies.BusinesLogic.Contracts;
using EHospital.Allergies.Model;
using System.Linq;
using EHospital.Logging;

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
            LoggingToFile.LoggingInfo("Receiving all symptoms of chosen patient allergy.");
            try
            {
                var symptoms = _allergySymptom.GetAllAllergySymptoms(allegryId);
                LoggingToFile.LoggingInfo($"Successfully obtained {symptoms.Count()} symptoms.");
                return Ok(Mapper.Map<IEnumerable<SymptomView>>(symptoms));
            }
            catch (ArgumentException ex)
            {
                LoggingToFile.LoggingWarn(ex.Message, ex);
                return NotFound(ex.Message);
            }
        }

        [HttpGet("allergySymptomId={allergySymptomId}", Name = "AllergySymptomById")]
        public async Task<IActionResult> GetAllergySymptom(int allergySymptomId)
        {
            LoggingToFile.LoggingInfo($"Getting allergy-symptom pair by id = {allergySymptomId}.");
            try
            {
                var allergySymptom = await _allergySymptom.GetAllergySymptom(allergySymptomId);
                LoggingToFile.LoggingInfo($"Got allergy-symptom pair by id = {allergySymptomId}.");
                return Ok(Mapper.Map<AllergySymptomView>(allergySymptom));
            }
            catch (ArgumentNullException ex)
            {
                LoggingToFile.LoggingWarn($"Such allergy symptom combination by id = {allergySymptomId} doesn`t exist.", ex);
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAllergySymptom([FromBody]AllergySymptomRequest allergySymptom)
        {           
            if (!(allergySymptom is AllergySymptomRequest))
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _allergySymptom.CreateAllergySymptomAsync(Mapper.Map<AllergySymptom>(allergySymptom));
                LoggingToFile.LoggingInfo($"Successfull create of allergy-symptom pair.");
                return Created("allergysymptoms", Mapper.Map<AllergySymptomView>(result));
            }
            catch (ArgumentNullException ex)
            {
                LoggingToFile.LoggingError($"Cannot create allergy-symptom pair due to {ex.Message}", ex);
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                LoggingToFile.LoggingError("Cannot create allergy-symptom pair due to its duplicate.", ex);
                return Conflict(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAllergySymptom(int id)
        {
            LoggingToFile.LoggingInfo("Deleting symptom of allergy of patient.");
            try
            {
                var result = await _allergySymptom.DeleteAllergySymptomAsync(id);
                LoggingToFile.LoggingInfo($"Allergy-symptom pair with {result.Id} id deleted successfully.");
                return Ok(Mapper.Map<AllergySymptomView>(result));
            }
            catch (ArgumentNullException ex)
            {
                LoggingToFile.LoggingError(ex.Message, ex);
                return NotFound(ex.Message);
            }
        }
    }
}
