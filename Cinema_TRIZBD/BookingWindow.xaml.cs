using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Cinema_TRIZBD
{
    public partial class BookingWindow : Window, INotifyPropertyChanged
    {
        public Sessions Session { get; }
        public Halls Hall { get; }
        public List<OccupiedSeats> OccupiedSeats { get; }
        private SeatViewModel _selectedSeat;

        public SeatViewModel SelectedSeat
        {
            get => _selectedSeat;
            set
            {
                _selectedSeat = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasSelectedSeat));
                OnPropertyChanged(nameof(SelectedSeatText));
            }
        }

        public bool HasSelectedSeat => SelectedSeat != null;

        public string SelectedSeatText => SelectedSeat != null
            ? $"Ряд: {SelectedSeat.Row + 1}; Место: {SelectedSeat.Seat + 1}"
            : "Не выбрано";

        public event PropertyChangedEventHandler PropertyChanged;

        public BookingWindow(Sessions session)
        {
            InitializeComponent();
            Session = session;
            var db = My_CinemaEntities.GetContext();

            // Загружаем связанные данные
            db.Entry(session).Reference(s => s.Halls).Load();
            Hall = session.Halls;
            db.Entry(session).Collection(s => s.OccupiedSeats).Load();
            OccupiedSeats = session.OccupiedSeats.ToList();

            GenerateSeats();
            DataContext = this;
        }

        private void GenerateSeats()
        {
            var seats = new List<SeatViewModel>();

            for (int row = 0; row < Hall.Rows; row++)
            {
                for (int seat = 0; seat < Hall.Seats; seat++)
                {
                    bool isOccupied = OccupiedSeats.Any(os => os.Row == row && os.Seat == seat);
                    seats.Add(new SeatViewModel
                    {
                        Row = row,
                        Seat = seat,
                        IsAvailable = !isOccupied,
                        SeatNumber = $"{row + 1}-{seat + 1}"
                    });
                }
            }

            SeatsItemsControl.ItemsSource = seats;
        }

        private void SeatButton_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).DataContext is SeatViewModel seat)
            {
                SelectedSeat = seat;
            }
        }

        private void BuyButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedSeat == null)
            {
                MessageBox.Show("Пожалуйста, выберите место!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            //var currentUser = ((AppWindow)Application.Current.MainWindow).CurrentUser;
            var ticketWindow = new TicketWindow(Session, SelectedSeat, CurrentUser.User);
            ticketWindow.Show();
            this.Close();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public class SeatViewModel
        {
            public int Row { get; set; }
            public int Seat { get; set; }
            public string SeatNumber { get; set; }
            public bool IsAvailable { get; set; }
        }
    }
}