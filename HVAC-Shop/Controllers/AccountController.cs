using HVAC_Shop.Core.Domain.Entities;
using HVAC_Shop.Core.Domain.IdentityEntities;
using HVAC_Shop.Core.DTO;
using HVAC_Shop.Core.Enum;
using HVAC_Shop.Core.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
                Roles = roles
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

        [Authorize(Roles = "Admin")]
        [HttpPost("promote-to-admin/{email}")]
        public async Task<ActionResult> PromoteToAdmin(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                return NotFound("User not found.");

            var result = await userManager.AddToRoleAsync(user, UserRoles.Admin.ToString());
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return ValidationProblem();
            }

            return Ok($"{email} successfully promoted to admin");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("revoke-admin/{email}")]
        public async Task<ActionResult> DemoteFromAdmin(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                return NotFound("User not found.");

            var result = await userManager.RemoveFromRoleAsync(user, UserRoles.Admin.ToString());
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return ValidationProblem();
            }

            return Ok($"{email} successfully removed from admin role");
        }

        [Authorize]
        [HttpPost("address")]
        public async Task<ActionResult> CreateOrUpdateAddress(UserAddressDto addressDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await userManager.Users.Include(a => a.Address).FirstOrDefaultAsync(x => x.UserName == User.Identity!.Name);

            if (user == null)
                return Unauthorized();

            var address = new Address
            {
                Id = addressDto.Id,
                Name = addressDto.Name,
                City = addressDto.City,
                Country = addressDto.Country,
                Line1 = addressDto.Line1,
                Line2 = addressDto.Line2,
                State = addressDto.State,
                PostalCode = addressDto.PostalCode
            };

            user.Address = address;

            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return ValidationProblem();
            }
            return Ok(user.Address.ToDto());
        }

        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult> GetAddress()
        {
            var address = await userManager.Users
                .Where(x => x.UserName == User.Identity!.Name)
                .Select(u => u.Address)
                .FirstOrDefaultAsync();

            if (address == null) return NoContent();

            return Ok(address.ToDto());
        }
    }
}
