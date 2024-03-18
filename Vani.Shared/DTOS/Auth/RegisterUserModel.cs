using System.ComponentModel.DataAnnotations;

namespace Vani.Shared.DTOS.Auth;

public class RegisterUserModel
{
    [Required(ErrorMessage = "First Name is required")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last Name is required")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Confirm Password is required")]
    public string ConfirmPassword { get; set; }

    [Required(ErrorMessage = "Phone Number is required")]
    public string PhoneNumber { get; set; }
}
