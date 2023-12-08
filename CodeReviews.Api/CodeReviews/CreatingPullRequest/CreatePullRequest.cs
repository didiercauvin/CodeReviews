using CodeReviews.Api.Core;
using Wolverine;
using Wolverine.Http;

namespace CodeReviews.Api.CodeReviews.CreatingPullRequest;

public record CreatePullRequest(string Title, IEnumerable<CreatePullRequestFile> Files, IEnumerable<CreatePullRequestReviewers> Reviewers);

public record PullRequestCreated(string Title);

public static class CreatePullRequestHandler
{
    [WolverinePost("/pullrequests")]
    public static async Task Handle(CreatePullRequestRequest request, IMessageBus bus,
        CodeReviewDbContext dbContext, CancellationToken ct)
    {
        var (title, files, reviewers) = request;

        var pullrequest = new PullRequest()
        {
            Title = title,
            Files = files.Select(f => new PullRequestFile { Filename = f.Filename }).ToList(),
            Reviews = reviewers.Select(r => new PullRequestReview { IdReviewer = r.IdReviewer }).ToList()
        };

        await dbContext.AddAndSaveAsync(pullrequest, ct);

        await bus.PublishAsync(new PullRequestCreated(title));
    }
}

public static class PullRequestCreatedHandler
{
    public static void Handle(PullRequestCreated @event)
    {
        Console.WriteLine($"la pull request '{@event.Title}' a été créée");
    }
}