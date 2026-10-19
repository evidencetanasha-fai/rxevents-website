using System;

namespace RxEvents.Models
{
    public class FAQ
    {
        public int FAQID { get; set; }

        public string Question { get; set; }

        public string Answer { get; set; }

        public string Category { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}