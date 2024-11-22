using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace ApiCodeF
{
    public class Game
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(5), MaxLength(30)]
        [DisplayName("Título")]
        public String Title { get; set; }

        [Required]
        public int GenreId { get; set; }
        [JsonIgnore]
        [DisplayName("Género")]
        public Genre? Genre { get; set; }
    }
}
