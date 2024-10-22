using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace makets.pages
{
    /// <summary>
    /// Логика взаимодействия для UserPurposes.xaml
    /// </summary>
    public partial class UserPurposes : Window
    {
        public UserPurposes()
        {
            InitializeComponent();
        }
        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton toggleButton = sender as ToggleButton;
            if (toggleButton != null)
            {
                // Дополнительная логика, если необходимо
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UserInfo userInfo = new UserInfo();
            userInfo.Show();
            this.Close();
        }
    }
}
