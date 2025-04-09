// ©2024, ANSYS Inc. Unauthorized use, distribution or duplication is prohibited.

using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.pic_ApiService>("apiservice");

builder.AddProject<Projects.pic_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();
