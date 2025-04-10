// Copyright (C) 2025 ANSYS, Inc. and/or its affiliates.

var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.pic_ApiService>("apiservice");

builder.AddProject<Projects.pic_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
