using System;
using System.Collections.Generic;
using System.Text;

namespace ZenPay.Core.ENUM
{
    public enum TransactionStatus
    {
        Pending,
        Authorized,
        Declined,
        Captured,
        Refunded,
        Failed
    }
}
