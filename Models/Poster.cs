using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThePie.Models
{
    public class Poster
    {
        [key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [DisplayName("Poster title")]
        public String Name { get; set; }

        [DisplayName("File name")]
        public String ImageName { get; set; }

        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile Photo { get; set; }
    }
}
