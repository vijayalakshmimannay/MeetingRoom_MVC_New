

using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MeetingRoom1.Models
{
    public class EmployeeModel
    {
         public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Required.")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [StringLength(150, MinimumLength = 6)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Required.")]
        public string BranchName { get; set; }
        
        public int Role { get; set; }


    }

    
}
