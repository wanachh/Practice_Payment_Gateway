using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ZenPay.Core.ENUM;

namespace ZenPay.Core.Models
{
    public class Transaction
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(50)]
        public string TransactionNumber { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        [MaxLength(3)]
        public string Currency { get; set; } = "THB";

        [Required]
        public string Status { get; set; } = "Pending";

        public string? ErrorCode { get; set; } //if no error = NULL

        public DateTimeOffset TransactionDateTime { get; set; } = DateTimeOffset.UtcNow;
    }
}
