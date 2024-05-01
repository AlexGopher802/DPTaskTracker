using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using DPTaskTracker.Application.Models.Tasks;
using DPTaskTracker.Application.Models.Users;

namespace DPTaskTracker.Desktop;

public partial class AddTaskWindow : Window
{
    private readonly IReadOnlyList<UserLoginOutputDto> _currentUsers;
    private readonly int _projectId;
    private readonly HttpClient _httpClient;

    public AddTaskWindow(IReadOnlyList<UserLoginOutputDto> currentUsers,
        int projectId)
    {
        _currentUsers = currentUsers;
        _projectId = projectId;
        _httpClient = StaticSettings.GetHttpClient();

        InitializeComponent();
    }

    private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void AddTaskWindow_OnClosed(object? sender, EventArgs e)
    {
        Owner.IsEnabled = true;
    }

    private async void BtnConfirm_OnClick(object sender, RoutedEventArgs e)
    {
        if (CbUsers.SelectedItem == null)
        {
            MessageBox.Show("Выберите пользователя!");
            return;
        }

        if (string.IsNullOrWhiteSpace(TbName.Text))
        {
            MessageBox.Show("Введите название задачи!");
            return;
        }

        var user = (UserLoginOutputDto) CbUsers.SelectedItem;
        var taskCreateInputDto = new TaskCreateInputDto(TbName.Text, TbDescription.Text, user.Id);

        var response = await _httpClient.PostAsJsonAsync($"api/project/{_projectId}/add-task", taskCreateInputDto);

        if (response.IsSuccessStatusCode)
        {
            MessageBox.Show("Задача успешно добавлена!");
            Close();
            (Owner as ProjectInfoWindow)?.LoadTasks();
        }
        else
        {
            MessageBox.Show("Произошла ошибка при добавлении пользователя к проекту!");
        }
    }

    private void AddTaskWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        CbUsers.ItemsSource = _currentUsers;
        CbUsers.DisplayMemberPath = "Username";
    }
}