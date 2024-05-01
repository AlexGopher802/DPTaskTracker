using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using DPTaskTracker.Application.Models.Users;

namespace DPTaskTracker.Desktop;

public partial class AddUserProjectWindow : Window
{
    private readonly HttpClient _httpClient;

    private int _projectId;

    public AddUserProjectWindow(int projectId)
    {
        _httpClient = StaticSettings.GetHttpClient();
        _projectId = projectId;

        InitializeComponent();
    }

    private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void AddUserProjectWindow_OnClosed(object? sender, EventArgs e)
    {
        Owner.IsEnabled = true;
    }

    private async void AddUserProjectWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        var response = await _httpClient.GetAsync("api/user/get-all");

        if (response.IsSuccessStatusCode)
        {
            var users = await response.Content.ReadFromJsonAsync<List<UserLoginOutputDto>>();
            CbUsers.ItemsSource = users;
            CbUsers.DisplayMemberPath = "Username";
        }
        else
        {
            MessageBox.Show("Произошла ошибка при загрузке пользователей!");
        }
    }

    private async void BtnConfirm_OnClick(object sender, RoutedEventArgs e)
    {
        if (CbUsers.SelectedItem == null)
        {
            MessageBox.Show("Выберите пользователя!");
            return;
        }

        var user = (UserLoginOutputDto) CbUsers.SelectedItem;
        var response = await _httpClient.PostAsync($"api/project/{_projectId}/add-user/{user.Id}", null);

        if (response.IsSuccessStatusCode)
        {
            MessageBox.Show("Пользователь успешно добавлен к проекту!");
            Close();
            (Owner as ProjectInfoWindow)?.LoadUsers();
        }
        else
        {
            MessageBox.Show("Произошла ошибка при добавлении пользователя к проекту!");
        }
    }
}