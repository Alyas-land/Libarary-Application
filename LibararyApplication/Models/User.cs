using System.ComponentModel.DataAnnotations;

namespace LibararyApplication.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "لطفا این فیلد را پر کنید")]
        //[RegularExpression(@"\d{10}", ErrorMessage = "کد ملی باید 10 رقم باشد.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "لطفا این فیلد را پر کنید")]
        [StringLength(100, ErrorMessage = "طول نام نا معتبر", MinimumLength = 3)]
        public string Name { get; set; }

        public string Password { get; set; }

        public DateTime CreatedAccount { get; set; }

        public string Role{ get; set; }

        public List<Reservation> reservation {  get; set; } 




        public User()
        {
            reservation = new List<Reservation>();
        }
    }
}
