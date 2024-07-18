using System.Net.Http.Json;
using Helpdesk.Api.Incidents;

using var httpClient = new HttpClient();
httpClient.BaseAddress = new Uri("http://localhost:5248/api/");
httpClient.Timeout = TimeSpan.FromSeconds(3);

var customerId = Guid.NewGuid().ToString();
Console.WriteLine($"Creating incidents for customerId: {customerId}");
NBomberRunner
   .RegisterScenarios(
        Scenario.Create("Create incident", async _ =>
            {
                var response = await httpClient.PostAsync($"customers/{customerId}/incidents",
                    JsonContent.Create(new LogIncidentRequest(new(ContactChannel.Email, "test", "test", "test@example.com", "12345456"), "Test incident")));
                response.EnsureSuccessStatusCode();
                return Response.Ok(true);
            })
           .WithoutWarmUp()
           .WithClean(async _ =>
            {
                var response = await httpClient.PostAsync($"customers/{customerId}/incidents/compare", null);
                Console.WriteLine($"{(response.IsSuccessStatusCode ? "OK" : "FAILED")}: {await response.Content.ReadAsStringAsync()}");
            })
           .WithLoadSimulations(Simulation.KeepConstant(10, TimeSpan.FromMinutes(3))))
   .Run();
