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
    /// Логика взаимодействия для WelcomePagexaml.xaml
    /// </summary>
    public partial class WelcomePagexaml : Window
    {
        public WelcomePagexaml()
        {
            InitializeComponent();
        }

        private void StartTextBlock_MouseUp(object sender, MouseButtonEventArgs e)
        {
            NewUser newUser = new NewUser();
            newUser.Show();
            this.Close();
        }
    }
}
