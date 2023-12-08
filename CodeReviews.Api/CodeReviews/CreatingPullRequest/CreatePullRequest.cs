using CodeReviews.Api.Core;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Wolverine;
using Wolverine.EntityFrameworkCore;
using Wolverine.ErrorHandling;
using Wolverine.Http;
using Wolverine.Runtime;

namespace CodeReviews.Api.CodeReviews.CreatingPullRequest;

public record CreatePullRequest(string Title, IEnumerable<CreatePullRequestFile> Files, IEnumerable<CreatePullRequestReviewers> Reviewers);

public record PullRequestCreated(string Title);
public record PullRequestRefused(string Title);

public static class CreatePullRequestHandler
{
    public static ProblemDetails Before(CreatePullRequest request)
    {
        if (request.Title.Contains("PR"))
            return new ProblemDetails
            {
                Detail = "L'abreviation PR est interdite!",
                Status = 400
            };

        return WolverineContinue.NoProblems;
    }

    [WolverinePost("/pullrequests")]
    public static async Task Handle(CreatePullRequest request, IDbContextOutbox<CodeReviewDbContext> outbox, IMessageBus bus,
         CancellationToken ct)
    {
        var (title, files, reviewers) = request;

        var pullrequest = PullRequest.Create(title,
            files.Select(f => new PullRequestFile { Filename = f.Filename }).ToList(),
            reviewers.Select(r => new PullRequestReview { IdReviewer = r.IdReviewer }).ToList());

        await outbox.DbContext.AddAsync(pullrequest, ct);

        await outbox.PublishAsync(new PullRequestCreated(title));

        await outbox.SaveChangesAndFlushMessagesAsync();
    }
}

public static class PullRequestCreatedHandler
{
    public static void Handle(PullRequestCreated @event)
    {
        throw new CodeReviewException("bouuuum");
    }
}