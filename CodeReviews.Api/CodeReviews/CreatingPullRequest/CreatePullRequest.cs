using CodeReviews.Api.Core;
using Wolverine;
using Wolverine.EntityFrameworkCore;
using Wolverine.Http;

namespace CodeReviews.Api.CodeReviews.CreatingPullRequest;

public record CreatePullRequest(string Title, IEnumerable<CreatePullRequestFile> Files, IEnumerable<CreatePullRequestReviewers> Reviewers);

public record PullRequestCreated(string Title);

public static class CreatePullRequestHandler
{
    [WolverinePost("/pullrequests")]
    public static async Task Handle(CreatePullRequestRequest request, IDbContextOutbox<CodeReviewDbContext> outbox,
         CancellationToken ct)
    {
        var (title, files, reviewers) = request;

        var pullrequest = new PullRequest()
        {
            Title = title,
            Files = files.Select(f => new PullRequestFile { Filename = f.Filename }).ToList(),
            Reviews = reviewers.Select(r => new PullRequestReview { IdReviewer = r.IdReviewer }).ToList()
        };

        await outbox.DbContext.AddAsync(pullrequest, ct);

        await outbox.PublishAsync(new PullRequestCreated(title));

        await outbox.SaveChangesAndFlushMessagesAsync();
    }
}

public static class PullRequestCreatedHandler
{
    public static void Handle(PullRequestCreated @event)
    {
        Console.WriteLine($"la pull request '{@event.Title}' a été créée");
    }
}