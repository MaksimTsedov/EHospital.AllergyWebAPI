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
    /// <summary>
    /// Controller for <see cref="BusinesLogic.Services.AllergySymptomService">
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]
    public class AllergySymptomsController : ControllerBase
    {        
        private readonly IAllergySymptomService _allergySymptom;

        /// <summary>
        /// Initializes a new instance of the <see cref="AllergySymptomsController"/> class.
        /// </summary>
        /// <param name="allergySymptom">The allergy symptom.</param>
        public AllergySymptomsController(IAllergySymptomService allergySymptom)
        {
            _allergySymptom = allergySymptom;
        }

        /// <summary>
        /// Gets all allergy symptoms by allergy id.
        /// </summary>
        /// <param name="allergyId">The allergy identifier.</param>
        /// <returns>HTTP response result.</returns>
        [HttpGet("allergyId={allegryId}")]
        public async Task<IActionResult> GetAllAllergySymptoms(int allergyId)
        {
            LoggingToFile.LoggingInfo("Receiving all symptoms of chosen patient allergy.");
            try
            {
                var symptoms = await _allergySymptom.GetAllAllergySymptoms(allergyId);
                LoggingToFile.LoggingInfo($"Successfully obtained {symptoms.Count()} symptoms.");
                return Ok(Mapper.Map<IEnumerable<SymptomView>>(symptoms));
            }
            catch (ArgumentException ex)
            {
                LoggingToFile.LoggingWarn(ex.Message, ex);
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Gets the allergy symptom by its id.
        /// </summary>
        /// <param name="allergySymptomId">The allergy-symptom pair identifier.</param>
        /// <returns>HTTP response result.</returns>
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

        /// <summary>
        /// Creates the allergy symptom from client.
        /// </summary>
        /// <param name="allergySymptom">The allergy symptom.</param>
        /// <returns>HTTP response result.</returns>
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
                LoggingToFile.LoggingInfo($"Successful create of allergy-symptom pair.");
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

        /// <summary>
        /// Deletes the allergy symptom by id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>HTTP response result.</returns>
        [HttpDelete("{id}")]
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
