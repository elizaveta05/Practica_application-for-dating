using makets.Model.Model_chat;
using makets.Model.Model_users;
using makets.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace makets.pages
{
    public partial class Chat : Window
    {
        private readonly HttpClient _httpClient;
        private int _currentUserId, _selectedChat;
        private readonly ObservableCollection<ChatInfo> _chatList = new ObservableCollection<ChatInfo>();
        private ObservableCollection<Message> _messages = new ObservableCollection<Message>();
        private DateTime _lastMessageTimestamp;
        private DispatcherTimer _messagePollingTimer;
        private readonly ClientWebSocket _webSocket = new ClientWebSocket();
        private CancellationTokenSource _cancellationTokenSource;
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
            _currentUserId = userId;
            ChatListBox.ItemsSource = _chatList;

            if (userId > 0)
            {
                LoadUserChats(userId);
                InitializeWebSocket(); // Инициализация WebSocket
            }
        }
        private async void InitializeWebSocket()
        {
            try
            {
                // Подключаемся к WebSocket-серверу
                await _webSocket.ConnectAsync(new Uri($"wss://localhost:7036/api/chat/connect/{_currentUserId}"), CancellationToken.None);

                _cancellationTokenSource = new CancellationTokenSource();
                _ = ReceiveMessagesAsync(_cancellationTokenSource.Token); // Запускаем получение сообщений
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения WebSocket: {ex.Message}");
            }
        }

        private async Task ReceiveMessagesAsync(CancellationToken token)
        {
            var buffer = new byte[1024 * 4];
            while (_webSocket.State == WebSocketState.Open && !token.IsCancellationRequested)
            {
                var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), token);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Закрытие соединения", token);
                }
                else
                {
                    var messageJson = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    var newMessage = JsonConvert.DeserializeObject<Message>(messageJson);

                    // Добавляем новое сообщение в коллекцию и обновляем интерфейс
                    if (newMessage != null && newMessage.ChatId == _selectedChat && newMessage.UserSendingId != _currentUserId)
                    {
                        _messages.Add(newMessage);
                        Dispatcher.Invoke(DisplayMessages);
                    }
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _cancellationTokenSource?.Cancel();
            _webSocket?.Dispose();
        }
        private void ChatsTextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock clickedTextBlock = sender as TextBlock;

            if (clickedTextBlock != null)
            {
                switch (clickedTextBlock.Text)
                {
                    case "Профиль":
                        UserProfile window = new UserProfile(_currentUserId);
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
                    if (chats == null || chats.Count == 0)
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

        private async void ChatListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedChat = (ChatListBox.SelectedItem as ChatInfo)?.ChatId ?? 0;
            if (_selectedChat > 0)
            {
                // Получаем сообщения из чата 
                var response = await _httpClient.GetAsync($"Chat/messages/{_selectedChat}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var messages = JsonConvert.DeserializeObject<ObservableCollection<Message>>(jsonString);

                    if (messages != null)
                    {
                        _messages = messages;
                        //SelectedUserName.Text = _selectedUser.UserName;
                        _lastMessageTimestamp = _messages.LastOrDefault()?.TimeCreated ?? DateTime.MinValue;
                        SelectChatMessage.Visibility = Visibility.Collapsed;
                        MessageInputPanel.Visibility = Visibility.Visible;
                        DisplayMessages();
                    }
                }
                else
                {
                    MessageBox.Show($"Ошибка при загрузке сообщений: {response.StatusCode}");
                }
         
            }
            else
            {
                MessageBox.Show("ID чата не найден.");
                MessageInputPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void DisplayMessages()
        {
            MessagesPanel.Children.Clear();
            foreach (var message in _messages)
            {
                var messageStackPanel = new StackPanel
                {
                    Orientation = Orientation.Vertical,
                    HorizontalAlignment = (message.UserSendingId == _currentUserId) ? HorizontalAlignment.Right : HorizontalAlignment.Left,
                    Margin = new Thickness(5)
                };

                var messageBorder = new Border
                {
                    CornerRadius = new CornerRadius(10),
                    Background = (message.UserSendingId == _currentUserId) ? Brushes.LightBlue : Brushes.LightGreen,
                    Margin = new Thickness(5)
                };

                var messageTextBlock = new TextBlock
                {
                    Text = message.MessageText,
                    FontFamily = new FontFamily("Comic Sans MS"),
                    Padding = new Thickness(10)
                };

                var localTime = message.TimeCreated.ToLocalTime();
                var messageTimeBlock = new TextBlock
                {
                    Text = localTime.ToString("dd/MM/yyyy HH:mm"),
                    FontFamily = new FontFamily("Comic Sans MS"),
                    FontSize = 10,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Margin = new Thickness(5, 0, 5, 0)
                };

                messageBorder.Child = messageTextBlock;
                messageStackPanel.Children.Add(messageBorder);
                messageStackPanel.Children.Add(messageTimeBlock);
                MessagesPanel.Children.Add(messageStackPanel);
            }

            MessageScrollViewer.ScrollToEnd();
        }
        private async void SendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedChat <= 0 || string.IsNullOrWhiteSpace(MessageTextBox.Text))
            {
                MessageBox.Show("Выберите чат и введите сообщение.");
                return;
            }

            var newMessage = new Message
            {
                ChatId = _selectedChat,
                UserSendingId = _currentUserId,
                MessageText = MessageTextBox.Text,
                TimeCreated = DateTime.UtcNow
            };

            var json = JsonConvert.SerializeObject(newMessage);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("Chat/send", content);
                if (response.IsSuccessStatusCode)
                {
                    var resultContent = await response.Content.ReadAsStringAsync();
                    var sentMessage = JsonConvert.DeserializeObject<Message>(resultContent);

                    if (sentMessage != null)
                    {
                        // Добавляем сообщение в список сообщений
                        _messages.Add(sentMessage);
                        _lastMessageTimestamp = sentMessage.TimeCreated;
                        DisplayMessages();

                        // Обновляем последнее сообщение в списке чатов
                        var chat = _chatList.FirstOrDefault(c => c.ChatId == _selectedChat);
                        if (chat != null)
                        {
                            chat.LastMessage = $"{sentMessage.MessageText} ({sentMessage.TimeCreated:dd/MM/yyyy HH:mm})";
                        }

                        // Очищаем текстовое поле после отправки
                        MessageTextBox.Text = string.Empty;
                    }
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Ошибка при отправке сообщения: {response.StatusCode} - {errorContent}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}");
            }
        }

        private void InitializeMessagePolling()
        {
            _messagePollingTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(5)
            };
            _messagePollingTimer.Tick += async (sender, e) => await PollForNewMessages();
            _messagePollingTimer.Start();
        }

        private async Task PollForNewMessages()
        {
            if (_selectedChat > 0)
            {
                var response = await _httpClient.GetAsync(
                    $"chat/messages/{_selectedChat}?lastTimestamp={_lastMessageTimestamp:O}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var newMessages = JsonConvert.DeserializeObject<ObservableCollection<Message>>(jsonString);

                    if (newMessages != null && newMessages.Count > 0)
                    {
                        foreach (var message in newMessages)
                        {
                            _messages.Add(message);
                            _lastMessageTimestamp = message.TimeCreated;
                        }
                        DisplayMessages();
                    }
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Ошибка при загрузке сообщений: {response.StatusCode} - {errorContent}");
                }
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        public class ChatResponse
        {
            public int ChatId { get; set; }
        }
    }
}
