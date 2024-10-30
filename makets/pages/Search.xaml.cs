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
    /// Логика взаимодействия для Search.xaml
    /// </summary>
    public partial class Search : Window
    {
        public Search()
        {
            InitializeComponent();
        }

        private void ChatsTextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock clickedTextBlock = sender as TextBlock;

            if (clickedTextBlock != null)
            {
                switch (clickedTextBlock.Text)
                {
                    /*
                    case "Чаты":
                        Chat chat = new Chat(userId);
                        chat.Show();
                        this.Close();
                        break;

                    case "Найти собеседника":
                        Search search = new Search();
                        search.Show();
                        this.Close();
                        break;
                    */
                }
            }

        }
    }
}
