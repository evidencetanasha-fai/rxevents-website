using System;

namespace RxEvents.Models
{
    public class WishlistImage
    {
        public int WishlistImageID { get; set; }

        public int WishlistFolderID { get; set; }

        public string ImagePath { get; set; }

        public string Caption { get; set; }

        public DateTime DateAdded { get; set; }
    }
}