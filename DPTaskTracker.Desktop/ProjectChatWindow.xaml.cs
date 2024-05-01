using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DPTaskTracker.Application.Models.Messages;
using DPTaskTracker.Application.Models.Users;

namespace DPTaskTracker.Desktop;

public partial class ProjectChatWindow : Window
{
    private readonly HttpClient _httpClient;
    private readonly int _projectId;

    public ProjectChatWindow(int projectId)
    {
        _httpClient = StaticSettings.GetHttpClient();
        _projectId = projectId;

        InitializeComponent();
    }

    private void BtnExit_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void ProjectChatWindow_OnClosed(object? sender, EventArgs e)
    {
        Owner.Show();
    }

    private async void ProjectChatWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        await LoadMessages();
    }

    public async Task LoadMessages()
    {
        var response = await _httpClient.GetAsync($"api/message/{_projectId}/get-all");

        if (response.IsSuccessStatusCode)
        {
            var messages = await response.Content.ReadFromJsonAsync<List<MessageOutputDto>>();

            DataGridMessages.ClearValue(ItemsControl.ItemsSourceProperty);
            DataGridMessages.ItemsSource = messages;
        }
        else
        {
            MessageBox.Show("Произошла ошибка при загрузке пользователей!");
        }
    }

    private void BtnMessageAdd_OnClick(object sender, RoutedEventArgs e)
    {
        IsEnabled = false;
        new AddMessageWindow(_projectId) { Owner = this }.Show();
    }
}