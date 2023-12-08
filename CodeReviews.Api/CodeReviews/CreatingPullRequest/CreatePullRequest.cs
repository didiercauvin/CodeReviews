using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace CodeReviews.Api.CodeReviews.CreatingPullRequest;

public record CreatePullRequest(string Title, IEnumerable<CreatePullRequestFile> Files, IEnumerable<CreatePullRequestReviewers> Reviewers);

public static class CreatePullRequestHandler
{
    public static async Task Handle(CreatePullRequest command, CodeReviewDbContext dbContext, CancellationToken ct)
    {
        var pullrequest = new PullRequest()
        {
            Title = command.Title,
            Files = command.Files.Select(f => new PullRequestFile { Filename = f.Filename }).ToList(),
            Reviews = command.Reviewers.Select(r => new PullRequestReview { IdReviewer = r.IdReviewer }).ToList()
        };

        await dbContext.AddAsync(pullrequest, ct);
        await dbContext.SaveChangesAsync(ct);
    }
}

public static class CreatePullRequestEndpoint
{
    public static IEndpointRouteBuilder UseCreatePullRequestEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/pullrequests", ([FromBody] CreatePullRequestRequest request, IMessageBus bus) =>
        {
            var (title, files, reviewers) = request;
            return bus.InvokeAsync(new CreatePullRequest(request.Title, files, reviewers));
        });

        return endpoints;
    }
}