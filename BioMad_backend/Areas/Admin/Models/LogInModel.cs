using System.ComponentModel.DataAnnotations;

namespace BioMad_backend.Areas.Admin.Models
{
  public class LogInModel
  {
    [Required, EmailAddress(ErrorMessage = "Неправильный Email")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Пароль обязателен")]
    public string Password { get; set; }
  }
}