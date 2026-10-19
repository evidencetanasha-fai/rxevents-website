using System;

namespace RxEvents.Models
{
    public class Booking
    {
        public int BookingID { get; set; }

        public string BookingReference { get; set; }

        public int UserID { get; set; }

        public int EventID { get; set; }
        public int? WishlistFolderID { get; set; }
        public string EventName { get; set; }

        public int? VenueID { get; set; }
        public string VenueAddress { get; set; }

        public int PackageID { get; set; }

        public DateTime EventDate { get; set; }

        public TimeSpan EventTime { get; set; }

        public int GuestCount { get; set; }

        public decimal TotalAmount { get; set; }

        public string BookingStatus { get; set; }

        public string AdditionalNotes { get; set; }

        public DateTime BookingDate { get; set; }
    }
}