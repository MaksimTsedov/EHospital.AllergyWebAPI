using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EHospital.AllergyDA.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using EHospital.AllergyAPI.Views;
using EHospital.AllergyDomain.Contracts;

namespace EHospital.AllergyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AllergiesController : ControllerBase
    {
        IAllergyRepository _allergy;

        public AllergiesController(IAllergyRepository allergy)
        {
            _allergy = allergy;
            AutoMapper.Mapper.Reset();
            Mapper.Initialize(cfg => {
                cfg.CreateMap<Allergy, AllergyView>(MemberList.None);
                cfg.CreateMap<AllergyRequest, Allergy>().ConvertUsing(arg =>
                {
                    return new Allergy()
                    {
                        Pathogen = arg.Pathogen
                    };
                });
            });
        }

        [HttpGet]
        public IActionResult GetAllAllergies()
        {
            try
            {
                var allergies = _allergy.GetAllAllergies();                
                return Ok(Mapper.Map<IEnumerable<AllergyView>>(allergies));
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("beginning={beginning}", Name = "SearchAllergyQuery")]
        public IActionResult GetAllAllergies(string beginning)
        {
            try
            {
                var allergies = _allergy.SearchAllergiesByName(beginning);
                return Ok(Mapper.Map<IEnumerable<AllergyView>>(allergies));
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }            
        }

        [HttpGet("id={id}", Name = "AllergyById")]
        public IActionResult GetAllergy(int id)
        {
            try
            {
                var allergy = _allergy.GetAllergy(id);
                return Ok(Mapper.Map<AllergyView>(allergy));
            }
            catch (NullReferenceException ex)
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
            else
            {
                return BadRequest(ModelState);
            }
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
                catch (NullReferenceException ex)
                {
                    return NotFound(ex.Message);
                }
                catch (ArgumentException ex)
                {
                    return Conflict(ex.Message);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

    }
}
