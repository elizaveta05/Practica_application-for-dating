using makets.helper;
using Newtonsoft.Json;
using makets.Model.Model_users;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace makets.pages
{
    public partial class NewUser : Window
    {
        private readonly HttpClient _httpClient;
        private int _userId;
        private int _selectedCityId;

        public NewUser(int userId)
        {
            InitializeComponent();
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7036/api/")
            };
            _userId = userId; // Сохраняем userId для передачи на сервер
            LoadCitiesAsync(); // Загрузка городов при инициализации
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateForm())
                return;

            string fullName = tb_name.Text.Trim();
            var nameParts = fullName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (nameParts.Length < 2 || nameParts.Length > 3)
            {
                MessageBox.Show("ФИО должно содержать хотя бы фамилию и имя.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string lastName = nameParts[0];
            string firstName = nameParts[1];
            string patronymic = nameParts.Length == 3 ? nameParts[2] : null;


            string selectedGender = (MaleOrFemale.SelectedItem as ComboBoxItem)?.Content.ToString();
            int genderId = selectedGender == "Мужской" ? 1 : 2;

            var dataUser = new 
            {
                LastName = lastName,
                FirstName = firstName,
                Patronymic = patronymic,
                DateOfBirth = datePicker.SelectedDate.Value.ToString("yyyy-MM-dd"), 
                GenderId = genderId, 
                LocationId = _selectedCityId, 
                UdrId = _userId 
            };

            await SendDataToServerAsync(dataUser);
        }

        private async Task SendDataToServerAsync(Object dataUser)
        {
            //Конвентируем данные перед отправкой
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(dataUser);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            //Отправляем на серверную часть в контроллер
            var response = await _httpClient.PostAsync("Autho/newDataUser", content);
            //Ответ от сервера
            if (response.IsSuccessStatusCode)
            {
                var resultContent = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<dynamic>(resultContent);
                int resultUserId = result.userId;//Получаем id новой записи

                //Переход происходит только в случае, если сервер возвращает в ответ новый id, который не может быть null
                if(resultUserId.ToString() != null)
                {
                    UserTags userTags = new UserTags(resultUserId);
                    userTags.Show();
                    this.Close();
                }
            }
            else
            {
                string error = await response.Content.ReadAsStringAsync();
                MessageBox.Show($"Ошибка при отправке данных: {error}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
                var response = await _httpClient.GetAsync("Autho/cities");

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

        public class Location
        {
            public int LocationId { get; set; }
            public string LocationName { get; set; }
        }
    }
}
