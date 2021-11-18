using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThePie.Models
{
    public class Author
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Display(Name = "Author")]
        public String Name { get; set; }

        [Display(Name = "Profile image")]
        public int PosterId { get; set; }

        [Display(Name = "Sex")]
        public Sex AuthorSex { get; set; }

        public Boolean Active { get; set; }

        [ForeignKey("PosterId")]
        public Poster Poster { get; set; }

        public enum Sex
        {
            Male,
            Female,
        }

    }
}
