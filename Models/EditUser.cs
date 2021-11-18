using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ThePie.Models
{
    public class EditUser
    {
        public EditUser()
        {
            Claims = new List<string>();
            Roles = new List<string>();
        }

        public string id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string City { get; set; }

        public List<string> Claims { get; set; }

        public IList<string> Roles { get; set; }
    }
}
