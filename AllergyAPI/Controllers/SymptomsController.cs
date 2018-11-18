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
        private readonly ISymptomService _symptom;

        public SymptomsController(ISymptomService symptom)
        {
            _symptom = symptom;
        }

        [HttpGet]
        public IActionResult GetAllSymptoms()
        {
            var symptoms = _symptom.GetAllSymptoms();
            if (symptoms.Count() != 0)
            {
                return Ok(Mapper.Map<IEnumerable<SymptomView>>(symptoms));
            }

            return NotFound("No symptoms recorded.");
        }

        [HttpGet("searchKey={searchKey}", Name = "SearchSymptomQuery")]
        public IActionResult GetAllSymptoms(string searchKey)
        {
            var symptoms = _symptom.SearchSymptomsByName(searchKey);
            if (symptoms.Count() != 0)
            {
                return Ok(Mapper.Map<IEnumerable<SymptomView>>(symptoms));
            }

            return NotFound("No symptoms recorded.");
        }

        [HttpGet("{id}", Name = "SymptomById")]
        public IActionResult GetSymptom(int id)
        {
            try
            {
                var symptom = _symptom.GetSymptom(id);
                return Ok(Mapper.Map<SymptomView>(symptom));
            }
            catch (ArgumentNullException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateSymptom([FromBody]SymptomRequest symptom)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _symptom.CreateSymptomAsync(Mapper.Map<Symptom>(symptom));
                    return Created("symptoms", Mapper.Map<SymptomView>(result));
                }
                catch (ArgumentException ex)
                {
                    return Conflict(ex.Message);
                }
            }

            return BadRequest(ModelState);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteSymptom(int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    return Ok(Mapper.Map<SymptomView>(await _symptom.DeleteSymptomAsync(id)));
                }
                catch (ArgumentNullException ex)
                {
                    return NotFound(ex.Message);
                }
                catch (InvalidOperationException ex)
                {
                    return Conflict(ex.Message);
                }
            }

            return BadRequest(ModelState);
        }
    }
}
