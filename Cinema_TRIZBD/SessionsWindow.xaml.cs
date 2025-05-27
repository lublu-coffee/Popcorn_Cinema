using System.Windows;
using System.Windows.Controls;
using System.Linq;
using Cinema_TRIZBD.ViewModels;

namespace Cinema_TRIZBD
{
    public partial class SessionsWindow : Window
    {
        private readonly Films _film;

        public SessionsWindow(Films film)
        {
            InitializeComponent();
            _film = film;
            Title = $"Сеансы фильма: {film.Title}";
            LoadCities();
            LoadSessions();
        }

        private void LoadCities()
        {
            var db = My_CinemaEntities.GetContext();
            CityComboBox.ItemsSource = db.Cites.ToList();
            if (CityComboBox.Items.Count > 0)
            {
                CityComboBox.SelectedIndex = 0;
            }
        }

        private void LoadSessions(int? cityId = null)
        {
            var db = My_CinemaEntities.GetContext();
            var query = db.Sessions
                .Where(s => s.Film_Id == _film.Id)
                .ToList();

            if (cityId.HasValue)
            {
                query = query.Where(s => s.Halls.Cinemas.City_Id == cityId.Value).ToList();
            }

            SessionsItemsControl.Items.Clear();

            foreach (var session in query)
            {
                SessionsItemsControl.Items.Add(new SessionCardControl(new SessionViewModel(session)));
            }

            // Показываем соответствующее сообщение
            if (query.Any())
            {
                SessionsScroll.Visibility = Visibility.Visible;
                NoSessionsText.Visibility = Visibility.Collapsed;
            }
            else
            {
                SessionsScroll.Visibility = Visibility.Collapsed;
                NoSessionsText.Visibility = Visibility.Visible;
                NoSessionsText.Text = cityId.HasValue
                    ? "В этом городе нет доступных сеансов"
                    : "Нет доступных сеансов для этого фильма";
            }
        }

        private void CityComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CityComboBox.SelectedItem is Cites selectedCity)
            {
                LoadSessions(selectedCity.Id);
            }
        }
    }
}