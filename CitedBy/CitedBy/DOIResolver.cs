using System.Net.Http.Json;

namespace CitedBy;

public static class DOIResolver
{
    private const string Endpoint = "api/handles";
    private static readonly HttpClient HttpClient;

    static DOIResolver()
    {
        HttpClient = new HttpClient();
        HttpClient.BaseAddress = new Uri("https://doi.org");
    }

    public static async Task<DoiResponse?> ResolveAsync(string doi,
        CancellationToken cancellationToken = default)
    {
        var url = $"{Endpoint}/{doi}";
        var response = await HttpClient.GetAsync(url, cancellationToken);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<DoiResponse>(
            cancellationToken);
    }

    public static async Task<Uri?> ResolveUriAsync(string doi,
        CancellationToken cancellationToken = default)
    {
        var response = await ResolveAsync(doi, cancellationToken);
        if (response == null) return null;
        foreach (var value in response.Values)
            if (value.Type == "URL")
            {
                var uri = new Uri(value.Data.Value.ToString() ?? string.Empty);
                return uri;
            }

        return null;
    }
}

public class DoiResponse
{
    public int ResponseCode { get; set; }
    public string Handle { get; set; } = string.Empty;
    public List<DoiValue> Values { get; set; } = new();
}

public class DoiValue
{
    public int Index { get; set; }
    public string Type { get; set; } = string.Empty;
    public DoiData Data { get; set; } = new();
    public int Ttl { get; set; }
    public DateTime Timestamp { get; set; }
}

public class DoiData
{
    public string Format { get; set; } = string.Empty;
    public object Value { get; set; } = new();
}