using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApiCodeF.Data;
using ApiCodeF.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ApiCodeF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] Register model)
        {
            // Comprobar que se han incluido los campos
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
                return BadRequest("Email y Password son requeridos.");
            }

            // Crear nuevo usuario
            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            // Si falla BadRequest
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            // Asigna el rol "Basic" al usuario
            var roleResult = await _userManager.AddToRoleAsync(user, "Basic");
            if (!roleResult.Succeeded)
            {
                return BadRequest("No se pudo asignar el rol al usuario.");
            }

            return CreatedAtAction(nameof(Register), new { id = user.Id }, user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login model)
        {
            // Comprobar si se han rellenado los campos
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
                return BadRequest("Email y Password son requeridos.");
            }

            // Buscar el usuario y comprobar si existe
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !(await _userManager.CheckPasswordAsync(user, model.Password)))
            {
                return Unauthorized("Credenciales inválidas.");
            }

            // Date
            DateTimeOffset date = new DateTimeOffset(DateTime.UtcNow);
            String dateString = date.ToUnixTimeSeconds().ToString();


            // Lista de claims
            var claims = new List<Claim>
            { 
                // Identificación unico del token
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                // Fecha de emision del token
                new Claim(JwtRegisteredClaimNames.Iat, dateString, ClaimValueTypes.Integer64),
                // Usuario portador del token
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName)
            };

            // Si el usuario tiene roles, agregar los roles a los claims
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Crear Token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["JWT:Issuer"],
                _configuration["JWT:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: credentials
                );

            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }
    }
}
