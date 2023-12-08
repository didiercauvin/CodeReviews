using Microsoft.AspNetCore.Mvc;
using Wolverine;
using Wolverine.Http;

namespace CodeReviews.Api.CodeReviews.CreatingPullRequest;

public record CreatePullRequest(string Title, IEnumerable<CreatePullRequestFile> Files, IEnumerable<CreatePullRequestReviewers> Reviewers);

public static class CreatePullRequestHandler
{
    [WolverinePost("/pullrequests")]
    public static async Task Handle(CreatePullRequestRequest request, CodeReviewDbContext dbContext, CancellationToken ct)
    {
        var (title, files, reviewers) = request;

        var pullrequest = new PullRequest()
        {
            Title = title,
            Files = files.Select(f => new PullRequestFile { Filename = f.Filename }).ToList(),
            Reviews = reviewers.Select(r => new PullRequestReview { IdReviewer = r.IdReviewer }).ToList()
        };

        await dbContext.AddAsync(pullrequest, ct);
        await dbContext.SaveChangesAsync(ct);
    }
}