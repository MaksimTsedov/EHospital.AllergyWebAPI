using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using EHospital.Allergies.WebAPI.Views;
using EHospital.Allergies.BusinesLogic.Contracts;
using EHospital.Allergies.Model;
using System.Linq;

namespace EHospital.Allergies.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AllergySymptomsController : ControllerBase
    {
        private static readonly log4net.ILog log = log4net.LogManager
                                                          .GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IAllergySymptomService _allergySymptom;

        public AllergySymptomsController(IAllergySymptomService allergySymptom, ISymptomService symptoms)
        {
            _allergySymptom = allergySymptom;
        }

        [HttpGet("allergyId={allegryId}")]
        public IActionResult GetAllAllergySymptoms(int allegryId)
        {
            log.Info("Receiving all symptoms of chosen patient allergy.");
            try
            {
                var symptoms = _allergySymptom.GetAllAllergySymptoms(allegryId);
                log.Info($"Successfully obtained {symptoms.Count()} symptoms.");
                return Ok(Mapper.Map<IEnumerable<SymptomView>>(symptoms));
            }
            catch (ArgumentException ex)
            {
                log.Warn(ex.Message, ex);
                return NotFound(ex.Message);
            }
        }

        [HttpGet("allergySymptomId={allergySymptomId}", Name = "AllergySymptomById")]
        public IActionResult GetAllergySymptom(int allergySymptomId)
        {
            log.Info($"Getting allergy-symptom pair by id = {allergySymptomId}.");
            try
            {
                var allergySymptom = _allergySymptom.GetAllergySymptom(allergySymptomId);
                log.Info($"Got allergy-symptom pair by id = {allergySymptomId}.");
                return Ok(Mapper.Map<AllergySymptomView>(allergySymptom));
            }
            catch (ArgumentNullException ex)
            {
                log.Warn($"Such allergy symptom combination by id = {allergySymptomId} doesn`t exist.", ex);
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAllergySymptom([FromBody]AllergySymptomRequest allergySymptom)
        {
            log.Info("Assignment symptom to allergy of patient.");
            if (ModelState.IsValid)
            {
                log.Error($"Invalid symptom assignment: {ModelState}.");
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _allergySymptom.CreateAllergySymptomAsync(Mapper.Map<AllergySymptom>(allergySymptom));
                log.Info($"Successfull create of allergy-symptom pair.");
                return Created("allergysymptoms", Mapper.Map<AllergySymptomView>(result));
            }
            catch (ArgumentNullException ex)
            {
                log.Error($"Cannot create allergy-symptom pair due to {ex.Message}", ex);
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                log.Error("Cannot create allergy-symptom pair due to its duplicate.", ex);
                return Conflict(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAllergySymptom(int id)
        {
            log.Info("Deleting symptom of allergy of patient.");
            try
            {
                var result = await _allergySymptom.DeleteAllergySymptomAsync(id);
                log.Info($"Allergy-symptom pair with {result.Id} id deleted successfully.");
                return Ok(Mapper.Map<AllergySymptomView>(result));
            }
            catch (ArgumentNullException ex)
            {
                log.Error(ex.Message, ex);
                return NotFound(ex.Message);
            }
        }
    }
}
