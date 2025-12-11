using System;
using System.Collections.Generic;

namespace GreenCampus.Models
{
    public partial class User
    {
        public int UserId { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        public bool IsAdmin { get; set; }

    }
}