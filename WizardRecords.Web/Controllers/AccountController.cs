using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WizardRecords.Dtos;
using WizardRecords.Api.Domain.Entities;
using WizardRecords.Api.Interfaces;

namespace WizardRecords.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ICartRepository _cartRepository;




        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole<Guid>> roleManager, IConfiguration configuration, ICartRepository cartRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _cartRepository = cartRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model) {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new User(model.UserName) {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                AddressNum = model.AddressNum,
                StreetName = model.StreetName,
                City = model.City,
                Province = model.Province,
                PostalCode = model.PostalCode
            };


            if (_cartRepository.FindByEmail(user.Email))
                return BadRequest();

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            var roleResult = await _userManager.AddToRoleAsync(user, "Guest");

            if (!roleResult.Succeeded)
                return BadRequest(roleResult.Errors);

            await _signInManager.SignInAsync(user, isPersistent: false);
            return Ok();
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model) {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest("Invalid login attempt.");

            var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (!result.Succeeded)
                return BadRequest("Invalid login attempt.");

            var tokenString = GenerateJwtTokenAsync(user);
            return Ok(new { token = tokenString });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            Console.WriteLine("User disconnected");
            return Ok();
        }

        private async Task<string> GenerateJwtTokenAsync(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SuperFunHappySlide!!!"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var roles = await _userManager.GetRolesAsync(user);

            // Créez un ClaimsIdentity avec les réclamations existantes et ajoutez la réclamation du rôle.
            var claims = new List<Claim>{
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("id", user.Id.ToString()),
                //new Claim("firstName", user.FirstName),
                //new Claim("lastName", user.LastName),
                //new Claim("email", user.Email),
                //new Claim("phoneNumber", user.PhoneNumber),
                //TODO: ajouter adresse
                //new Claim("address", user. ),
                //TODO: ajouter ville
                //new Claim("city", user. ),
                //TODO: ajouter province
                //new Claim("provinceState", user. ),
                //TODO: ajouter pays
                //new Claim("country", user. ),
                //TODO: ajouter code postal
                //new Claim("postalCode", user. ),

            };

            foreach (var role in roles)
            {
                claims.Add(new Claim("role", role));
            }

            var token = new JwtSecurityToken(
                issuer: "VotreIssuer",
                audience: "VotreAudience",
                claims: claims,
                expires: DateTime.Now.AddHours(1), // Temps d'expiration du token
                signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            Console.WriteLine( tokenString );
            return tokenString;
        }
    }
}
