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

namespace makets.pages.Profile
{
    /// <summary>
    /// Логика взаимодействия для EditUserPurposes.xaml
    /// </summary>
    public partial class EditUserPurposes : Window
    {
        public EditUserPurposes()
        {
            InitializeComponent();
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton selectedToggleButton = sender as ToggleButton;

            // Снимаем выбор с остальных кнопок
            var parent = VisualTreeHelper.GetParent(selectedToggleButton);
            while (parent != null && !(parent is StackPanel))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            if (parent is StackPanel stackPanel)
            {
                foreach (var child in stackPanel.Children)
                {
                    if (child is ToggleButton toggleButton && toggleButton != selectedToggleButton)
                    {
                        toggleButton.IsChecked = false;
                    }
                }
            }
        }
        private void Button1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
