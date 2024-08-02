using System.ComponentModel.DataAnnotations;

namespace MessagingPlatform.Application.Common.Models;

public class SignInDto
{
    [Required]
    public string Username { get; set; }
    
    [Required]
    public string Password { get; set; }
}