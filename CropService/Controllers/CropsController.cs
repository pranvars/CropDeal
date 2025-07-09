using CropService.Process;
using CropService.Repositories;
using CustomersAPI;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CropService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    public class CropsController : ControllerBase
    {
        private readonly ICropProcess _process;

        public CropsController(ICropProcess process)
        {
            _process = process;
        }
        [CustomAuthentication(Roles = "Farmer,Dealer")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Crop>>> GetAllCrops()
        {
            var crops = await _process.GetAllCropsAsync();
            if(crops == null || !crops.Any())
            {
                return NotFound();
            }
            return Ok(crops);
        }
        [CustomAuthentication(Roles = "Farmer,Dealer")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Crop>> GetCropById(int id)
        {
            try
            {
                var crop = await _process.GetCropByIdAsync(id);
                return Ok(crop);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        [CustomAuthentication(Roles = "Farmer,Dealer")]
        [HttpGet("name/{cropName}")]
        public async Task<ActionResult<Crop>> GetCropByName(string cropName)
        {
            try
            {
                var crop = await _process.GetCropByNameAsync(cropName);
                return Ok(crop);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [CustomAuthentication(Roles = "Farmer")]
        [HttpPost]
        public async Task<ActionResult<Crop>> CreateCrop([FromBody] Crop crop)
        {
            try
            {
                var createdCrop = await _process.CreateCropAsync(crop);
                return CreatedAtAction(nameof(GetCropById), new { id = createdCrop.CropId }, createdCrop);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [CustomAuthentication(Roles = "Farmer")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCrop(int id, [FromBody] Crop crop)
        {
            if (id != crop.CropId) return BadRequest(new { message = "Mismatched Crop ID." });

            try
            {
                await _process.UpdateCropAsync(crop);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [CustomAuthentication(Roles = "Farmer")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCrop(int id)
        {
            try
            {
                await _process.DeleteCropAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }

}

