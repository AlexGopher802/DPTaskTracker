using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using DPTaskTracker.Application.Models.Tasks;
using DPTaskTracker.Application.Models.Users;
using DPTaskTracker.Domain.Models;

namespace DPTaskTracker.Desktop;

public partial class EditTaskWindow : Window
{
    private readonly HttpClient _httpClient;
    private readonly TaskOutputDto _currentTask;
    private readonly IReadOnlyList<UserLoginOutputDto> _currentUsers;
    
    public EditTaskWindow(TaskOutputDto currentTask,
        IReadOnlyList<UserLoginOutputDto> currentUsers)
    {
        _currentTask = currentTask;
        _currentUsers = currentUsers;
        _httpClient = StaticSettings.GetHttpClient();

        InitializeComponent();
    }

    private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void EditTaskWindow_OnClosed(object? sender, EventArgs e)
    {
        Owner.IsEnabled = true;
    }

    private void EditTaskWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        TbName.Text = _currentTask.Name;
        TbDescription.Text = _currentTask.Description ?? string.Empty;

        CbUsers.ItemsSource = _currentUsers;
        CbUsers.DisplayMemberPath = "Username";
        CbUsers.SelectedItem = _currentUsers.FirstOrDefault(x => x.Id == _currentTask.UserId);

        CbStatuses.ItemsSource = Enum.GetValues<TaskStatus>();
        CbStatuses.SelectedItem = Enum.Parse<TaskStatus>(_currentTask.StatusCode);
    }

    private async void BtnConfirm_OnClick(object sender, RoutedEventArgs e)
    {
        var selectedUser = (UserLoginOutputDto)CbUsers.SelectedItem!;
        var selectedStatus = CbStatuses.SelectedItem.ToString();

        var inputDto = new TaskCreateInputDto(TbName.Text, TbDescription.Text, selectedUser.Id, selectedStatus);

        var response = await _httpClient.PutAsJsonAsync($"api/project/update-task/{_currentTask.Id}", inputDto);

        if (response.IsSuccessStatusCode)
        {
            MessageBox.Show("Задача успешно обновлена!");
            Close();
            (Owner as ProjectInfoWindow)?.LoadTasks();
        }
        else
        {
            MessageBox.Show("Произошла ошибка при обновлении задачи!");
        }
    }
}