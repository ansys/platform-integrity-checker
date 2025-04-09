// ©2025, ANSYS Inc. Unauthorized use, distribution or duplication is prohibited.

namespace pic.Web;

public class StatusApiClient
{
    private readonly HttpClient _httpClient;

    public StatusApiClient(HttpClient httpClient) => 
        _httpClient = httpClient;
    
    public async Task<Component[]> GetStatusAsync()
    {
        List<Component>? components = null;

        await foreach (var component in _httpClient.GetFromJsonAsAsyncEnumerable<Component>("/components"))
        {
            if (component is not null)
            {
                components ??= [];
                components.Add(component);
            }
        }

        return components?.ToArray() ?? [];
    }
}

public record Component (string Name, string? Url, string? Description, string Status);