using makets.pages;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace makets
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Label_MouseUp(object sender, MouseButtonEventArgs e)
        {
            PasswordReset passwordReset = new PasswordReset();
            passwordReset.Show();
            this.Close();
        }

        private void Label_MouseUp_1(object sender, MouseButtonEventArgs e)
        {
            Registration registrationWindow = new Registration();
            registrationWindow.Show();
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            /*
            //логика проверки логина, пароля 
            WelcomePagexaml welcomWindow = new WelcomePagexaml(0);
            welcomWindow.Show();
            this.Close();
            */
            UserInfo window = new UserInfo(2);
            window.Show();
            this.Close();
        }
    }
}