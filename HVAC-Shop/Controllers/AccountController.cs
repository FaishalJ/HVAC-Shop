using HVAC_Shop.Core.Domain.IdentityEntities;
using HVAC_Shop.Core.DTO;
using HVAC_Shop.Core.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HVAC_Shop.Controllers
{
    public class AccountController(SignInManager<User> signInManager, UserManager<User> userManager) : BaseController
    {
        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newUser = new User
            {
                UserName = registerDto.Email,
                Email = registerDto.Email
            };

            var result = await userManager.CreateAsync(newUser, registerDto.Password);

            // Ensure role exists before assigning
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return ValidationProblem();
            }

            //await userManager.AddToRoleAsync(newUser, UserRoles.User.ToString());
            var roleResult = await userManager.AddToRoleAsync(newUser, UserRoles.User.ToString());

            if (!roleResult.Succeeded)
                return BadRequest(roleResult.Errors);

            return Ok();
        }

        [HttpGet("user-info")]
        public async Task<ActionResult<User>> GetUserInfo()
        {
            if (User.Identity?.IsAuthenticated == false)
                return NoContent();

            var user = await userManager.GetUserAsync(User);

            if (user == null)
                return Unauthorized();

            var roles = await userManager.GetRolesAsync(user);

            return Ok(new
            {
                user.UserName,
                user.Email,
                Roles = string.Join(", ", roles),
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("users")]
        public async Task<ActionResult<List<UserDto>>> GetUsers()
        {
            var users = userManager.Users.ToList();

            List<UserDto> userDtos = [];

            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);

                userDtos.Add(new UserDto
                {
                    Id = user.Id,
                    Email = user.Email!,
                    Roles = string.Join(", ", roles)
                });
            }
            return Ok(userDtos);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            return Ok();
        }

    }
}
