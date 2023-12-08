using CodeReviews.Api.CodeReviews;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Wolverine;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseWolverine();

// Add services to the container.


builder.Services.AddDbContext<CodeReviewDbContext>();

var app = builder.Build();
// Configure the HTTP request pipeline.

app.UseHttpsRedirection();




app.MapGet("/pullrequests", ([FromServices] CodeReviewDbContext dbContext) =>
{
    var pullrequests = dbContext.PullRequests.ToList();

    return pullrequests;
});

app.MapPost("/pullrequests", async (CreatePullRequestRequest body, [FromServices] CodeReviewDbContext dbContext, CancellationToken ct) =>
{
    var pullrequest = new PullRequest()
    {
        Files = body.files.Select(f => new  PullRequestFile { Filename = f.Filename }).ToList(),
        Reviews = body.reviewers.Select(r => new PullRequestReview { IdReviewer = r.IdReviewer }).ToList()
    };

    await dbContext.AddAsync(pullrequest);
    await dbContext.SaveChangesAsync();
});


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


public record CreatePullRequestRequest(IEnumerable<CreatePullRequestFileRequest> files, IEnumerable<CreatePullRequestReviewersRequest> reviewers);
public record CreatePullRequestFileRequest(string Filename);
public record CreatePullRequestReviewersRequest(int IdReviewer);