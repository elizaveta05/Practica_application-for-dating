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

namespace makets.pages.Profile
{
    /// <summary>
    /// Логика взаимодействия для EditUserTags.xaml
    /// </summary>
    public partial class EditUserTags : Window
    {
        private List<int> selectedTagIds = new List<int>();

        public EditUserTags()
        {
            InitializeComponent();
        }
        private void TagButton_Click(object sender, RoutedEventArgs e)
        {
            Button tagButton = sender as Button;
            if (tagButton != null)
            {
                string tagContent = tagButton.Content.ToString();

                // Попробуем безопасно преобразовать свойство Tag в целое число
                if (int.TryParse(tagButton.Tag.ToString(), out int tagId))
                {
                    selectedTagIds.Add(tagId);

                    double width = tagButton.Width;
                    double height = tagButton.Height;
                    TagPanel.Children.Remove(tagButton);

                    StackPanel tagStackPanel = new StackPanel { Orientation = Orientation.Horizontal };
                    TextBlock tagTextBlock = new TextBlock { Text = tagContent, Margin = new Thickness(0, 0, 5, 0), FontSize = 16, FontWeight = FontWeights.Bold };
                    Button removeButton = new Button { Content = "X", Margin = new Thickness(0, 0, 5, 0), Style = (Style)FindResource("RemoveButtonStyle") };
                    removeButton.Click += (s, args) => RemoveTag_Click(tagContent, tagButton, tagStackPanel, width, height);

                    tagStackPanel.Children.Add(tagTextBlock);
                    tagStackPanel.Children.Add(removeButton);
                    SelectedTagPanel.Children.Add(tagStackPanel);
                }
                else
                {
                    MessageBox.Show("Некорректный формат тега.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void RemoveTag_Click(string tagContent, Button tagButton, StackPanel tagStackPanel, double width, double height)
        {
            SelectedTagPanel.Children.Remove(tagStackPanel);
            tagButton.Width = width;
            tagButton.Height = height;
            TagPanel.Children.Add(tagButton);
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
