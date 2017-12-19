using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WTracking.Models.AccountViewModels
{
    public class ExternalLoginViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
