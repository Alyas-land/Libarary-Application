using System.ComponentModel.DataAnnotations;

namespace LibararyApplication.Models
{
    public class RegisterViewModel
    {
     
        [Required(ErrorMessage = "لطفا این فیلد را پر کنید")]
        public string Email { get; set; }

        [Required(ErrorMessage = "لطفا این فیلد را پر کنید")]
        [StringLength(100, ErrorMessage = "طول نام نا معتبر", MinimumLength = 3)]
        public string Name { get; set; }

        [StringLength(100)]
        [MinLength(3)]
        [Required(ErrorMessage = "لطفا فیلد را پر کنید")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [StringLength(100)]
        [MinLength(3)]
        [Required(ErrorMessage = "لطفا فیلد را پر کنید")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "رمز ها مطابقت ندارد، دوباره تلاش کنید")]
        public string ConfrimPassword { get; set; }
    }
}
