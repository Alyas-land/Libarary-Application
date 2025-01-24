namespace LibararyApplication.Models
{
    public class ReservationItem
    {
        public int Id { get; set; }
        public int BookId { get; set; }

        public int Quantity { get; set; }
        public Book Book { get; set; }

        public int ReservationId { get; set; }
        public Reservation Reservation { get; set; }


    }
}
