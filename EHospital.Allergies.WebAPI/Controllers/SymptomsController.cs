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
    public class SymptomsController : ControllerBase
    {
        private static readonly log4net.ILog log = log4net.LogManager
                                                          .GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ISymptomService _symptom;

        public SymptomsController(ISymptomService symptom)
        {
            _symptom = symptom;
        }

        [HttpGet]
        public IActionResult GetAllSymptoms()
        {
            log.Info("Getting all symptoms.");
            var symptoms = _symptom.GetAllSymptoms();
            if (!symptoms.Any())
            {
                log.Warn("No symptom recorded.");
                return NotFound("No symptoms recorded.");
            }

            return Ok(Mapper.Map<IEnumerable<SymptomView>>(symptoms));
        }

        [HttpGet("searchKey={searchKey}", Name = "SearchSymptomQuery")]
        public IActionResult GetAllSymptoms(string searchKey)
        {
            log.Info($"Getting symptoms by search key \"{searchKey}\".");
            var symptoms = _symptom.SearchSymptomsByName(searchKey);
            if (!symptoms.Any())
            {
                log.Warn($"No symptom found by \"{searchKey}\" search key.");
                return NotFound("No symptoms recorded.");
            }

            log.Info($"Got {symptoms.Count()} symptoms with search key \"{searchKey}\".");
            return Ok(Mapper.Map<IEnumerable<SymptomView>>(symptoms));
        }

        [HttpGet("{id}", Name = "SymptomById")]
        public IActionResult GetSymptom(int id)
        {
            log.Info($"Getting symptom by id = {id}.");
            try
            {
                var symptom = _symptom.GetSymptom(id);
                log.Info($"Got symptom by id = {id}.");
                return Ok(Mapper.Map<SymptomView>(symptom));
            }
            catch (ArgumentNullException ex)
            {
                log.Warn($"No symptom found by id = {id}.", ex);
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateSymptom([FromBody]SymptomRequest symptom)
        {
            log.Info("Creating symptom.");
            if (!ModelState.IsValid)
            {
                log.Error($"Invalid build of symptom: {ModelState}.");
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _symptom.CreateSymptomAsync(Mapper.Map<Symptom>(symptom));
                log.Info($"Successfull create of {result.Naming} symptom.");
                return Created("symptoms", Mapper.Map<SymptomView>(result));
            }
            catch (ArgumentException ex)
            {
                log.Error("Cannot insert symptom due to duplicate name.", ex);
                return Conflict(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteSymptom(int id)
        {
            log.Info("Deleting symptom.");
            try
            {
                var result = await _symptom.DeleteSymptomAsync(id);
                log.Info($"{result.Naming} deleted successfully.");
                return Ok(Mapper.Map<SymptomView>(result));
            }
            catch (ArgumentNullException ex)
            {
                log.Error(ex.Message, ex);
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                log.Warn(ex.Message, ex);
                return Conflict(ex.Message);
            }
        }
    }
}
