using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZenPay.Core.DTOs;
using ZenPay.Core.Services;

namespace ZenPay.Api.Controllers
{
    /// <summary>
    /// This is a typical ASP.NET Core API Controller.
    /// It handles incoming HTTP requests (GET, POST), calls our Service Layer,
    /// and then returns the appropriate HTTP status codes (200 OK, 404 Not Found, etc.).
    /// </summary>
    [ApiController]     // Tells ASP.NET this is an API controller (adds auto-validation for DTOs)
    [Route("[controller]")] // Maps this controller to /Transaction (based on its name)
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        // Dependency Injection: The framework gives us the service automatically!
        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        /// <summary>
        /// Retrieves all transactions.
        /// GET /transaction
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var transactions = await _transactionService.GetAllTransactionsAsync();
            return Ok(transactions); // Returns 200 OK with the data
        }

        /// <summary>
        /// Retrieves a specific transaction by its ID.
        /// GET /transaction/{id}
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var transaction = await _transactionService.GetTransactionByIdAsync(id);
            
            if (transaction == null) 
            {
                return NotFound(); // Returns 404 Not Found if the ID doesn't exist
            }
            
            return Ok(transaction);
        }

        /// <summary>
        /// Creates a new transaction.
        /// POST /transaction
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTransactionRequest request)
        {
            // Note: We don't need to check "ModelState.IsValid" manually here because 
            // the [ApiController] attribute does it for us automatically based on our DTO!
            var result = await _transactionService.CreateTransactionAsync(request);
            
            // Returns 201 Created and provides a link to the GET endpoint for this new item.
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Simulates processing a transaction with a bank.
        /// POST /transaction/{id}/process
        /// </summary>
        [HttpPost("{id:guid}/process")]
        public async Task<IActionResult> Process(Guid id)
        {
            try
            {
                var result = await _transactionService.ProcessTransactionAsync(id);
                
                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                // Returns 400 Bad Request if the transaction wasn't in "Pending" status
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}
