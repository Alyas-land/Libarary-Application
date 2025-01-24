namespace LibararyApplication.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }


        public bool ReadStatus { get; set; }
        public bool? LibrarianStatus { get; set; }
        public bool? ReturnStatus { get; set; }
        public DateTime CreatedAt { get; set; }

        public DateTime SubmitTime { get; set; }

        public List<ReservationItem> reservationItem { get; set; }

        public string Description { get; set; }

        public Reservation()
        {
            reservationItem = new List<ReservationItem>();
            //LibrarianStatus = librarianStatus;
            //Description = description;
        }
    }
}
