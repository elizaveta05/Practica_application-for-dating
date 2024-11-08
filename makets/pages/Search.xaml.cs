using makets.helper;
using makets.Model.Model_users;
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

        internal int currentUserId { get; set; } = int.MinValue;
        internal int currentListIndex { get; set; } = 0;
        internal List<DataUser>? _data = new();


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

        private async void UpdateInfo()
        {
           
            Username.Text = this._data[currentListIndex].Name;
            var userDescription = await APIService.GetUserDescription(this._data[currentListIndex].UserId);

            if (!string.IsNullOrEmpty(userDescription))
            {
                AboutUser.Text = "Не удалось получить данные о пользователе";

            }
            else
            {
                AboutUser.Text = userDescription;
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var data = await APIService.GetUsers();
            if (data is null || data.Count == 0)
            {
                MessageBox.Show("Ошибка при загрузке пользователей.");
                return;
            }
            this._data = data;
            UpdateInfo();  
        }

        private void LikeBtn_Click(object sender, RoutedEventArgs e)
        {
            this.currentListIndex++;
            this.currentUserId = this._data[currentListIndex].UserId;
            UpdateInfo();
        }
    }
}
