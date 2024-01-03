using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WizardRecords.Api.Data.Entities;
using WizardRecords.Api.Domain.Entities;
using WizardRecords.Api.Interfaces;
using WizardRecords.Dtos;
using WizardRecords.Repositories;

namespace WizardRecords.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartController : ControllerBase
    {
        //pas sur pour l'album.. 
        private readonly IAlbumRepository _albumRepository;
        private readonly ICartRepository _cartRepository;
        private readonly UserManager<User> _userManager;

        public CartController(IAlbumRepository albumRepository, ICartRepository cartRepository, UserManager<User> userManager)
        {
            _cartRepository = cartRepository;
            _albumRepository = albumRepository;
            _userManager = userManager;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Cart>>> GetAllItem()
        {

            var cart = await _cartRepository.GetAllItemAsync();
            return Ok(cart);
        }

        [HttpPut("update/{cartId}/{AlbumId}")] //change l'item au panier
        public async Task<ActionResult<Cart>> UpdateItem(Guid cartId, Guid AlbumId, int quantity) {
            try {
                var cart = await _cartRepository.GetCartByIdAsync(cartId);
                if(cart != null) {
                    await _cartRepository.UpdateItemByIdAsync(cartId, AlbumId, quantity);
                    return Ok();
                }
                else {
                    return NotFound();
                }
            }
            catch(Exception) {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpDelete("delete/{cartId}/{AlbumId}")] //Delete l'item au panier
        public async Task<ActionResult<Cart>> DeleteItem(Guid cartId, Guid AlbumId)
        {
            try
            {
                var cart = await _cartRepository.GetCartByIdAsync(cartId);
                if (cart != null)
                {
                    var check = await _cartRepository.DeleteItemByIdAsync(cartId, AlbumId);
                    if (check != null)
                    {
                        return Ok();
                    }
                    else
                    {
                        return NotFound();
                    }
                    
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpPost("add/{cartId}/{AlbumId}")] //Cart Ajouter l'item au panier
        public async Task<ActionResult<Cart>> AddItem(Guid cartId, Guid AlbumId)
        {
            try
            {

                var cart = await _cartRepository.GetCartByIdAsync(cartId);
                if (cart != null)
                {
                  var check =  await _cartRepository.AddItemByIdAsync(cartId, AlbumId);
                    if(check != null)
                    {
                        return Ok();
                    }
                    else
                    {
                        return NotFound();
                    }
                    
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpPost("createpanier/{userId}")]
        public async Task<ActionResult<Cart>> CreateCart(Guid userId)
        {
            try
            {
                var user = await _cartRepository.GetUserByIdAsync(userId);
                if (user == null)
                {
                    throw new Exception("User not found");
                }
                else
                {
                    var cart = await _cartRepository.CreateCartAsync(user.Id);
                    return Ok(cart);
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }


        [HttpPost("create")] //Detail, pour créé user et Tokem ( retourne token ) 
        public async Task<ActionResult<string>> CreateUser()
        {
            try
            {
                
                    var user = await _cartRepository.CreateUserGuest();
                    //Faut que je créé mon user en dehors d'ici Create Guest va return le TOKEN
                    var tokenString = GenerateJwtTokenAsync(user);
                    return Ok(new { token = tokenString });

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpGet("user/{userId}")] //CART pour aller chercher le panier
        public async Task<ActionResult<Cart>> GetUserCart(Guid userId)
        {
            try
            {
                var user = await _cartRepository.GetUserByIdAsync(userId);
                if (user == null)
                {
                   throw new Exception("User not found");
                }
                else
                {
                    var cart = await _cartRepository.GetUserCartAsync(user.Id);

                    if (cart != null)
                    {
                        return Ok(cart);
                    }
                    else
                    {
                        return NotFound(); // Ajustez ce comportement en fonction de vos besoins
                    }
                }               
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        private async Task<string> GenerateJwtTokenAsync(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SuperFunHappySlide!!!"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

           // var roles = await _userManager.GetRolesAsync(user);

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
                new Claim("role", "Guest")
            };


            //claims.Add(new Claim("role", "Guest"));
            

            var token = new JwtSecurityToken(
                issuer: "VotreIssuer",
                audience: "VotreAudience",
                claims: claims,
                expires: DateTime.Now.AddHours(1), // Temps d'expiration du token
                signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            Console.WriteLine(tokenString);
            return tokenString;
        }

    }
}
