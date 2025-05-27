using System.Linq;
using System.Windows;

namespace Cinema_TRIZBD
{
    public partial class AdminLoginWindow : Window
    {
        public AdminLoginWindow()
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

            var admin = db.Administrators.FirstOrDefault(a => a.Email == emailTextBox.Text &&
                                                          a.Password == hashedPassword);

            if (admin == null)
            {
                MessageBox.Show("Неверные учетные данные администратора!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Успешный вход
            Current_Admin.Login(admin);
            var adminWindow = new AdminWindow();
            adminWindow.Show();

            // Закрываем все предыдущие окна
            foreach (Window window in Application.Current.Windows)
            {
                if (window != adminWindow)
                    window.Close();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
