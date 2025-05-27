using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Cinema_TRIZBD
{
    public partial class AppWindow : Window
    {
        public Users User => CurrentUser.User;
        private bool _areTicketsShown = false;

        public AppWindow()
        {
            InitializeComponent();
            if (!CurrentUser.IsLoggedIn)
            {
                MessageBox.Show("Ошибка авторизации");
                Close();
                return;
            }

            DataContext = this;
            LoadFilms();
        }

        private void LoadFilms()
        {
            var db = My_CinemaEntities.GetContext();
            var films = db.Films.ToList();

            foreach (var film in films)
            {
                db.Entry(film).Collection(f => f.Genres).Load();
                FilmsPanel.Children.Add(new FilmCardControl(film));
            }
        }

        private void MyTicketsButton_Click(object sender, RoutedEventArgs e)
        {
            if (_areTicketsShown)
            {
                ClearTickets();
            }
            else
            {
                LoadUserTickets();
            }
        }

        private void LoadUserTickets()
        {
            var db = My_CinemaEntities.GetContext();
            var tickets = db.Tickets
                .Where(t => t.UserId == User.Id)
                .Include(t => t.Sessions)
                .Include(t => t.Sessions.Films)
                .Include(t => t.Sessions.Halls)
                .Include(t => t.Sessions.Halls.Cinemas)
                .ToList();

            TicketsItemsControl.Items.Clear();

            if (tickets.Any())
            {
                NoTicketsText.Visibility = Visibility.Collapsed;
                TicketsItemsControl.Visibility = Visibility.Visible;

                foreach (var ticket in tickets)
                {
                    TicketsItemsControl.Items.Add(new
                    {
                        FilmTitle = ticket.Sessions.Films.Title,
                        CinemaHall = $"{ticket.Sessions.Halls.Cinemas.Title}, Зал {ticket.Sessions.Halls.Title}",
                        DateTime = $"{ticket.Sessions.Data} в {ticket.Sessions.Time}",
                        SeatInfo = $"Ряд {ticket.Row + 1}, Место {ticket.Seat + 1}",
                        Price = $"{ticket.Sessions.Price} руб.",
                        City = $"{ticket.Sessions.Halls.Cinemas.Cites.Titles}",
                        TicketId = ticket.Id
                    });
                }
            }
            else
            {
                NoTicketsText.Visibility = Visibility.Visible;
                TicketsItemsControl.Visibility = Visibility.Collapsed;
            }

            _areTicketsShown = true;
        }

        private void ClearTickets()
        {
            TicketsItemsControl.Items.Clear();
            NoTicketsText.Visibility = Visibility.Collapsed;
            _areTicketsShown = false;
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Если переключились на другую вкладку и билеты были показаны - скрываем их
            if (e.Source is TabControl && _areTicketsShown)
            {
                ClearTickets();
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentUser.Logout();
            new MainWindow().Show();
            Close();
        }
        private void RefreshUserData()
        {
            // Принудительно обновляем привязки данных
            var db = My_CinemaEntities.GetContext();
            db.Entry(User).Reload(); // Перезагружаем данные пользователя из БД

            // Обновляем все привязки данных
            var temp = User;
            CurrentUser.Logout(); // Временно сбрасываем
            CurrentUser.Login(temp); // Возвращаем обратно
            // Принудительно обновляем интерфейс
            this.DataContext = null;
            this.DataContext = this;
        }

        private void EditProfileButton_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new EditProfileWindow(User);
            if (editWindow.ShowDialog() == true)
            {
                RefreshUserData(); // Обновляем данные на экране
            }
        }

        private void DeleteTicketButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите удалить этот билет?",
                               "Подтверждение",
                               MessageBoxButton.YesNo,
                               MessageBoxImage.Question) != MessageBoxResult.Yes)
            {
                return;
            }

            var button = (Button)sender;
            int ticketId = (int)button.Tag;

            var db = My_CinemaEntities.GetContext();

            var ticket = db.Tickets.FirstOrDefault(t => t.Id == ticketId);
            if (ticket != null)
            {
                var occupiedSeat = db.OccupiedSeats.FirstOrDefault(os =>
                    os.SessionId == ticket.SessionId &&
                    os.Row == ticket.Row &&
                    os.Seat == ticket.Seat);

                if (occupiedSeat != null)
                {
                    db.OccupiedSeats.Remove(occupiedSeat);
                }

                db.Tickets.Remove(ticket);
                db.SaveChanges();

                // После удаления проверяем, остались ли еще билеты
                LoadUserTickets();

                MessageBox.Show("Билет успешно удален!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
