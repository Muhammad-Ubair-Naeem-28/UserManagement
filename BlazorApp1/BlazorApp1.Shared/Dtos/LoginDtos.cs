using System.ComponentModel.DataAnnotations;

namespace BlazorApp1.Shared.Dtos
{
    public class LoginData
    {
        //email    
        [Key]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;
        //password 
        [Required(ErrorMessage = "Password is required")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
            ErrorMessage = "Password must contain at least 1 uppercase, 1 lowercase, 1 number, and 1 special character")]
        public string Password { get; set; } = string.Empty;
        public bool RememberMe { get; set; }
    }


}