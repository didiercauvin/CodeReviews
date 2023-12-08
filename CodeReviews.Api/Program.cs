using CodeReviews.Api.CodeReviews;
using CodeReviews.Api.CodeReviews.CreatingPullRequest;
using Microsoft.EntityFrameworkCore;
using Oakton.Resources;
using Wolverine;
using Wolverine.EntityFrameworkCore;
using Wolverine.Http;
using Wolverine.SqlServer;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseWolverine(opts =>
{
    // Setting up Sql Server-backed message storage
    // This requires a reference to Wolverine.SqlServer
    opts.PersistMessagesWithSqlServer("Server=localhost;Initial Catalog=demo-code-reviews;Integrated Security=true; TrustServerCertificate=True;");

    // Set up Entity Framework Core as the support
    // for Wolverine's transactional middleware
    opts.UseEntityFrameworkCoreTransactions();

    // Enrolling all local queues into the
    // durable inbox/outbox processing
    opts.Policies.UseDurableLocalQueues();

    opts.Policies.AutoApplyTransactions();

    opts.Policies.ForMessagesOfType<CreatePullRequestRequest>()
        .AddMiddleware(typeof(CodeReviewDoNothingMiddleware))
        .AddMiddleware(typeof(PullRequestLookupMiddleware));
});

builder.Services.AddResourceSetupOnStartup();

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();

builder.Services.AddDbContextWithWolverineIntegration<CodeReviewDbContext>(
    x => x.UseSqlServer("Server=localhost;Initial Catalog=demo-code-reviews;Integrated Security=true; TrustServerCertificate=True;"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger()
        .UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapWolverineEndpoints();

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