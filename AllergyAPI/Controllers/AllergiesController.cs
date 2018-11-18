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
    public class AllergiesController : ControllerBase
    {
        private readonly IAllergyService _allergy;

        public AllergiesController(IAllergyService allergy)
        {
            _allergy = allergy;
        }

        [HttpGet]
        public IActionResult GetAllAllergies()
        {
            var allergies = _allergy.GetAllAllergies();
            if (allergies.Count() != 0)
            {
                return Ok(Mapper.Map<IEnumerable<AllergyView>>(allergies));
            }

            return NotFound("No allergy recorded.");
        }

        [HttpGet("searchKey={searchKey}", Name = "SearchAllergyQuery")]
        public IActionResult GetAllAllergies(string searchKey)
        {
            var allergies = _allergy.SearchAllergiesByName(searchKey);
            if (allergies.Count() != 0)
            {
                return Ok(Mapper.Map<IEnumerable<AllergyView>>(allergies));
            }

            return NotFound("No allergy recorded.");
        }

        [HttpGet("{id}", Name = "AllergyById")]
        public IActionResult GetAllergy(int id)
        {
            try
            {
                var allergy = _allergy.GetAllergy(id);
                return Ok(Mapper.Map<AllergyView>(allergy));
            }
            catch (ArgumentNullException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAllergy([FromBody]AllergyRequest allergy)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _allergy.CreateAllergyAsync(Mapper.Map<Allergy>(allergy));
                    return Created("allergies", Mapper.Map<AllergyView>(result));
                }
                catch (ArgumentException ex)
                {
                    return Conflict(ex.Message);
                }
            }

            return BadRequest(ModelState);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAllergy(int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _allergy.DeleteAllergyAsync(id);
                    return Ok(Mapper.Map<AllergyView>(result));
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
