using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using DPTaskTracker.Application.Models.Projects;

namespace DPTaskTracker.Desktop;

public partial class AddProjectWindow : Window
{
    private readonly HttpClient _httpClient;
    
    public AddProjectWindow()
    {
        _httpClient = StaticSettings.GetHttpClient();

        InitializeComponent();
    }

    private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void AddProjectWindow_OnClosed(object? sender, EventArgs e)
    {
        Owner.IsEnabled = true;
    }

    private async void BtnConfirm_OnClick(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TbName.Text) ||
            string.IsNullOrWhiteSpace(TbDescription.Text) ||
            string.IsNullOrWhiteSpace(DpDeadline.Text))
        {
            MessageBox.Show("Заполните все поля!");
            return;
        }

        if (!DateTime.TryParse(DpDeadline.Text, out var deadline))
        {
            MessageBox.Show("Некорректная дата!");
            return;
        }

        var project = new ProjectCreateInputDto(TbName.Text, TbDescription.Text, deadline);

        var response = await _httpClient.PostAsJsonAsync("api/project/create", project);

        if (response.IsSuccessStatusCode)
        {
            MessageBox.Show("Проект успешно создан!");
            Close();
            (Owner as ProjectsWindow)?.LoadProjects();
        }
        else
        {
            MessageBox.Show("Произошла ошибка при создании проекта!");
        }
    }
}