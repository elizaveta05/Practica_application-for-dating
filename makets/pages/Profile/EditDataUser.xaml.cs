using makets.Model.Model_users;
using makets.pages.Profile;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
using System.Xml;

namespace makets.pages
{
    /// <summary>
    /// Логика взаимодействия для EditDataUser.xaml
    /// </summary>
    public partial class EditDataUser : Window
    {
        private int userId;
        private readonly HttpClient _httpClient;
        private int _selectedCityId;
        private DataUser _originalUserData;
        private int UdrId;

        public EditDataUser(int userId)
        {
            InitializeComponent();
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7036/api/")
            };
            this.userId = userId;
            LoadDataAsync(); // Асинхронная загрузка городов и данных пользователя
        }

        // Метод загрузки городов и данных о пользователе
        private async Task LoadDataAsync()
        {
            await LoadCitiesAsync(); // Загрузка городов
            await LoadData(userId); // Загрузка данных пользователя после загрузки городов
        }

        //Метод загрузки данных в combobox о доступных городах
        private async Task LoadCitiesAsync()
        {
            var cities = await GetCitySuggestionsAsync();
            cb_city.ItemsSource = cities;
            cb_city.DisplayMemberPath = "LocationName";
            cb_city.SelectedValuePath = "LocationId";
        }

        //Метод выбора конкретного города с сохранением id выбора
        private void CitySuggestions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cb_city.SelectedItem is Location selectedCity)
            {
                _selectedCityId = selectedCity.LocationId; // Сохраняем ID выбранного города
            }
        }

        //Метод отправки запроса на получения списка всех доступных городов
        private async Task<List<Location>> GetCitySuggestionsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("Registration/cities");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<Location>>(content);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка запроса: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return new List<Location>();
        }

        //Метод загрузки данных о пользователе
        private async Task LoadData(int userId)
        {
            //Конвентируем данные перед отправкой
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(userId);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            //Отправляем на серверную часть в контроллер
            var response = await _httpClient.PostAsync("Profile/getDataUser", content);

            if (response.IsSuccessStatusCode)
            {
                // Чтение и парсинг ответа
                var resultContent = await response.Content.ReadAsStringAsync();
                var userData = JsonConvert.DeserializeObject<DataUser>(resultContent);

                if (userData != null)
                {
                    // Заполняем поля данными пользователя
                    tb_name.Text = $"{userData.LastName.Trim()} {userData.FirstName.Trim()} {userData.Patronymic?.Trim() ?? ""}";
                    datePicker.SelectedDate = DateTime.Parse(userData.DateOfBirth);

                    // Устанавливаем пол
                    if (userData.GenderId == 1)
                        MaleOrFemale.SelectedIndex = 0; // Мужской
                    else if (userData.GenderId == 2)
                        MaleOrFemale.SelectedIndex = 1; // Женский

                    // Устанавливаем выбранный город
                    cb_city.SelectedIndex= userData.LocationId - 1;
                    UdrId = userData.UdrId;
                    // Сохраняем оригинальные данные для проверки изменений
                    _originalUserData = userData;
                }
                else
                {
                    MessageBox.Show("Данные профиля пользователя не загружаются.");
                }

            }
            else
            {
                MessageBox.Show("Ошибка загрузки данных о пользователе.");
            }
        }
    
        public class Location
        {
            public int LocationId { get; set; }
            public string LocationName { get; set; }
        }

        // Метод для проверки изменений
        private bool HasChanges()
        {
            var currentUserData = GetCurrentUserData();

            return currentUserData.FirstName != _originalUserData.FirstName ||
                   currentUserData.LastName != _originalUserData.LastName ||
                   currentUserData.Patronymic != _originalUserData.Patronymic ||
                   currentUserData.DateOfBirth != _originalUserData.DateOfBirth ||
                   currentUserData.GenderId != _originalUserData.GenderId ||
                   currentUserData.LocationId != _originalUserData.LocationId;
        }

        // Метод для получения текущих данных из элементов управления
        private DataUser GetCurrentUserData()
        {
            return new DataUser
            {
                UserId = userId,
                FirstName = tb_name.Text.Split(' ')[0],
                LastName = tb_name.Text.Split(' ')[1],
                Patronymic = tb_name.Text.Split(' ').Length > 2 ? tb_name.Text.Split(' ')[2] : null,
                DateOfBirth = datePicker.SelectedDate.Value.ToString("yyyy-MM-dd"),
                GenderId = MaleOrFemale.SelectedIndex + 1,
                LocationId = _selectedCityId,
                UdrId = UdrId
            };
        }

        private async void Button_Click(object sender, RoutedEventArgs e) // Кнопка "Сохранить"
        {
            await SaveUserData();
            EditUserTags editUserTags = new EditUserTags();
            editUserTags.Show();
            this.Close();
        }

        private async void Button1_Click(object sender, RoutedEventArgs e) // Кнопка "Вернуться в профиль"
        {
            if (HasChanges())
            {
                var result = MessageBox.Show("Есть несохраненные изменения. Сохранить изменения?", "Подтвердите", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    await SaveUserData(); // Сохранение данных
                }
            }

            UserProfile userProfile = new UserProfile(userId);
            userProfile.Show();
            this.Close();
        }

        private async Task<bool> SaveUserData()
        {
            if (HasChanges())
            {
                var currentUserData = GetCurrentUserData();
                if (!ValidateForm())
                    return false; // Если форма не валидна, не продолжаем

                var json = JsonConvert.SerializeObject(currentUserData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("Profile/updateUserData", content);

                if (response.IsSuccessStatusCode)
                {
                    return true; // Сохраняем успешно
                }
                else
                {
                    MessageBox.Show("Ошибка при сохранении данных.");
                    return false; // Ошибка при сохранении
                }
            }
            else
            {
                MessageBox.Show("Нет изменений для сохранения.");
                return false; // Нет изменений
            }
        }


        //Метод проверки данных
        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(tb_name.Text))
            {
                MessageBox.Show("Пожалуйста, введите ФИО.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            var nameParts = tb_name.Text.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (nameParts.Length != 2 && nameParts.Length != 3)
            {
                MessageBox.Show("ФИО должно содержать либо Фамилию и Имя, либо Фамилию, Имя и Отчество.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (datePicker.SelectedDate == null)
            {
                MessageBox.Show("Пожалуйста, выберите дату рождения.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if ((DateTime.Now.Year - datePicker.SelectedDate.Value.Year) > 100)
            {
                MessageBox.Show("Дата рождения не может быть более 100 лет назад.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if ((DateTime.Now.Year - datePicker.SelectedDate.Value.Year) < 18)
            {
                MessageBox.Show("Пользователь должен быть старше 18 лет.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (MaleOrFemale.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите пол.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (cb_city.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите город.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

    }
}
