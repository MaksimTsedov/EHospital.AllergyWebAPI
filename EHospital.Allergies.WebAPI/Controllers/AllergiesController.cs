using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using EHospital.Allergies.WebAPI.Views;
using EHospital.Allergies.Model;
using System.Linq;
using EHospital.Allergies.BusinessLogic.Contracts;
using EHospital.Allergies.BusinessLogic.Services;
using EHospital.Logging;

namespace EHospital.Allergies.WebAPI.Controllers
{
    /// <summary>
    /// Controller for <see cref="AllergyService"/>
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]
    public class AllergiesController : ControllerBase
    {
        private readonly IAllergyService _allergy;

        /// <summary>
        /// Initializes a new instance of the <see cref="AllergiesController"/> class.
        /// </summary>
        /// <param name="allergy">The allergy.</param>
        public AllergiesController(IAllergyService allergy)
        {
            _allergy = allergy;
        }

        /// <summary>
        /// Gets all allergies.
        /// </summary>
        /// <returns>HTTP response result.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllAllergies()
        {
            LoggingToFile.LoggingInfo("Getting all allergies.");
            var allergies = await _allergy.GetAllAllergies();
            if (!allergies.Any())
            {
                LoggingToFile.LoggingWarn("No allergy recorded.");
                return NotFound("No allergy recorded.");
            }

            return Ok(Mapper.Map<IEnumerable<AllergyView>>(allergies));
        }

        /// <summary>
        /// Gets all allergies by search key.
        /// </summary>
        /// <param name="searchKey">The search key.</param>
        /// <returns>HTTP response result.</returns>
        [HttpGet("searchKey={searchKey}", Name = "SearchAllergyQuery")]
        public async Task<IActionResult> GetAllAllergies(string searchKey)
        {
            LoggingToFile.LoggingInfo($"Getting allergies by search key \"{searchKey}\".");
            var allergies = await _allergy.SearchAllergiesByName(searchKey);
            if (!allergies.Any())
            {
                LoggingToFile.LoggingWarn($"No allergy found by \"{searchKey}\" search key.");
                return NotFound("No allergy recorded.");
            }

            LoggingToFile.LoggingInfo($"Got {allergies.Count()} allergies with search key \"{searchKey}\".");
            return Ok(Mapper.Map<IEnumerable<AllergyView>>(allergies));
        }

        /// <summary>
        /// Gets the allergy by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>HTTP response result.</returns>
        [HttpGet("{id}", Name = "AllergyById")]
        public async Task<IActionResult> GetAllergy(int id)
        {
            LoggingToFile.LoggingInfo($"Getting allergy by id = {id}.");
            try
            {
                var allergy = await _allergy.GetAllergy(id);
                LoggingToFile.LoggingInfo($"Got allergy by id = {id}.");
                return Ok(Mapper.Map<AllergyView>(allergy));
            }
            catch (ArgumentNullException ex)
            {
                LoggingToFile.LoggingWarn($"No allergy found by id = {id}.", ex);
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Creates the allergy.
        /// </summary>
        /// <param name="allergy">The allergy got from client.</param>
        /// <returns>HTTP response result.</returns>
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
                LoggingToFile.LoggingInfo($"Successful create of {result.Pathogen} allergy.");
                return Created("allergies", Mapper.Map<AllergyView>(result));
            }
            catch (ArgumentException ex)
            {
                LoggingToFile.LoggingError("Cannot insert allergy due to duplicate name.", ex);
                return Conflict(ex.Message);
            }
        }

        /// <summary>
        /// Deletes the allergy by id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>HTTP response result.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAllergy(int id)
        {
            LoggingToFile.LoggingInfo("Deleting allergy.");

            try
            {
                var result = await _allergy.DeleteAllergyAsync(id);
                LoggingToFile.LoggingInfo($"{result.Pathogen} deleted successfully.");
                return Ok(Mapper.Map<AllergyView>(result));
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
