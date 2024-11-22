using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SuperApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JuegosController : ControllerBase
    {
        public static int StaticId = 4;
        public static List<Juegos> games = new List<Juegos>{

            new Juegos
            {
                Id = 1,
                Title = "The legend of link",
                Genre = "Aventura"
            },
            new Juegos
            {
                Id = 2,
                Title = "Baba is you",
                Genre = "Puzzle"
            },
            new Juegos
            {
                Id = 3,
                Title = "Morio Car Waa",
                Genre = "Carreras"
            },
            new Juegos
            {
                Id = 4,
                Title = "VVVVVV",
                Genre = "Plataformas"
            }
        };
        
        // GET: api/<JuegosController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Juegos>> GetAll()
        {
            return Ok(games);
        }

        // GET api/<JuegosController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get(int id)
        {
            var game = games.FirstOrDefault(g => g.Id == id);
            if (game == null)
            {
                return NotFound($"No hay juego con id {id}");
            }
            return Ok(game);
        }

        // POST api/<JuegosController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PostCreate([FromBody]Juegos game)
        {
            game.Id = ++StaticId;
            
            if (TryValidateModel(game)) { 
                games.Add(game);
                return Ok(games);
            }
            else
                return BadRequest($"Los campos Titulo: o Genero: no son validos");
        }

        // PUT api/<JuegosController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PutEdit(int id, [FromBody] Juegos gameEdit)
        {
            var game = games.FirstOrDefault(g => g.Id == id);
            if (game == null)
            {
                return NotFound($"No existe el juego con id {id}");
            }


            if (TryValidateModel(gameEdit))
            {
                game.Title = gameEdit.Title;
                game.Genre = gameEdit.Genre;
                return Ok(game);
            }  
            else {
                return BadRequest($"Los campos no son válidos");
            }
        }

        // DELETE api/<JuegosController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            var game = games.FirstOrDefault(g => g.Id == id);
            if (game == null)
            {
                return NotFound($"No existe el juego con id {id}");
            }
            games.Remove(game);
            return Ok(games);
        }
    }
}
