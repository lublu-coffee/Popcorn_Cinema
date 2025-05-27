using System.Windows;
using static Cinema_TRIZBD.BookingWindow;

namespace Cinema_TRIZBD
{
    public partial class TicketWindow : Window
    {
        private readonly Sessions _session;
        private readonly SeatViewModel _seat;
        private readonly Users _user;

        public string FilmTitle { get; }
        public string CinemaInfo { get; }
        public string SessionInfo { get; }
        public string SeatInfo { get; }
        public string PriceInfo { get; }

        public TicketWindow(Sessions session, SeatViewModel seat, Users user)
        {
            InitializeComponent();

            _session = session;
            _seat = seat;
            _user = user;

            // Формируем данные для отображения
            var db = My_CinemaEntities.GetContext();
            db.Entry(session).Reference(s => s.Films).Load();
            db.Entry(session).Reference(s => s.Halls).Load();
            db.Entry(session.Halls).Reference(h => h.Cinemas).Load();

            FilmTitle = session.Films.Title;
            CinemaInfo = $"Кинотеатр: {session.Halls.Cinemas.Title}, Зал: {session.Halls.Title}";
            SessionInfo = $"Дата: {session.Data}, Время: {session.Time}";
            SeatInfo = $"Ряд: {_seat.Row + 1}, Место: {_seat.Seat + 1}";
            PriceInfo = $"Цена: {session.Price} руб.";

            DataContext = this;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            var db = My_CinemaEntities.GetContext();

            // Бронируем место
            var occupiedSeat = new OccupiedSeats
            {
                Row = _seat.Row,
                Seat = _seat.Seat,
                SessionId = _session.Id
            };

            // Создаем билет
            var ticket = new Tickets
            {
                SessionId = _session.Id,
                UserId = _user.Id,
                Row = _seat.Row,
                Seat = _seat.Seat
            };

            db.OccupiedSeats.Add(occupiedSeat);
            db.Tickets.Add(ticket);

            try
            {
                db.SaveChanges();
                MessageBox.Show("Поздравляем с успешной покупкой билета!", "Успех",
                              MessageBoxButton.OK, MessageBoxImage.Information);

                // Закрываем все окна
                this.Close();
                //Owner?.Close();
            }
            catch
            {
                MessageBox.Show("Ошибка при покупке билета", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}