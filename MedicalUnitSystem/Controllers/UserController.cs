using System;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MedicalUnitSystem.Data;
using MedicalUnitSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens;
using System.Threading.Tasks;

namespace MedicalUnitSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public UserController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid model received.");

            // Check for existing user by email
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
                return StatusCode(StatusCodes.Status400BadRequest, "User already exists with this email.");

            // Create a new user
            var newUser = new ApplicationUser
            {
                Email = model.Email,
                UserName = model.Email, // Using Email as Username
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                Role = model.UserRole,
                Address = model.Address
            };

            // Create User and assign password
            var createUserResult = await _userManager.CreateAsync(newUser, model.Password); 
            if (!createUserResult.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to create user.");

            // Ensure roles exist and assign the specified role
            await EnsureRolesExist();
            if (!await _roleManager.RoleExistsAsync(model.UserRole))
                return BadRequest("Invalid role specified.");

            var addToRoleResult = await _userManager.AddToRoleAsync(newUser, model.UserRole);
            if (!addToRoleResult.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to assign role to user.");

            return Ok("User registered successfully.");
        }

        // Helper Method to Ensure Roles Exist
        private async Task EnsureRolesExist()
        {
            var roles = new[] { "Doctor", "Nurse", "Pharmacist", "LabScientist", "ProtocolUnit" };
            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                    await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid login request." });

            // Verify username
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return Unauthorized(new { message = "Invalid email or password." });

            // Verify password
            if (!await _userManager.CheckPasswordAsync(user, model.Password))
                return Unauthorized(new { message = "Invalid email or password." });

            // Generate JWT Token
            var token = GenerateJwtToken(user);

            return Ok(new { token });
        }

        private string GenerateJwtToken(ApplicationUser user)
        {
            var authClaims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(ClaimTypes.Role, user.Role) 
        };

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"], 
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}