using CropService;
using CustomersAPI;
using FarmerService.Process;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FarmerService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    public class FarmerController : ControllerBase
    {
        private readonly IFarmerProcess _farmerProcess;

        public FarmerController(IFarmerProcess farmerProcess)
        {
            _farmerProcess = farmerProcess;
        }
        [CustomAuthentication(Roles = "Admin,Farmer")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFarmerById(int id)
        {
            var farmer = await _farmerProcess.GetFarmerByIdAsync(id);
            if (farmer == null) return NotFound();
            return Ok(farmer);
        }
        [CustomAuthentication(Roles = "Admin,Farmer")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFarmer(int id, [FromBody] Farmer farmer)
        {
            if (id != farmer.FarmerId) return BadRequest();
            var result = await _farmerProcess.UpdateFarmerAsync(id, farmer);
            return result ? NoContent() : NotFound();
        }
        [CustomAuthentication(Roles = "Farmer")]
        [HttpPost("publish-crop")]
        public async Task<IActionResult> PublishCrop([FromBody] Crop crop)
        {
            var result = await _farmerProcess.PublishCropAsync(crop);
            return result ? Ok() : BadRequest();
        }

        //[HttpPut("update-status")]
        //public async Task<IActionResult> UpdateFarmersStatus([FromQuery] bool isActive)
        //{
        //    var result = await _farmerProcess.UpdateFarmersStatusAsync(isActive);
        //    if (!result)
        //        return NotFound("No farmers found to update.");
        //    return Ok("Farmers' status updated successfully.");
        //}
        [CustomAuthentication(Roles = "Admin")]
        [HttpPut("update-status")]
        public async Task<IActionResult> UpdateFarmersStatus([FromBody] List<int> farmerIds, [FromQuery] bool isActive)
        {
            var result = await _farmerProcess.UpdateFarmersStatusAsync(farmerIds, isActive);
            if (!result)
                return NotFound("No farmers found to update.");
            return Ok();
        }
        [CustomAuthentication(Roles = "Admin,Farmer")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllFarmers()
        {
            var farmers = await _farmerProcess.GetAllFarmersAsync();
            return Ok(farmers);
        }
        [HttpPost("add")]
        public async Task<IActionResult> AddFarmer([FromBody] Farmer farmer)
        {
            if (farmer == null)
            {
                return BadRequest("Farmer information is required.");
            }

            var result = await _farmerProcess.RegisterFarmer(farmer);

            if (result == null) 
            {
                return BadRequest("Farmer registration failed.");
            }

            return CreatedAtAction(nameof(AddFarmer), new { id = result.FarmerId }, result);
        }
        [HttpGet("getFarmerId/{userId}")]
        public async Task<IActionResult> GetFarmerIdByUserId(int userId)
        {
            var farmerId = await _farmerProcess.GetFarmerIdByUserIdAsync(userId);
            if (farmerId == null)
            {
                return NotFound("Farmer not found for the given UserId.");
            }
            return Ok(farmerId);
        }

    }
}
