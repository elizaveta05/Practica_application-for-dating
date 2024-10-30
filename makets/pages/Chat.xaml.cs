using makets.Model.Model_chat;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
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
    /// Логика взаимодействия для Chat.xaml
    /// </summary>
    public partial class Chat : Window
    {
        private readonly HttpClient _httpClient;
        private int userId;
        private readonly ObservableCollection<ChatInfo> _chatList = new ObservableCollection<ChatInfo>();

        public Chat(int userId)
        {
            InitializeComponent();
            _httpClient = new HttpClient(new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (msg, cert, chain, param) => true
            })
            {
                BaseAddress = new Uri("https://localhost:7036/api/")
            };
            this.userId = userId;
            ChatListBox.ItemsSource = _chatList;

            if(userId.ToString() != null)
            {
                LoadUserChats(userId);
            }

        }
        private void ChatsTextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock clickedTextBlock = sender as TextBlock;

            if (clickedTextBlock != null)
            {
                switch (clickedTextBlock.Text)
                {
                    case "Профиль":
                        UserProfile window = new UserProfile(userId);
                        window.Show();
                        this.Close();
                        break;

                    case "Найти собеседника":
                        Search search = new Search();
                        search.Show();
                        this.Close();
                        break;
                }
            }
        }

        private async void LoadUserChats(int userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"Chat/getUserChats/{userId}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var chats = JsonConvert.DeserializeObject<List<ChatInfo>>(jsonString);
                    if(chats == null || chats.Count == 0)
                    {
                        MessageBox.Show("Чатов нет");
                        return;
                    }
                    _chatList.Clear();
                    foreach (var chat in chats)
                    {
                        _chatList.Add(new ChatInfo
                        {
                            ChatId = chat.ChatId,
                            OtherUser = chat.OtherUser,
                            LastMessage = chat.LastMessage
                        });
                    }
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Ошибка при загрузке чатов: {response.StatusCode} - {errorContent}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}");
            }
        }

    }
}
