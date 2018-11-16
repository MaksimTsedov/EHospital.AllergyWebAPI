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
    public class SymptomsController : ControllerBase
    {
        ISymptomRepository _symptom;

        public SymptomsController(ISymptomRepository symptom)
        {
            _symptom = symptom;
            AutoMapper.Mapper.Reset();
            Mapper.Initialize(cfg => {
                cfg.CreateMap<Symptom, SymptomView>();
                cfg.CreateMap<SymptomRequest, Symptom>().ConvertUsing(arg =>
                {
                    return new Symptom()
                    {
                        Naming = arg.Naming
                    };
                });
            });
        }

        [HttpGet]
        public IActionResult GetAllSymptoms()
        {
            try
            {
                var symptoms = _symptom.GetAllSymptoms();
                return Ok(Mapper.Map<IEnumerable<SymptomView>>(symptoms));
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("beginning={beginning}", Name = "SearchSymptomQuery")]
        public IActionResult GetAllSymptoms(string beginning)
        {
            try
            {
                var symptoms = _symptom.SearchSymptomsByName(beginning);
                return Ok(Mapper.Map<IEnumerable<SymptomView>>(symptoms));
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("id={id}", Name = "SymptomById")]
        public IActionResult GetSymptom(int id)
        {
            try
            {
                var symptom = _symptom.GetSymptom(id);
                return Ok(Mapper.Map<SymptomView>(symptom));
            }
            catch (NullReferenceException ex)
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
        public async Task<IActionResult> DeleteSymptom(int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    return Ok(Mapper.Map<SymptomView>(await _symptom.DeleteSymptomAsync(id)));
                }
                catch (NullReferenceException ex)
                {
                    return NotFound(ex.Message);
                }
                catch (TaskCanceledException ex)
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
