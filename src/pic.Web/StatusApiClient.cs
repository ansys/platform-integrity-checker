// ©2024, ANSYS Inc. Unauthorized use, distribution or duplication is prohibited.

using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace pic.Web;

public class StatusApiClient
{
    private readonly HttpClient _httpClient;

    public StatusApiClient(HttpClient httpClient) => 
        _httpClient = httpClient;
    
    public async Task<cislComponent[]> GetStatusAsync()
    {
        List<cislComponent>? components = null;

        await foreach (var component in _httpClient.GetFromJsonAsAsyncEnumerable<cislComponent>("/cislComponents"))
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

public record cislComponent (string Name, string? Url, string? Description, string Status);