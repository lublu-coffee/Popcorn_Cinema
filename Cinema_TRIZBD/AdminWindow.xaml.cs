using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Cinema_TRIZBD
{
    public partial class AdminWindow : Window
    {
        public Administrators Admin => Current_Admin.Admin;
        private My_CinemaEntities db = new My_CinemaEntities();

        public AdminWindow()
        {
            InitializeComponent();
            if (!Current_Admin.IsLoggedIn)
            {
                MessageBox.Show("Ошибка авторизации");
                Close();
                return;
            }

            DataContext = this;
            LoadGenres();
        }

        private void LoadGenres()
        {
            var genres = db.Genres.ToList();
            GenresListBox.ItemsSource = genres;
        }

        private void CreateFilmButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleTextBox.Text) ||
                string.IsNullOrWhiteSpace(StoryLineTextBox.Text) ||
                string.IsNullOrWhiteSpace(DurationTextBox.Text) ||
                string.IsNullOrWhiteSpace(ImageTextBox.Text) ||
                GenresListBox.SelectedItems.Count == 0)
            {
                MessageBox.Show("Заполните все поля и выберите хотя бы один жанр");
                return;
            }

            if (!int.TryParse(DurationTextBox.Text, out int duration))
            {
                MessageBox.Show("Введите корректную продолжительность (в минутах)");
                return;
            }

            var newFilm = new Films
            {
                Title = TitleTextBox.Text,
                StotyLine = StoryLineTextBox.Text,
                Duration = duration,
                Image = ImageTextBox.Text
            };

            foreach (Genres selectedGenre in GenresListBox.SelectedItems)
            {
                var genre = db.Genres.Find(selectedGenre.Id);
                newFilm.Genres.Add(genre);
            }

            db.Films.Add(newFilm);
            db.SaveChanges();

            MessageBox.Show("Фильм успешно добавлен");
            ClearForm();
        }

        private void ClearForm()
        {
            TitleTextBox.Clear();
            StoryLineTextBox.Clear();
            DurationTextBox.Clear();
            ImageTextBox.Clear();
            GenresListBox.UnselectAll();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            Current_Admin.Logout();
            new MainWindow().Show();
            Close();
        }
    }
}
