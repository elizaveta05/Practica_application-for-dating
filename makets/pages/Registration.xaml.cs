using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace makets.pages
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
        }

        private void Label_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //добавить проверку полей
            if (PassB1.Password == PassB2.Password)
            {
                MessageBox.Show("Пароли совпадают. Регистрация успешна!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                WelcomePagexaml welcomWindow = new WelcomePagexaml();
                welcomWindow.Show();
                this.Close();

            }
            else
            {
                MessageBox.Show("Пароли не совпадают. Пожалуйста, попробуйте снова.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
