using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using DPTaskTracker.Application.Models.Users;

namespace DPTaskTracker.Desktop;

public partial class AddUserWindow : Window
{
    private readonly HttpClient _httpClient;
    
    public AddUserWindow()
    {
        _httpClient = StaticSettings.GetHttpClient();
        
        InitializeComponent();
    }

    private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void AddUserWindow_OnClosed(object? sender, EventArgs e)
    {
        Owner.IsEnabled = true;
    }

    private async void BtnConfirm_OnClick(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TbUsername.Text) ||
            string.IsNullOrWhiteSpace(TbEmail.Text) ||
            string.IsNullOrWhiteSpace(PbPassword.Password))
        {
            MessageBox.Show("Заполните все поля!");
            return;
        }

        var inputDto = new UserRegisterInputDto(TbUsername.Text, TbEmail.Text, PbPassword.Password);
        var response = await _httpClient.PostAsJsonAsync("api/user/register", inputDto);

        if (response.IsSuccessStatusCode)
        {
            MessageBox.Show("Пользователь успешно добавлен!");
            Close();
            (Owner as UsersWindow)?.LoadUsers();
        }
        else
        {
            MessageBox.Show("Произошла ошибка при добавлении пользователя!");
        }
    }
}