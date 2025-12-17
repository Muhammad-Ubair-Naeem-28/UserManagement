using System.ComponentModel.DataAnnotations;

namespace BlazorApp1.Shared.Dtos
{
    public class GetUserListDtos
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
