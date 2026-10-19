namespace RxEvents.Models
{
    public class Event
    {
        public int EventID { get; set; }

        public int CategoryID { get; set; }

        public string EventName { get; set; }

        public string Description { get; set; }

        public string EventImage { get; set; }

        public decimal BasePrice { get; set; }

        public bool IsActive { get; set; }

        public string CategoryName { get; set; }
    }
}