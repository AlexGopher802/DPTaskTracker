using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DPTaskTracker.Application.Models.Projects;
using DPTaskTracker.Application.Models.Tasks;
using DPTaskTracker.Application.Models.Users;

namespace DPTaskTracker.Desktop;

public partial class ProjectInfoWindow : Window
{
    private readonly HttpClient _httpClient;
    private readonly ProjectOutputDto _currentProject;

    private List<UserLoginOutputDto> CurrentUsers { get; set; }

    public ProjectInfoWindow(ProjectOutputDto project)
    {
        _httpClient = StaticSettings.GetHttpClient();
        _currentProject = project;

        InitializeComponent();
    }

    private void BtnExit_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void ProjectInfoWindow_OnClosed(object? sender, EventArgs e)
    {
        Owner.Show();
    }

    private async void ProjectInfoWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        await LoadProject();
        await LoadTasks();
        await LoadUsers();
    }

    public async Task LoadTasks()
    {
        var response = await _httpClient.GetAsync($"api/project/{_currentProject.Id}/get-tasks");

        if (response.IsSuccessStatusCode)
        {
            DataGridTasks.ClearValue(ItemsControl.ItemsSourceProperty);
            DataGridTasks.ItemsSource = await response.Content.ReadFromJsonAsync<List<TaskOutputDto>>();
        }
        else
        {
            MessageBox.Show("Произошла ошибка при загрузке задач!");
        }
    }
    
    public async Task LoadProject()
    {
        TbWelcome.Text = $"Проект: {_currentProject.Name}";
    }
    
    public async Task LoadUsers()
    {
        var response = await _httpClient.GetAsync($"api/project/{_currentProject.Id}/get-users");

        if (response.IsSuccessStatusCode)
        {
            var users = await response.Content.ReadFromJsonAsync<List<UserLoginOutputDto>>();
            CurrentUsers = users;

            DataGridUsers.ClearValue(ItemsControl.ItemsSourceProperty);
            DataGridUsers.ItemsSource = users;
        }
        else
        {
            MessageBox.Show("Произошла ошибка при загрузке пользователей!");
        }
    }

    private void BtnUserAdd_OnClick(object sender, RoutedEventArgs e)
    {
        IsEnabled = false;
        new AddUserProjectWindow(_currentProject.Id) { Owner = this }.Show();
    }

    private async void CmUserRemove_OnClick(object sender, RoutedEventArgs e)
    {
        var selectedUser = (UserLoginOutputDto) DataGridUsers.SelectedItem;

        if (selectedUser == null)
        {
            MessageBox.Show("Выберите пользователя!");
            return;
        }

        var response = await _httpClient.PostAsync($"api/project/{_currentProject.Id}/remove-user/{selectedUser.Id}", null);

        if (response.IsSuccessStatusCode)
        {
            MessageBox.Show("Пользователь успешно исключен из проекта!");
            await LoadUsers();
        }
        else
        {
            MessageBox.Show("Произошла ошибка при исключении пользователя из проекта!");
        }
    }

    private void BtnTaskAdd_OnClick(object sender, RoutedEventArgs e)
    {
        if (CurrentUsers.Count == 0)
        {
            MessageBox.Show("В проекте нет пользователей!");
            return;
        }

        IsEnabled = false;
        new AddTaskWindow(CurrentUsers, _currentProject.Id) { Owner = this }.Show();
    }

    private void BtnUpdateProject_OnClick(object sender, RoutedEventArgs e)
    {
        IsEnabled = false;
        new EditProjectWindow(_currentProject) { Owner = this }.Show();
    }

    private void CmUpdateTask_OnClick(object sender, RoutedEventArgs e)
    {
        var selectedTask = (TaskOutputDto) DataGridTasks.SelectedItem;

        if (selectedTask == null)
        {
            MessageBox.Show("Выберите задачу!");
            return;
        }

        IsEnabled = false;
        new EditTaskWindow(selectedTask, CurrentUsers) { Owner = this }.Show();
    }

    private void BtnProjectChat_OnClick(object sender, RoutedEventArgs e)
    {
        Hide();
        new ProjectChatWindow(_currentProject.Id) { Owner = this }.Show();
    }
}