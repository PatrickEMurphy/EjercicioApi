using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiCodeF
{
    public class Genre
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(5), MaxLength(30)]
        [DisplayName("Género")]
        public String GenreName { get; set; }

        // Lista de Juegos
        [JsonIgnore]
        public List<Game>? Games { get; set; }
    }
}
