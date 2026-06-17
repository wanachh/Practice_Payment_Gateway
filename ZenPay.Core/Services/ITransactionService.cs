using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZenPay.Core.DTOs;

namespace ZenPay.Core.Services
{
    /// <summary>
    /// Interface for our Transaction Service.
    /// Interfaces define "what" the service can do, without worrying about "how" it does it.
    /// This makes our code easy to test (we can mock it) and easily swappable!
    /// </summary>
    public interface ITransactionService
    {
        Task<IEnumerable<TransactionResponse>> GetAllTransactionsAsync();
        
        Task<TransactionResponse?> GetTransactionByIdAsync(Guid id);
        
        Task<TransactionResponse> CreateTransactionAsync(CreateTransactionRequest request);
        
        Task<TransactionResponse?> ProcessTransactionAsync(Guid id);
    }
}
