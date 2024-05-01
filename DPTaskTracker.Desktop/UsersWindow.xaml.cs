using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DPTaskTracker.Application.Models.Users;

namespace DPTaskTracker.Desktop;

public partial class UsersWindow : Window
{
    private readonly HttpClient _httpClient;

    public UsersWindow()
    {
        _httpClient = StaticSettings.GetHttpClient();

        InitializeComponent();
    }

    private void UsersWindow_OnClosed(object? sender, EventArgs e)
    {
        Owner.Show();
    }

    private void BtnExit_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private async void UsersWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        await LoadUsers();
    }
    
    public async Task LoadUsers()
    {
        var response = await _httpClient.GetAsync("api/user/get-all");

        if (response.IsSuccessStatusCode)
        {
            DataGridUsers.ClearValue(ItemsControl.ItemsSourceProperty);
            DataGridUsers.ItemsSource = await response.Content.ReadFromJsonAsync<List<UserLoginOutputDto>>();
        }
        else
        {
            MessageBox.Show("Произошла ошибка при загрузке проектов!");
        }
    }

    private void BtnUserAdd_OnClick(object sender, RoutedEventArgs e)
    {
        IsEnabled = false;
        new AddUserWindow { Owner = this }.Show();
    }
}