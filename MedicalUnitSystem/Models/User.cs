using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MedicalUnitSystem.Models
{
    public class UserRegistrationModel
    {
        [Key]  
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; } 

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string UserRole { get; set; } // e.g., Doctor, Nurse, etc.

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Address { get; set; }
    }

    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }
    }

    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Role { get; set; } // e.g., Doctor, Nurse, etc.

        [Required]
        public string Address { get; set; }
    }
}

