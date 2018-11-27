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
    public class SymptomsController : ControllerBase
    {   
        private readonly ISymptomService _symptom;

        public SymptomsController(ISymptomService symptom)
        {
            _symptom = symptom;
        }

        [HttpGet]
        public IActionResult GetAllSymptoms()
        {
            LoggingToFile.LoggingInfo("Getting all symptoms.");
            var symptoms = _symptom.GetAllSymptoms();
            if (!symptoms.Any())
            {
                LoggingToFile.LoggingWarn("No symptom recorded.");
                return NotFound("No symptoms recorded.");
            }

            return Ok(Mapper.Map<IEnumerable<SymptomView>>(symptoms));
        }

        [HttpGet("searchKey={searchKey}", Name = "SearchSymptomQuery")]
        public async Task<IActionResult> GetAllSymptoms(string searchKey)
        {
            LoggingToFile.LoggingInfo($"Getting symptoms by search key \"{searchKey}\".");
            var symptoms = await _symptom.SearchSymptomsByName(searchKey);
            if (!symptoms.Any())
            {
                LoggingToFile.LoggingWarn($"No symptom found by \"{searchKey}\" search key.");
                return NotFound("No symptoms recorded.");
            }

            LoggingToFile.LoggingInfo($"Got {symptoms.Count()} symptoms with search key \"{searchKey}\".");
            return Ok(Mapper.Map<IEnumerable<SymptomView>>(symptoms));
        }

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

        [HttpPost]
        public async Task<IActionResult> CreateSymptom([FromBody]SymptomRequest symptom)
        {
            if (!(symptom is SymptomRequest))
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _symptom.CreateSymptomAsync(Mapper.Map<Symptom>(symptom));
                LoggingToFile.LoggingInfo($"Successfull create of {result.Naming} symptom.");
                return Created("symptoms", Mapper.Map<SymptomView>(result));
            }
            catch (ArgumentException ex)
            {
                LoggingToFile.LoggingError("Cannot insert symptom due to duplicate name.", ex);
                return Conflict(ex.Message);
            }
        }

        [HttpDelete]
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
