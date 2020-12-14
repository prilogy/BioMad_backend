using System.ComponentModel.DataAnnotations;

namespace BioMad_backend.Areas.Admin.Models
{
  public class SignUpModel
  {
    [Required(ErrorMessage = "Email обязателен"), EmailAddress(ErrorMessage = "Неправильный Email")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Пароль обязателен")]
    public string Password { get; set; }
    
    [Compare(nameof(RePassword), ErrorMessage = "Пароли должны совпадать")]
    public string RePassword { get; set; }
    [Required(ErrorMessage = "Секретный код обязателен")]
    public string Secret { get; set; }
  }
}