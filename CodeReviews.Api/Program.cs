using CodeReviews.Api.CodeReviews;
using CodeReviews.Api.CodeReviews.CreatingPullRequest;
using Oakton.Resources;
using Wolverine;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseWolverine();

builder.Services.AddResourceSetupOnStartup();

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();

builder.Services.AddDbContext<CodeReviewDbContext>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger()
        .UseSwaggerUI();
}

app.UseHttpsRedirection()
    .UseRouting()
    .UseEndpoints(endpoints => endpoints.UsePullRequestEndpoints());

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
});

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}


public record CreatePullRequestRequest(string Title, IEnumerable<CreatePullRequestFile> Files, IEnumerable<CreatePullRequestReviewers> Reviewers);
public record CreatePullRequestFile(string Filename);
public record CreatePullRequestReviewers(int IdReviewer);