using System;

namespace RxEvents.Models
{
    public class WishlistFolder
    {
        public int WishlistFolderID { get; set; }

        public int UserID { get; set; }

        public int CategoryID { get; set; }

        public string FolderName { get; set; }

        public decimal? Budget { get; set; }

        public string Notes { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string CategoryName { get; set; }

        public int ImageCount { get; set; }

        public string CoverImage { get; set; }
    }
}