using System;

namespace RxEvents.Models
{
    public class User
    {
        public int UserID { get; set; }
        public int RoleID { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public string PasswordHash { get; set; }
        public string ProfilePhoto { get; set; }
        public string DisplayMode { get; set; }

        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }

        // Useful when displaying the user's role
        public string RoleName { get; set; }
    }
}