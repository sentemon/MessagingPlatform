using System.ComponentModel.DataAnnotations;

namespace MessagingPlatform.Application.Common.Models;

public class SignInDto
{
    [Required]
    public required string Username { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    public required string Password { get; set; }
}