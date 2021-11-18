using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThePie.Models
{ 
    public class Published
    {
        [Key, Column(Order = 1)]
        public int AuthorId { get; set; }

        [Key, Column(Order = 2)]
        public int PublisherId { get; set; }

        public int Total { get; set; }

        [ForeignKey("AuthorId")]
        public Author Author { get; set; }

        [ForeignKey("PublisherId")]
        public Publisher Publisher { get; set; }
    }
}
