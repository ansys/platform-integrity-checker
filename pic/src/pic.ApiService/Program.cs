// ©2024, ANSYS Inc. Unauthorized use, distribution or duplication is prohibited.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

// Bind components from appsettings.json
var components = builder.Configuration.GetSection("Components").Get<List<cislComponent>>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

app.MapGet("/cislcomponents",
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

app.Logger.LogInformation("Silencing probes {SilentProbes}", builder.Configuration.GetValue<bool>("SilentProbes"));

app.Run();

// Define the record for components
record cislComponent(string Name, string? Url, string? Description, string Status = "Unknown");