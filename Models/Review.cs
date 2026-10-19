using System;

namespace RxEvents.Models
{
    public class Review
    {
        public int ReviewID { get; set; }

        public int UserID { get; set; }

        public int EventID { get; set; }

        public int BookingID { get; set; }

        public int Rating { get; set; }

        public string Comment { get; set; }

        public DateTime ReviewDate { get; set; }

        public bool IsApproved { get; set; }

        // Display information
        public string EventName { get; set; }

        public string EventImage { get; set; }

        public string CustomerName { get; set; }
    }
}