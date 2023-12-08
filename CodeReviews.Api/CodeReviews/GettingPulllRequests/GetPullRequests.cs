using Microsoft.AspNetCore.Mvc;

namespace CodeReviews.Api.CodeReviews.GettingPulllRequests;

public static class GetPullRequestsEndpoint
{
    public static IEndpointRouteBuilder UseGetPullRequestsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/pullrequests", ([FromServices] CodeReviewDbContext dbContext) =>
        {
            var pullrequests = dbContext.PullRequests.ToList();

            return pullrequests;
        });

        return endpoints;
    }
}