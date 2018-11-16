using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EHospital.Allergies.DAL.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using EHospital.Allergies.WebAPI.Views;
using EHospital.Allergies.Domain.Contracts;

namespace EHospital.Allergies.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AllergySymptomsController : ControllerBase
    {
        IAllergySymptomRepository _allergySymptom;
        ISymptomRepository _symptoms; // Can I load this repo with data for mapping?

        public AllergySymptomsController(IAllergySymptomRepository allergySymptom, ISymptomRepository symptoms)
        {
            _allergySymptom = allergySymptom;
            _symptoms = symptoms;
            AutoMapper.Mapper.Reset();
            Mapper.Initialize(cfg => {
                cfg.CreateMap<AllergySymptom, AllergySymptomView>().
                                ForMember(dest => dest.SymptomName, opt => opt.MapFrom
                                (src => _symptoms.GetSymptom(src.SymptomId).Naming));
                cfg.CreateMap<AllergySymptomRequest, AllergySymptom>().ConvertUsing(arg =>
                {
                    return new AllergySymptom()
                    {
                        PatientAllergyId = arg.PatientAllergyId,
                        SymptomId = arg.SymptomId
                    };
                });
            });
        }

        [HttpGet("allergyId={allegryId}")]
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
