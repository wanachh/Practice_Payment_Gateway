using System;

namespace ZenPay.Core.DTOs
{
    /// <summary>
    /// DTO for the response sent back to the user.
    /// This allows us to hide internal database fields in the future if we want to,
    /// and ensures the API response structure doesn't break if we change our database schema.
    /// </summary>
    public class TransactionResponse
    {
        public Guid Id { get; set; }
        
        public string TransactionNumber { get; set; } = string.Empty;
        
        public decimal Amount { get; set; }
        
        public string Currency { get; set; } = string.Empty;
        
        public string Status { get; set; } = string.Empty;
        
        public string? ErrorCode { get; set; }
        
        public DateTimeOffset TransactionDateTime { get; set; }
    }
}
