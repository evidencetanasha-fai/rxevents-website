namespace RxEvents.Models
{
    public class Venue
    {
        public int VenueID { get; set; }

        public string VenueName { get; set; }

        public string Location { get; set; }

        public int Capacity { get; set; }

        public string Description { get; set; }

        public string ImagePath { get; set; }

        public decimal Price { get; set; }

        public bool IsActive { get; set; }
    }
}