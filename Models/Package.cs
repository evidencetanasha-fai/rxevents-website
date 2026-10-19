using System.Collections.Generic;

namespace RxEvents.Models
{
    public class Package
    {
        public int PackageID { get; set; }
        public string PackageName { get; set; }
        public string PackageType { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImagePath { get; set; }
        public bool IsActive { get; set; }

        public List<Service> IncludedServices { get; set; }

        public Package()
        {
            IncludedServices = new List<Service>();
        }
    }
}