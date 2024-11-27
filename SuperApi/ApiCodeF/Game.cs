using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiCodeF
{
    public class Game
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(5), MaxLength(30)]
        [DisplayName("Título")]
        public String Name { get; set; }

        [Required]
        [DisplayName("Precio")]
        public double Price { get; set; } 

        [Required]
        public int GenreId { get; set; }

        [JsonIgnore]
        [DisplayName("Género")]
        public Genre? Genre { get; set; }
    }
}
