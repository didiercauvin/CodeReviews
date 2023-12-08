using Wolverine.Http;

namespace CodeReviews.Api.CodeReviews.GettingPulllRequests;

public static class GetPullRequestsEndpoint
{
    [WolverineGet("/pullrequests")]
    public static PullRequest[] GetAll(CodeReviewDbContext dbContext)
    {
        return dbContext.PullRequests.ToArray();
    }
}