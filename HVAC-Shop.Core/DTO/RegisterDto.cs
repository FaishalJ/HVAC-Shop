using System.ComponentModel.DataAnnotations;

namespace HVAC_Shop.Core.DTO
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Provide valid email")]
        public string Email { get; set; } ="";
        public string Password { get; set; } = "";
    }
}
