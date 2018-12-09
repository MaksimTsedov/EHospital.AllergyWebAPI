using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EHospital.Allergies.BusinessLogic.Contracts;
using EHospital.Allergies.BusinessLogic.Services;
using EHospital.Allergies.Model;
using EHospital.Allergies.WebAPI.Views;
using EHospital.Logging;
using Microsoft.AspNetCore.Mvc;

namespace EHospital.Allergies.WebAPI.Controllers
{
    /// <summary>
    /// Controller for <see cref="SymptomService" />
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]
    public class SymptomsController : ControllerBase
    {   
        private readonly ISymptomService _symptom;

        /// <summary>
        /// Initializes a new instance of the <see cref="SymptomsController"/> class.
        /// </summary>
        /// <param name="symptom">The symptom.</param>
        public SymptomsController(ISymptomService symptom)
        {
            _symptom = symptom;
        }

        /// <summary>
        /// Gets all symptoms.
        /// </summary>
        /// <returns>HTTP response result.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllSymptoms()
        {
            LoggingToFile.LoggingInfo("Getting all symptoms.");
            var symptoms = await _symptom.GetAllSymptoms();
            if (!symptoms.Any())
            {
                LoggingToFile.LoggingWarn("No symptom recorded.");
                return NotFound("No symptoms recorded.");
            }

            return Ok(Mapper.Map<IEnumerable<SymptomView>>(symptoms));
        }

        /// <summary>
        /// Gets all symptoms by search key.
        /// </summary>
        /// <param name="searchKey">The search key.</param>
        /// <returns>HTTP response result.</returns>
        [HttpGet("searchKey={searchKey}", Name = "SearchSymptomQuery")]
        public async Task<IActionResult> GetAllSymptoms(string searchKey)
        {
            LoggingToFile.LoggingInfo($"Getting symptoms by search key \"{searchKey}\".");
            var symptoms = await _symptom.SearchSymptomsByName(searchKey);
            var symptomList = symptoms.ToList();
            if (!symptomList.Any())
            {
                LoggingToFile.LoggingWarn($"No symptom found by \"{searchKey}\" search key.");
                return NotFound("No symptoms recorded.");
            }

            LoggingToFile.LoggingInfo($"Got {symptomList.Count()} symptoms with search key \"{searchKey}\".");
            return Ok(Mapper.Map<IEnumerable<SymptomView>>(symptoms));
        }

        /// <summary>
        /// Gets the symptom by id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>HTTP response result.</returns>
        [HttpGet("{id}", Name = "SymptomById")]
        public async Task<IActionResult> GetSymptom(int id)
        {
            LoggingToFile.LoggingInfo($"Getting symptom by id = {id}.");
            try
            {
                var symptom = await _symptom.GetSymptom(id);
                LoggingToFile.LoggingInfo($"Got symptom by id = {id}.");
                return Ok(Mapper.Map<SymptomView>(symptom));
            }
            catch (ArgumentNullException ex)
            {
                LoggingToFile.LoggingWarn($"No symptom found by id = {id}.", ex);
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Creates the symptom.
        /// </summary>
        /// <param name="symptom">The symptom info got from client.</param>
        /// <returns>HTTP response result.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateSymptom([FromBody]SymptomRequest symptom)
        {
            if (symptom == null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _symptom.CreateSymptomAsync(Mapper.Map<Symptom>(symptom));
                LoggingToFile.LoggingInfo($"Successful create of {result.Naming} symptom.");
                return Created("symptoms", Mapper.Map<SymptomView>(result));
            }
            catch (ArgumentException ex)
            {
                LoggingToFile.LoggingError("Cannot insert symptom due to duplicate name.", ex);
                return Conflict(ex.Message);
            }
        }

        /// <summary>
        /// Deletes the symptom.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>HTTP response result.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSymptom(int id)
        {
            LoggingToFile.LoggingInfo("Deleting symptom.");
            try
            {
                var result = await _symptom.DeleteSymptomAsync(id);
                LoggingToFile.LoggingInfo($"{result.Naming} deleted successfully.");
                return Ok(Mapper.Map<SymptomView>(result));
            }
            catch (ArgumentNullException ex)
            {
                LoggingToFile.LoggingError(ex.Message, ex);
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                LoggingToFile.LoggingWarn(ex.Message, ex);
                return Conflict(ex.Message);
            }
        }
    }
}
