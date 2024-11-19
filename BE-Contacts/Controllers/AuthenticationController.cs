using BE_Contacts.Models.DTO;
using BE_Contacts.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BE_Contacts.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;

        public AuthenticationController(IConfiguration config, IUserRepository userRepository)
        {
            _config = config; // Inyectamos IConfiguration para acceder al appsettings.json
            _userRepository = userRepository;
        }

        // POST: api/authentication/login
        [HttpPost("login")]
        public async Task<ActionResult<string>> Authenticate([FromBody] AuthenticationRequestBody authenticationRequestBody)
        {
            // Paso 1: Validar credenciales del usuario
            var user = await ValidateUser(authenticationRequestBody);

            if (user == null)
            {
                return Unauthorized("Invalid credentials.");
            }

            // Paso 2: Crear el token JWT
            var tokenToReturn = GenerateJwtToken(user);

            // return Ok(tokenToReturn);
            // Devuelve un objeto JSON en lugar de solo una cadena
            return Ok(new { token = tokenToReturn });
        }

        private async Task<UserDTO> ValidateUser(AuthenticationRequestBody authRequestBody)
        {
            var user = await _userRepository.GetUserByEmailAsync(authRequestBody.Email);

            if (user != null && user.Password == authRequestBody.Password)
            {
                return user;
            }

            return null;
        }

        private string GenerateJwtToken(UserDTO user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["Authentication:SecretForKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claimsForToken = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.GivenName, user.Name),
                new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

            var jwtSecurityToken = new JwtSecurityToken(
                _config["Authentication:Issuer"],
                _config["Authentication:Audience"],
                claimsForToken,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(1),
                credentials);

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }
    }
}


