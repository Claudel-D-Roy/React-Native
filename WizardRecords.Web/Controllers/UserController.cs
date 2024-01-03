using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WizardRecords.Api.Domain.Entities;
using WizardRecords.Dtos;

namespace WizardRecords.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase {
        private readonly UserManager<User> _userManager;

        public UserController(UserManager<User> userManager) {
            _userManager = userManager;
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetUserProfile() {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user == null)
                return NotFound();

            var userDetails = new UserDto(
                user.Id,
                user.FirstName,
                user.LastName
            );

            return Ok(userDetails);
        }

        [HttpPut("profile/update")]
        [Authorize]
        public async Task<IActionResult> UpdateUserProfile([FromBody] UserDto userDetails) {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user == null)
                return NotFound();

            user.FirstName = userDetails.FirstName;
            user.LastName = userDetails.LastName;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest("Failed to update user profile.");

            return Ok(userDetails);
        }
    }
}
