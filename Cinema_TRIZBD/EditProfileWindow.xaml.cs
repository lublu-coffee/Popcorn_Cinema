using System;
using System.Data.Entity;
using System.Windows;

namespace Cinema_TRIZBD
{
    public partial class EditProfileWindow : Window
    {
        private Users _user;

        public EditProfileWindow(Users user)
        {
            InitializeComponent();
            _user = user;

            // Заполняем поля текущими данными
            FirstNameBox.Text = user.FirstName;
            LastNameBox.Text = user.LastName;
            PatronymicBox.Text = user.Patronymic;
            CityBox.Text = user.City;
            EmailBox.Text = user.Email;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Валидация данных
            if (string.IsNullOrWhiteSpace(FirstNameBox.Text) ||
                string.IsNullOrWhiteSpace(LastNameBox.Text) ||
                string.IsNullOrWhiteSpace(EmailBox.Text))
            {
                MessageBox.Show("Пожалуйста, заполните обязательные поля (Имя, Фамилия, Email)");
                return;
            }

            // Обновляем данные пользователя
            _user.FirstName = FirstNameBox.Text;
            _user.LastName = LastNameBox.Text;
            _user.Patronymic = PatronymicBox.Text;
            _user.City = CityBox.Text;
            _user.Email = EmailBox.Text;

            try
            {
                // Сохраняем в БД
                var db = My_CinemaEntities.GetContext();
                db.Entry(_user).State = EntityState.Modified;
                db.SaveChanges();

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}");
            }
        }
    }
}