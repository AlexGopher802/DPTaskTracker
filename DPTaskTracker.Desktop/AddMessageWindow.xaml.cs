using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using DPTaskTracker.Application.Models.Messages;

namespace DPTaskTracker.Desktop;

public partial class AddMessageWindow : Window
{
    private readonly HttpClient _httpClient;
    private readonly int _projectId;

    public AddMessageWindow(int projectId)
    {
        _httpClient = StaticSettings.GetHttpClient();
        _projectId = projectId;

        InitializeComponent();
    }

    private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void AddMessageWindow_OnClosed(object? sender, EventArgs e)
    {
        Owner.IsEnabled = true;
    }

    private async void BtnConfirm_OnClick(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TbText.Text))
        {
            MessageBox.Show("Введите текст сообщения!");
            return;
        }

        var message = new MessageCreateInputDto(TbText.Text, MainWindow.CurrentUser.Id, _projectId);

        var response = await _httpClient.PostAsJsonAsync("api/message/create", message);
        if (response.IsSuccessStatusCode)
        {
            Close();
            (Owner as ProjectChatWindow)?.LoadMessages();
        }
        else
        {
            MessageBox.Show("Произошла ошибка при добавлении сообщения!");
        }
    }
}