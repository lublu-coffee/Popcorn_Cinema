using System.Windows;
using System.Windows.Controls;

namespace Cinema_TRIZBD
{
    public partial class FilmCardControl : UserControl
    {
        public Films Film { get; set; }

        public FilmCardControl(Films film)
        {
            InitializeComponent();
            Film = film;
            this.DataContext = Film;
        }

        private void DetailsButton_Click(object sender, RoutedEventArgs e)
        {
            var detailsWindow = new FilmDetailsWindow(Film);
            detailsWindow.Owner = Window.GetWindow(this);
            detailsWindow.ShowDialog();
        }
    }
}