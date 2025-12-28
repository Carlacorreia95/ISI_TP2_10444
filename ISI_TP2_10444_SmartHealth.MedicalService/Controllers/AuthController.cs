using ISI_TP2_10444_SmartHealth_Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ISI_TP2_10444_SmartHealth_MedicalService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly SmartHealthContext _context;
        string role;
        private readonly IConfiguration _config;

        public AuthController(IConfiguration config, SmartHealthContext context)
        {
            _config = config;
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login(string username, string password)
        {

            var jwtSettings = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            if (username != "doctor")
            {
                var patient = _context.Patients
                    .FirstOrDefault(p => p.UserName == username);

                if (patient == null)
                    return Unauthorized("Username not found");

                if (patient.Password != password)
                    return Unauthorized("Invalid password");

                role = patient.UserType ?? "Patient";

            }
            else if(username == "doctor" && password == "1234"){
                role = "Doctor";
            }
            else
            {
                return Unauthorized("Invalid credentials");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role)
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpireMinutes"])),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return Ok(new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }
    }
}
