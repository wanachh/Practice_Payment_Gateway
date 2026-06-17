using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZenPay.Core.Data;
using ZenPay.Core.DTOs;
using ZenPay.Core.ENUM;
using ZenPay.Core.Models;

namespace ZenPay.Core.Services
{
    /// <summary>
    /// The concrete implementation of our service.
    /// This class contains all the core "Business Logic".
    /// By moving this out of the Controller/Program.cs, our API layer stays very clean
    /// and is only responsible for receiving web requests and sending web responses.
    /// </summary>
    public class TransactionService : ITransactionService
    {
        private readonly ZenPayDbContext _db;

        // Dependency Injection: The framework automatically provides the DbContext here.
        public TransactionService(ZenPayDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<TransactionResponse>> GetAllTransactionsAsync()
        {
            var transactions = await _db.Transactions.ToListAsync();
            
            // Map the database models to our DTOs before returning
            return transactions.Select(MapToResponse);
        }

        public async Task<TransactionResponse?> GetTransactionByIdAsync(Guid id)
        {
            var transaction = await _db.Transactions.FindAsync(id);
            
            if (transaction == null) return null;

            return MapToResponse(transaction);
        }

        public async Task<TransactionResponse> CreateTransactionAsync(CreateTransactionRequest request)
        {
            // Create a new database model from the user's DTO
            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                Amount = request.Amount,
                Currency = request.Currency,
                Status = TransactionStatus.Pending.ToString(),
                TransactionDateTime = DateTimeOffset.UtcNow,
                TransactionNumber = $"TXN-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString().Substring(0, 4)}"
            };

            _db.Transactions.Add(transaction);
            await _db.SaveChangesAsync();

            return MapToResponse(transaction);
        }

        public async Task<TransactionResponse?> ProcessTransactionAsync(Guid id)
        {
            var transaction = await _db.Transactions.FindAsync(id);
            if (transaction == null) return null;

            // Business Rule: We only process pending transactions.
            if (transaction.Status != TransactionStatus.Pending.ToString())
            {
                throw new InvalidOperationException("Only pending transactions can be processed.");
            }

            // Mock external bank logic (80% success, 20% fail)
            var random = new Random();
            if (random.Next(100) < 80)
            {
                transaction.Status = TransactionStatus.Captured.ToString();
            }
            else
            {
                transaction.Status = TransactionStatus.Failed.ToString();
                transaction.ErrorCode = "ERR_INSUFFICIENT_FUNDS";
            }

            await _db.SaveChangesAsync();
            return MapToResponse(transaction);
        }

        /// <summary>
        /// A private helper method to convert a Database Model (Transaction) 
        /// into a Response DTO (TransactionResponse).
        /// </summary>
        private TransactionResponse MapToResponse(Transaction t)
        {
            return new TransactionResponse
            {
                Id = t.Id,
                TransactionNumber = t.TransactionNumber,
                Amount = t.Amount,
                Currency = t.Currency,
                Status = t.Status,
                ErrorCode = t.ErrorCode,
                TransactionDateTime = t.TransactionDateTime
            };
        }
    }
}
