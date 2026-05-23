using System.ComponentModel.DataAnnotations;

namespace MovieLog.ViewModels;

public class RegisterViewModel
{
    //cereun un username obligatiroiu 
    [Required(ErrorMessage = "Username is required.")]
    [MinLength(3, ErrorMessage = "Username must be at least 3 characters long.")]
    [Display(Name = "Username")]
    public string Username { get; set; } = string.Empty;

    //cerem adresa de email si verificam daca e valida
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    //cerem numele complet al utilizatorului
    [Required(ErrorMessage = "Full name is required.")]
    [MinLength(3, ErrorMessage = "Full name must be at least 3 characters long.")]
    [Display(Name = "Full Name")]
    public string FullName { get; set; } = string.Empty;

    //cerem parola si ascundem textul introdus cu DataType.Password
    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    [MinLength(4, ErrorMessage = "Password must be at least 4 characters long.")]
    [Display(Name = "Password")]
    public string Password { get; set; }= string.Empty;

    //cerem confirmarea parolei si verificam daca se potriveste cu parola introdusa cu Compare("Password")
    [Required(ErrorMessage = "Confirm Password is required.")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    [Display(Name = "Confirm Password")]
    public string ConfirmPassword { get; set; } = string.Empty;

}
