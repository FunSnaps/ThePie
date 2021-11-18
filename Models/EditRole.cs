using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ThePie.Models
{
    public class EditRole
    {
        public EditRole()
        {
            Users = new List<string>(); 
        }

        public string id { get; set; }

        [Required(ErrorMessage ="RoleName is required!")]
        public string RoleName { get; set; }

        public List<string> Users { get; set; }
    }
}
