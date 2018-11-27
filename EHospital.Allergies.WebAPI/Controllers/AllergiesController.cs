using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using EHospital.Allergies.WebAPI.Views;
using EHospital.Allergies.BusinesLogic.Contracts;
using EHospital.Allergies.Model;
using System.Linq;
using System.Reflection;

namespace EHospital.Allergies.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AllergiesController : ControllerBase
    {
        private static readonly log4net.ILog log = log4net.LogManager
                                                          .GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IAllergyService _allergy;

        public AllergiesController(IAllergyService allergy)
        {
            _allergy = allergy;
        }

        [HttpGet]
        public IActionResult GetAllAllergies()
        {
            log.Info("Getting all allergies.");
            var allergies = _allergy.GetAllAllergies();
            if (!allergies.Any())
            {
                log.Warn("No allergy recorded.");
                return NotFound("No allergy recorded.");
            }

            return Ok(Mapper.Map<IEnumerable<AllergyView>>(allergies));
        }

        [HttpGet("searchKey={searchKey}", Name = "SearchAllergyQuery")]
        public async Task<IActionResult> GetAllAllergies(string searchKey)
        {
            log.Info($"Getting allergies by search key \"{searchKey}\".");
            var allergies = await _allergy.SearchAllergiesByName(searchKey);
            if (!allergies.Any())
            {
                log.Warn($"No allergy found by \"{searchKey}\" search key.");
                return NotFound("No allergy recorded.");
            }

            log.Info($"Got {allergies.Count()} allergies with search key \"{searchKey}\".");
            return Ok(Mapper.Map<IEnumerable<AllergyView>>(allergies));
        }

        [HttpGet("{id}", Name = "AllergyById")]
        public async Task<IActionResult> GetAllergy(int id)
        {
            log.Info($"Getting allergy by id = {id}.");
            try
            {
                var allergy = await _allergy.GetAllergy(id);
                log.Info($"Got allergy by id = {id}.");
                return Ok(Mapper.Map<AllergyView>(allergy));
            }
            catch (ArgumentNullException ex)
            {
                log.Warn($"No allergy found by id = {id}.", ex);
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAllergy([FromBody]AllergyRequest allergy)
        {
            if (!(allergy is AllergyRequest))
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _allergy.CreateAllergyAsync(Mapper.Map<Allergy>(allergy));
                log.Info($"Successfull create of {result.Pathogen} allergy.");
                return Created("allergies", Mapper.Map<AllergyView>(result));
            }
            catch (ArgumentException ex)
            {
                log.Error("Cannot insert allergy due to duplicate name.", ex);
                return Conflict(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAllergy(int id)
        {
            log.Info("Deleting allergy.");

            try
            {
                var result = await _allergy.DeleteAllergyAsync(id);
                log.Info($"{result.Pathogen} deleted successfully.");
                return Ok(Mapper.Map<AllergyView>(result));
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
