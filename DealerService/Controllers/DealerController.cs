using CustomersAPI;
using DealerService.Process;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DealerService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    public class DealerController : ControllerBase
    {
        private readonly IDealerProcess _dealerProcess;

        public DealerController(IDealerProcess dealerProcess)
        {
            _dealerProcess = dealerProcess;
        }
        [CustomAuthentication(Roles = "Admin,Dealer")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDealerById(int id)
        {
            var dealer = await _dealerProcess.GetDealerByIdAsync(id);
            if (dealer == null) return NotFound();
            return Ok(dealer);
        }
        [CustomAuthentication(Roles = "Admin,Dealer")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDealer(int id, [FromBody] Dealer dealer)
        {
            if (id != dealer.DealerId) return BadRequest();
            var updated = await _dealerProcess.UpdateDealerAsync(id, dealer);
            if (!updated) return NotFound();
            return NoContent();
        }
        [CustomAuthentication(Roles = "Dealer,Farmer")]
        [HttpGet("crops")]
        public async Task<IActionResult> GetAllCrops()
        {
            var crops = await _dealerProcess.GetAllCropsAsync();
            if (crops == null || crops.Count == 0) return NotFound();
            return Ok(crops);
        }
        [CustomAuthentication(Roles = "Dealer")]
        [HttpGet("crops/name/{name}")]
        public async Task<IActionResult> GetCropByName(string name)
        {
            var crop = await _dealerProcess.GetCropByNameAsync(name);
            if (crop == null) return NotFound();
            return Ok(crop);
        }
        [CustomAuthentication(Roles = "Dealer")]
        [HttpGet("crops/{id}")]
        public async Task<IActionResult> GetCropById(int id)
        {
            var crop = await _dealerProcess.GetCropByIdAsync(id);
            if (crop == null) return NotFound();
            return Ok(crop);
        }
        [CustomAuthentication(Roles = "Admin")]
        [HttpPut("update-status")]
        public async Task<IActionResult> UpdateDealersStatus([FromBody] List<int> dealerIds, [FromQuery] bool isActive)
        {
            var result = await _dealerProcess.UpdateDealersStatusAsync(dealerIds, isActive);
            if (!result)
                return NotFound("No dealers found to update.");
            return Ok();
        }
        [CustomAuthentication(Roles = "Admin,Dealer")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllDealers()
        {
            var dealers = await _dealerProcess.GetAllDealersAsync();
            return Ok(dealers);
        }
        [HttpPost("add")]
        public async Task<IActionResult> AddDealer([FromBody] Dealer dealer)
        {
            if (dealer == null)
            {
                return BadRequest("Dealer information is required.");
            }

            var result = await _dealerProcess.RegisterDealer(dealer);

            if (result == null)
            {
                return BadRequest("Dealer registration failed.");
            }

            return CreatedAtAction(nameof(AddDealer), new { id = result.DealerId }, result);
        }
        [HttpGet("getDealerId/{userId}")]
        public async Task<IActionResult> GetDealerIdByUserId(int userId)
        {
            var dealerId = await _dealerProcess.GetDealerIdByUserIdAsync(userId);
            if (dealerId == null)
            {
                return NotFound("Dealer not found for the given UserId.");
            }
            return Ok(dealerId);
        }
    }
}
