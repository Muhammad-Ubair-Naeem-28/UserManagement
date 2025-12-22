using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BlazorApp1.Shared.Dtos
{
    public class FormData
    {
        //name required
        [Required(ErrorMessage = "Name is required")]
        [StringLength(30, ErrorMessage = "Name cannot be longer than 30 characters")]
        public string Name { get; set; } = string.Empty;

        //email    
        [Key]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        //password 
        [NotMapped]
        [Required(ErrorMessage = "Password is required")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
            ErrorMessage = "Password must contain at least 1 uppercase, 1 lowercase, 1 number, and 1 special character")]
        public string Password { get; set; } = string.Empty;

        [JsonIgnore]
        public string PasswordHash { get; set; } = string.Empty;

        public string Role { get; set; } = "User";

        //url 
        [Url(ErrorMessage = "Please enter a valid URL")]
        public string? Url { get; set; }

        //gender
        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; } = "Other";

        //phone number
        [Phone(ErrorMessage = "Please enter a valid phone number")]
        public string? PhoneNumber { get; set; }

        //date
        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; } = DateTime.Today;
        public bool Verify { get; set; }

    }
}