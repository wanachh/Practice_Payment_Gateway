using System;
using System.ComponentModel.DataAnnotations;

namespace ZenPay.Core.DTOs
{
    /// <summary>
    /// DTO (Data Transfer Object) for creating a transaction.
    /// We use this instead of the database model (Transaction.cs) to control exactly
    /// what fields the user is allowed to send us when making a request.
    /// </summary>
    public class CreateTransactionRequest
    {
        // We only ask the user for the amount.
        // We don't ask them for an ID, Status, or Date because our API should generate those securely!
        [Required(ErrorMessage = "The Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }

        // Currency has a default, but the user can override it if they want.
        [MaxLength(3)]
        public string Currency { get; set; } = "THB";
    }
}
