using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using DPTaskTracker.Application.Models.Projects;

namespace DPTaskTracker.Desktop;

public partial class EditProjectWindow : Window
{
    private readonly HttpClient _httpClient;
    private readonly ProjectOutputDto _currentProject;

    public EditProjectWindow(ProjectOutputDto currentProject)
    {
        _httpClient = StaticSettings.GetHttpClient();
        _currentProject = currentProject;

        InitializeComponent();
    }

    private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void EditProjectWindow_OnClosed(object? sender, EventArgs e)
    {
        Owner.IsEnabled = true;
    }

    private void EditProjectWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        TbName.Text = _currentProject.Name;
        TbDescription.Text = _currentProject.Description ?? string.Empty;
        DpDeadline.SelectedDate = _currentProject.Deadline;
    }

    private async void BtnConfirm_OnClick(object sender, RoutedEventArgs e)
    {
        var project = new ProjectCreateInputDto(TbName.Text, TbDescription.Text, DpDeadline.SelectedDate ?? DateTime.Now);

        var response = await _httpClient.PutAsJsonAsync($"api/project/{_currentProject.Id}/update", project);

        if (response.IsSuccessStatusCode)
        {
            MessageBox.Show("Проект успешно обновлен!");
            Close();
        }
        else
        {
            MessageBox.Show("Произошла ошибка при обновлении проекта!");
        }
    }
}