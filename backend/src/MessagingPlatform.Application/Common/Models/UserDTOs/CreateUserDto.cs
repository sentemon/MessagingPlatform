using System.ComponentModel.DataAnnotations;

namespace MessagingPlatform.Application.Common.Models.UserDTOs;

public class CreateUserDto
{
    [Required]
    [MaxLength(50)]
    public required string FirstName { get; set; }

    [Required]
    [MaxLength(50)]
    public required string LastName { get; set; }

    [Required]
    [MaxLength(30)]
    public required string Username { get; set; }
    
    [Required]
    [MaxLength(320)]
    public required string Email { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    public required string Password { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public required string ConfirmPassword { get; set;  }
}