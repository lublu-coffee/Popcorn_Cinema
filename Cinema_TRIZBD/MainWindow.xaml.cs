using System.Windows;
using System.Windows.Input;

namespace Cinema_TRIZBD
{
    public partial class MainWindow : Window
    {
        private bool tPressed = false;
        private bool pPressed = false;

        public MainWindow()
        {
            InitializeComponent();
            this.PreviewKeyDown += MainWindow_PreviewKeyDown;
            this.PreviewKeyUp += MainWindow_PreviewKeyUp;
        }

        private void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.T) tPressed = true;
            if (e.Key == Key.P) pPressed = true;

            if (tPressed && pPressed)
            {
                var adminLogin = new AdminLoginWindow();
                adminLogin.Owner = this;
                this.Hide();
                adminLogin.ShowDialog();
                this.Show();

                tPressed = false;
                pPressed = false;
            }
        }

        private void MainWindow_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.T) tPressed = false;
            if (e.Key == Key.P) pPressed = false;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new UserLoginWindow();
            loginWindow.Owner = this;
            this.Hide();
            loginWindow.ShowDialog();
            this.Show();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var registerWindow = new UserRegisterWindow();
            registerWindow.Owner = this;
            this.Hide();
            registerWindow.ShowDialog();
            this.Show();
        }
    }
}