using System.ComponentModel.DataAnnotations;

public class RegistrationRequest
{
    [Required(ErrorMessage ="Username is required")]
    public string? Username {get; set;}
    
    [Required(ErrorMessage = "Password is required")]
    [MinLength(8, ErrorMessage = "The password should be at least 8 characters long")]
    [RegularExpression("^(?=.*?[A-Za-z])(?=.*?[0-9])(?=.*[!@#$%^&*]).{8,}$", ErrorMessage = "Password should contain at least one uppercase, one lowercase, one number and one of the following special characters: !@#$%^&*")]
    public string? Password{get; set;}
}