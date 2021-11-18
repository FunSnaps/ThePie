using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThePie.Models
{
    public class Comic 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public String Title { get; set; }

        [Display(Name = "Author")]
        public int AuthorId { get; set; }

        [Display(Name = "Publisher")]
        public int PublisherId { get; set; }

        [Display(Name = "Poster")]
        public int PosterId { get; set; }

        public DateTime ReleaseDate { get; set; }

        public int AgeRating { get; set; }

        public float Price { get; set; }

        [ForeignKey("AuthorId")]
        public Author Author { get; set; }

        [ForeignKey("PublisherId")]
        public Publisher Publisher { get; set; }

        [ForeignKey("PosterId")]
        public Poster Poster { get; set; }

        public Comic()
        {

        }
    }
}
