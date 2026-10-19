
using System;

namespace RxEvents.Models
{
    public class Payment
    {
        public int PaymentID { get; set; }

        public int BookingID { get; set; }

        public string PaymentReference { get; set; }

        public decimal Amount { get; set; }

        public string PaymentMethod { get; set; }

        public DateTime PaymentDate { get; set; }

        public string PaymentStatus { get; set; }

        public string TransactionReference { get; set; }
    }
}
