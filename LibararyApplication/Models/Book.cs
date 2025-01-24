using System.ComponentModel.DataAnnotations;

namespace LibararyApplication.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "لطفا این فیلد را پر کنید")]
        [StringLength(100, ErrorMessage = "طول نام نا معتبر", MinimumLength = 3)]
        public string Name { get; set; }

        public string Description { get; set; }

        public int PagePaper { get; set; }

        public int Quantity { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }


        public List<ReservationItem> reservationItem { get; set; }

        public Book()
        {
            reservationItem = new List<ReservationItem>();
        }

    }
}
