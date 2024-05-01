using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using DPTaskTracker.Application.Models.Users;

namespace DPTaskTracker.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static UserLoginOutputDto CurrentUser { get; private set; }

        private readonly HttpClient _httpClient;

        public MainWindow()
        {
            _httpClient = StaticSettings.GetHttpClient();

            InitializeComponent();
        }

        private async void Btn_login_OnClick(object sender, RoutedEventArgs e)
        {
            var login = tb_userName.Text;
            var password = pb_password.Password;

            var response = await _httpClient
                .PostAsJsonAsync("api/user/login", new UserLoginInputDto(login, password));

            if (response.IsSuccessStatusCode)
            {
                CurrentUser = await response.Content.ReadFromJsonAsync<UserLoginOutputDto>();

                Hide();
                new ProjectsWindow() { Owner = this }.Show();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль!");
            }
        }
    }
}