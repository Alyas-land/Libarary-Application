using System.ComponentModel.DataAnnotations;

namespace LibararyApplication.Models
{
    public class LoginViewModel
    {
        
        [Required(ErrorMessage = "این فیلد نباید خالی باشد")]
        [MinLength(4, ErrorMessage =("طول کاراکتر ها باید بیشتر 4 باشد"))]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "این فیلد نباید خالی باشد")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [MinLength(3, ErrorMessage = ("طول کاراکتر ها باید بیشتر 4 باشد"))]
        public string Password { get; set; }

    }
}
