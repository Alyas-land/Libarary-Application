namespace LibararyApplication.Models
{
    public class ShowCardViewModel
    {
        public IEnumerable<BookInfo> books { get; set; }
        public IEnumerable<ReservationItem> reservationItems { get; set; }


    }

    public class BookInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int Quantity { get; set; }


    }
}
