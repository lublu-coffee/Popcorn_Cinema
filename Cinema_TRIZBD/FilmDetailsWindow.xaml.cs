using System.Windows;
using Cinema_TRIZBD.ViewModels;


namespace Cinema_TRIZBD
{
    public partial class FilmDetailsWindow : Window
    {
        public FilmDetailsWindow(Films film)
        {
            InitializeComponent();

            // Создаем ViewModel и загружаем жанры
            var db = My_CinemaEntities.GetContext();
            db.Entry(film).Collection(f => f.Genres).Load();

            DataContext = new FilmViewModel(film);
        }

        private void SessionsButton_Click(object sender, RoutedEventArgs e)
        {
            var film = ((FilmViewModel)DataContext).Film;
            var sessionsWindow = new SessionsWindow(film);
            sessionsWindow.Owner = this;
            this.Hide();
            sessionsWindow.Show();
            //this.Show();
            //this.Close();
        }
    }
}