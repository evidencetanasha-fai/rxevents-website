using System;

namespace RxEvents.Models
{
    public class Gallery
    {
        public int GalleryID { get; set; }
        public int EventID { get; set; }
        public string ImagePath { get; set; }
        public string Caption { get; set; }
        public DateTime DateAdded { get; set; }
        public bool IsActive { get; set; }
        public string EventName { get; set; }
    }
}