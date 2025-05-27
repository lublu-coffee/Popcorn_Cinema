using System.Linq;
using System.Windows;

namespace Cinema_TRIZBD
{
    public partial class UserLoginWindow : Window
    {
        public UserLoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(emailTextBox.Text) ||
                string.IsNullOrWhiteSpace(passwordBox.Password))
            {
                MessageBox.Show("Введите email и пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var db = My_CinemaEntities.GetContext();
            string hashedPassword = new Hashing().GetHashString(passwordBox.Password);

            var user = db.Users.FirstOrDefault(u => u.Email == emailTextBox.Text &&
                                                  u.Password == hashedPassword);

            if (user == null)
            {
                MessageBox.Show("Неверный email или пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Успешный вход
            CurrentUser.Login(user);
            var appWindow = new AppWindow();
            appWindow.Show();

            // Закрываем все предыдущие окна
            foreach (Window window in Application.Current.Windows)
            {
                if (window != appWindow)
                    window.Close();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}