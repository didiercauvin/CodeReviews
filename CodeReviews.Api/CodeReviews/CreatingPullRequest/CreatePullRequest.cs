using Microsoft.AspNetCore.Mvc;

namespace CodeReviews.Api.CodeReviews.CreatingPullRequest;

public static class CreatePullRequestEndpoint
{
    public static IEndpointRouteBuilder UseCreatePullRequestEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/pullrequests", async ([FromBody] CreatePullRequestRequest body, [FromServices] CodeReviewDbContext dbContext, CancellationToken ct) =>
        {
            var pullrequest = new PullRequest()
            {
                Files = body.files.Select(f => new PullRequestFile { Filename = f.Filename }).ToList(),
                Reviews = body.reviewers.Select(r => new PullRequestReview { IdReviewer = r.IdReviewer }).ToList()
            };

            await dbContext.AddAsync(pullrequest, ct);
            await dbContext.SaveChangesAsync(ct);
        });

        return endpoints;
    }
}