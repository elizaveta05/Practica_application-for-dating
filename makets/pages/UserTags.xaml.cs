using makets.helper;
using System;
using System.Collections.Generic;
using System.Drawing;
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
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace makets.pages
{
    /// <summary>
    /// Логика взаимодействия для UserTags.xaml
    /// </summary>
    public partial class UserTags : Window
    {
        public UserTags()
        {
            InitializeComponent();
        }

        private void TagButton_Click(object sender, RoutedEventArgs e)
        {
            Button tagButton = sender as Button;
            if (tagButton != null)
            {
                string tagContent = tagButton.Content.ToString();
                double width = tagButton.Width;
                double height = tagButton.Height;
                TagPanel.Children.Remove(tagButton);

                StackPanel tagStackPanel = new StackPanel { Orientation = Orientation.Horizontal };
                TextBlock tagTextBlock = new TextBlock { Text = tagContent, Margin = new Thickness(0, 0, 5, 0), FontSize = 16, FontWeight = FontWeights.Bold};
                Button removeButton = new Button { Content = "X", Margin = new Thickness(0, 0, 5, 0), Style = (Style)FindResource("RemoveButtonStyle") };
                removeButton.Click += (s, args) => RemoveTag_Click(tagContent, tagButton, tagStackPanel, width, height);

                tagStackPanel.Children.Add(tagTextBlock);
                tagStackPanel.Children.Add(removeButton);
                SelectedTagPanel.Children.Add(tagStackPanel);
            }
        }

        private void RemoveTag_Click(string tagContent, Button tagButton, StackPanel tagStackPanel, double width, double height)
        {
            SelectedTagPanel.Children.Remove(tagStackPanel);
            tagButton.Width = width;
            tagButton.Height = height;
            TagPanel.Children.Add(tagButton);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UserPurposes purposes = new UserPurposes();
            purposes.Show();
            this.Close();
        }
    }
}

