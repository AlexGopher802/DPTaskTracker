using System.Net.Http;

namespace DPTaskTracker.Desktop;

public static class StaticSettings
{
    public const string ApiRoute = "http://localhost:5273/";

    private static HttpClient _httpClient;

    public static HttpClient GetHttpClient() => _httpClient ??= new HttpClient() { BaseAddress = new(ApiRoute) };
}