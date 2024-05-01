using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DPTaskTracker.Application.Models.Projects;
using DPTaskTracker.Domain.Models;

namespace DPTaskTracker.Desktop;

public partial class ProjectsWindow : Window
{
    private readonly HttpClient _httpClient;
    
    public ProjectsWindow()
    {
        _httpClient = StaticSettings.GetHttpClient();

        InitializeComponent();
    }

    private void ProjectsWindow_OnClosed(object? sender, EventArgs e)
    {
        Owner.Show();
    }

    private void BtnExit_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private async void ProjectsWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        if (MainWindow.CurrentUser.Role == UserRole.Admin)
        {
            BtnUsers.Visibility = Visibility.Visible;
        }

        await LoadProjects();
    }
    
    public async Task LoadProjects()
    {
        var response = await _httpClient.GetAsync("api/project/get-all");

        if (response.IsSuccessStatusCode)
        {
            DataGridProjects.ClearValue(ItemsControl.ItemsSourceProperty);
            DataGridProjects.ItemsSource = await response.Content.ReadFromJsonAsync<List<ProjectOutputDto>>();
        }
        else
        {
            MessageBox.Show("Произошла ошибка при загрузке проектов!");
        }
    }

    private void BtnUsers_OnClick(object sender, RoutedEventArgs e)
    {
        Hide();
        new UsersWindow { Owner = this }.Show();
    }

    private void BtnProjectAdd_OnClick(object sender, RoutedEventArgs e)
    {
        IsEnabled = false;
        new AddProjectWindow { Owner = this }.Show();
    }

    private void CmProperty_OnClick(object sender, RoutedEventArgs e)
    {
        var selectedProject = DataGridProjects.SelectedItem as ProjectOutputDto;
        Hide();
        new ProjectInfoWindow(selectedProject) { Owner = this }.Show();
    }
}