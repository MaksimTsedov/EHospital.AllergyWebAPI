using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EHospital.AllergyDA.Contracts;
using EHospital.AllergyDA.Entities;
using EHospital.AllergyDA.Repository;
using Microsoft.AspNetCore.Mvc;

namespace EHospital.AllergyAPI.Controllers
{
    // TODO: add logic to EHospital.AllergyDomain
    [Route("api/[controller]")]
    [ApiController]
    public class AllergiesController : ControllerBase
    {
        IUnitOfWork _unitOfWork;

        public AllergiesController()
        {
            _unitOfWork = new UnitOfWork();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAllergies()
        {
            var allergies = await _unitOfWork.Allergies.GetAll();
            return Ok(allergies.OrderBy(a => a.Pathogen));
        }

        [HttpGet("{beginning}", Name = "SearchQuery")]
        public async Task<IActionResult> GetAllAllergies(string beginning)
        {
            var allergies = await _unitOfWork.Allergies.GetAll(a => a.Pathogen.StartsWith(beginning));
            if (allergies.Count() == 0)
            {
                return NotFound(allergies);
            }

            return Ok(allergies.OrderBy(a => a.Pathogen));
        }

        [HttpGet("{id}", Name = "AllergyById")]
        public async Task<IActionResult> GetAllergy(int id)
        {
            var allergy = await _unitOfWork.Allergies.GetEntity(id);
            if (allergy == null)
            {
                return NotFound(allergy);
            }

            return Ok(allergy);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAllergy([FromBody]Allergy allergy)
        {
            if (ModelState.IsValid)
            {
                var result = _unitOfWork.Allergies.InsertEntity(allergy);
                await _unitOfWork.Save();
                return Created("allergies", result);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAllergy(int id, [FromBody]Allergy allergy)
        {
            if (ModelState.IsValid)
            {
                var result = await _unitOfWork.Allergies.GetEntity(id);
                if (result == null)
                {
                    return NotFound(result);
                }
                var updatedAllergy = _unitOfWork.Allergies.UpdateEntity(result);               
                await _unitOfWork.Save();
                return Ok(updatedAllergy);
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
                var result = await _unitOfWork.Allergies.GetEntity(id);
                if (result == null)
                {
                    return NotFound(result);
                }
                var deletedAllergy = _unitOfWork.Allergies.DeleteEntity(result);
                await _unitOfWork.Save();
                return Ok(deletedAllergy);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

    }
}
