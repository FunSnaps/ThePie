using System;
using System.ComponentModel.DataAnnotations;

namespace ThePie.Models
{
    public class CreateRole
    {
        [Required]
        public String RoleName { get; set; }
    }
}
