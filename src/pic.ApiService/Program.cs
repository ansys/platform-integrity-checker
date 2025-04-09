// ©2025, ANSYS Inc. Unauthorized use, distribution or duplication is prohibited.

using System.Net;
using pic.ApiService;
using pic.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddProblemDetails();

var components = builder.Configuration
                        .GetSection("Components")
                        .Get<List<Component>>();

var app = builder.Build();

app.UseExceptionHandler();

app.MapGet("/components",
           async () =>
               await Task.WhenAll(components!.Select(async component =>
                                                     {
                                                         string status;

                                                         try
                                                         {
                                                             using HttpClient client = new();
                                                             var response = await client.GetAsync(component.Url).WaitAsync(TimeSpan.FromSeconds(3));
                                                             status = HttpStatusCode.OK == response.StatusCode ? "Up" : "Down";
                                                         }
                                                         catch (TimeoutException ex)
                                                         {
                                                             status = "Timeout";
                                                             app.Logger.LogError(ex, ex.Message);
                                                         }
                                                         catch (Exception ex)
                                                         {
                                                             status = "Unknown";
                                                             app.Logger.LogError(ex, ex.Message);
                                                         }

                                                         return component with { Status = status };
                                                     }))
          );

app.MapDefaultEndpoints();

app.Logger.LogInformation("Silencing probes {SilentProbes}",
                          builder.Configuration.GetValue<bool>("SilentProbes"));

app.Run();