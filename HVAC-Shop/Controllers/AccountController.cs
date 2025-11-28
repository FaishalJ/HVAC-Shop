using HVAC_Shop.Core.Domain.IdentityEntities;
using HVAC_Shop.Core.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HVAC_Shop.Controllers
{
    public class AccountController(SignInManager<User> signInManager, UserManager<User> userManager) : BaseController
    {
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult> Register(RegisterDto registerDto)
        {
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

            // Ensure role exists before assigning
            if (await userManager.IsInRoleAsync(newUser, "User") == false)
            {
                var roleResult = await userManager.AddToRoleAsync(newUser, "User");
                if (!roleResult.Succeeded)
                    return BadRequest(roleResult.Errors);
            }

            return Ok();
        }
    }
}
