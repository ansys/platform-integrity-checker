// ©2025, ANSYS Inc. Unauthorized use, distribution or duplication is prohibited.

namespace pic.ApiService;

internal record CislComponent(string Name, string? Url, string? Description, string Status = "Unknown");