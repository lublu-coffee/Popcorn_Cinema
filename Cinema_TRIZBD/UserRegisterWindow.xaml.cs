using System.Windows;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace Cinema_TRIZBD
{
    public partial class UserRegisterWindow : Window
    {
        public UserRegisterWindow()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(firstNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(lastNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(emailTextBox.Text) ||
                string.IsNullOrWhiteSpace(passwordBox.Password) ||
                string.IsNullOrWhiteSpace(confirmPasswordBox.Password))
            {
                MessageBox.Show("Все поля обязательны для заполнения!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (passwordBox.Password != confirmPasswordBox.Password)
            {
                MessageBox.Show("Пароли не совпадают!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (passwordBox.Password.Length < 6)
            {
                MessageBox.Show("Пароль должен содержать минимум 6 символов!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var db = My_CinemaEntities.GetContext();
            if (db.Users.Any(u => u.Email == emailTextBox.Text))
            {
                MessageBox.Show("Пользователь с таким email уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var newUser = new Users
            {
                FirstName = firstNameTextBox.Text,
                LastName = lastNameTextBox.Text,
                Patronymic = patronymicTextBox.Text,
                City = cityTextBox.Text,
                Email = emailTextBox.Text,
                Password = new Hashing().GetHashString(passwordBox.Password)
            };

            try
            {
                db.Users.Add(newUser);
                db.SaveChanges();

                MessageBox.Show("Регистрация прошла успешно!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                // Открываем основное окно приложения
                CurrentUser.Login(newUser);
                var appWindow = new AppWindow();
                appWindow.Show();

                // Закрываем все предыдущие окна
                foreach (Window window in Application.Current.Windows)
                {
                    if (window != appWindow)
                        window.Close();
                }
            }
            catch
            {
                MessageBox.Show("Ошибка при сохранении данных!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

    public class Hashing
    {
        public string GetHashString(string s)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(s);
            using (MD5CryptoServiceProvider CSP = new MD5CryptoServiceProvider())
            {
                byte[] byteHash = CSP.ComputeHash(bytes);
                string hash = "";
                foreach (byte b in byteHash)
                {
                    hash += string.Format("{0:x2}", b);
                }
                return hash;
            }
        }
    }
}