using CustomersAPI;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TransactionService.Process;

namespace TransactionService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionProcess _transactionProcess;

        public TransactionController(ITransactionProcess transactionProcess)
        {
            _transactionProcess = transactionProcess;
        }
        [CustomAuthentication(Roles ="Farmer,Dealer")]
        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromBody] Transaction transaction)
        {
            if (transaction == null)
                return BadRequest("Invalid transaction data.");

            var result = await _transactionProcess.CreateTransactionAsync(transaction);
            if (result)
                return Ok();

            return StatusCode(500, "Error creating transaction.");
        }
        [CustomAuthentication(Roles = "Farmer")]
        [HttpGet("farmer")]
        public async Task<IActionResult> GetTransactionsByFarmerAndDateRange([FromQuery] int farmerId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            if (startDate > endDate)
                return BadRequest("Invalid date range.");

            var transactions = await _transactionProcess.GetTransactionsByFarmerAndDateRangeAsync(farmerId, startDate, endDate);
            return Ok(transactions);
        }
        [CustomAuthentication(Roles = "Dealer")]
        [HttpGet("dealer")]
        public async Task<IActionResult> GetTransactionsByDealerAndDateRange([FromQuery] int dealerId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            if (startDate > endDate)
                return BadRequest("Invalid date range.");

            var transactions = await _transactionProcess.GetTransactionsByDealerAndDateRangeAsync(dealerId, startDate, endDate);
            return Ok(transactions);
        }
        [CustomAuthentication(Roles = "Farmer,Dealer")]
        [HttpGet("{transactionId}")]
        public async Task<IActionResult> GetTransactionById(int transactionId)
        {
            var transaction = await _transactionProcess.GetTransactionByIdAsync(transactionId);
            if (transaction == null)
            {
                return NotFound("Transaction not found");
            }
            return Ok(transaction);
        }

    }
}
