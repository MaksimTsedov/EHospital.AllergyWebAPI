using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EHospital.AllergyDA.Entities;
using EHospital.AllergyDomain;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using EHospital.AllergyAPI.Views;
using EHospital.AllergyDomain.Contracts;

namespace EHospital.AllergyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AllergySymptomsController : ControllerBase
    {
        IAllergySymptomRepository _allergySymptom;

        public AllergySymptomsController(IAllergySymptomRepository allergySymptom)
        {
            _allergySymptom = allergySymptom;
            AutoMapper.Mapper.Reset();
            Mapper.Initialize(cfg => {
                cfg.CreateMap<AllergySymptom, AllergySymptomView>();
                cfg.CreateMap<Symptom, AllergySymptomView>().
                ForMember(desc => desc.Symptom, opt => opt.MapFrom(c => c.Naming));
                cfg.CreateMap<AllergySymptom, AllergySymptomRequest>();
            });
        }

        [HttpGet("allegryId={id}")]
        public IActionResult GetAllAllergySymptoms(int allegryId)
        {
            try
            {
                var symptoms = _allergySymptom.GetAllAllergySymptoms(allegryId);
                return Ok(Mapper.Map<IEnumerable<SymptomView>>(symptoms));
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("id={id}", Name = "AllergySymptomById")]
        public IActionResult GetAllergySymptom(int id)
        {
            try
            {
                var allergySymptom = _allergySymptom.GetAllergySymptom(id);
                return Ok(Mapper.Map<AllergySymptomView>(allergySymptom));
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAllergySymptom([FromBody]AllergySymptomRequest allergySymptom)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _allergySymptom.CreateAllergySymptomAsync(Mapper.Map<AllergySymptom>(allergySymptom));
                    return Created("allergysymptoms", Mapper.Map<AllergySymptomView>(result));
                }
                catch (NullReferenceException ex)
                {
                    return NotFound(ex.Message);
                }
                catch (DuplicateWaitObjectException ex)
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
        public async Task<IActionResult> DeleteAllergySymptom(int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _allergySymptom.DeleteAllergySymptomAsync(id);
                    return Ok(Mapper.Map<AllergySymptomView>(result));
                }
                catch (NullReferenceException ex)
                {
                    return NotFound(ex.Message);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

    }
}
