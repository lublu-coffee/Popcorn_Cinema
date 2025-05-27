using System.Windows;
using System.Windows.Controls;
using Cinema_TRIZBD.ViewModels;

namespace Cinema_TRIZBD
{
    public partial class SessionCardControl : UserControl
    {
        public SessionViewModel SessionVM { get; }

        public SessionCardControl(SessionViewModel session)
        {
            InitializeComponent();
            SessionVM = session;
            DataContext = SessionVM;
        }

        private void BookButton_Click(object sender, RoutedEventArgs e)
        {
            var bookingWindow = new BookingWindow(SessionVM.Session)
            {
                Owner = Window.GetWindow(this)
            };
            //((Window)this.Parent).Hide(); // Скрываем окно сеансов
            bookingWindow.ShowDialog();
            //((Window)this.Parent).Show(); // Показываем окно сеансов после закрытия
        }
    }
}