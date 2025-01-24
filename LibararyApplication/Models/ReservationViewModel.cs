namespace LibararyApplication.Models
{
    public class ReservationViewModel
    {
        public int ReservationId { get; set; }
        public string UserName { get; set; } // نام کاربر
        public bool Status { get; set; } // وضعیت رزرو
        public DateTime CreatedAt { get; set; } // زمان ایجاد
        public List<ReservationItemViewModel> ReservationBooks { get; set; }
    }
}
