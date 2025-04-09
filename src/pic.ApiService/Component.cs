// ©2025, ANSYS Inc. Unauthorized use, distribution or duplication is prohibited.

namespace pic.ApiService;

internal record Component(string Name, string? Url, string? Description, string Status = "Unknown");