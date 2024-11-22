using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SuperApi
{
    public class Juegos
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MinLength(5), MaxLength(30)]
        [DisplayName("Título")]
        public String Title { get; set; }
        [Required]
        [MinLength(2), MaxLength(20)]
        [DisplayName("Género")]
        public String Genre { get; set; }
    }
}
