using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wolverine;

namespace CodeReviews.Api.CodeReviews;

public class CodeReviewException : Exception
{
    public CodeReviewException() : base()
    {
    }

    public CodeReviewException(string? message) : base(message)
    {
    }

    public CodeReviewException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}

public class CodeReviewForbiddenWordException : Exception
{
    public CodeReviewForbiddenWordException() : base()
    {
    }

    public CodeReviewForbiddenWordException(string? message) : base(message)
    {
    }

    public CodeReviewForbiddenWordException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}

public static class CodeReviewDoNothingMiddleware
{
    public static Task<HandlerContinuation> LoadAsync(CreatePullRequestRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(HandlerContinuation.Continue);
    }
}

public static class PullRequestLookupMiddleware
{
    public static async Task<(HandlerContinuation, PullRequest?)> LoadAsync(CreatePullRequestRequest request, CodeReviewDbContext context,
        CancellationToken cancellationToken)
    {
        var pullrequest = await context.PullRequests.SingleOrDefaultAsync(pr => pr.Title == request.Title, cancellationToken);

        return (pullrequest is null ? HandlerContinuation.Continue : HandlerContinuation.Stop, pullrequest);
    }
}

public class CodeReviewDbContext : DbContext
{
    public CodeReviewDbContext(DbContextOptions<CodeReviewDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PullRequestMap).Assembly);
    }

    public DbSet<PullRequest> PullRequests { get; set; }
}

public class PullRequestMap : IEntityTypeConfiguration<PullRequest>
{
    public void Configure(EntityTypeBuilder<PullRequest> builder)
    {
        builder.HasKey("Id");
        builder.ToTable("PullRequests");


        builder.Navigation(b => b.Files).AutoInclude();
        builder.Navigation(b => b.Reviews).AutoInclude();

        builder
            .HasMany(x => x.Files)
            .WithOne("PullRequest")
            .HasForeignKey("IdPullRequest");

        builder
            .HasMany(x => x.Reviews)
            .WithOne("PullRequest")
            .HasForeignKey("IdPullRequest");
    }
}

public class PullRequestReviewMap : IEntityTypeConfiguration<PullRequestReview>
{
    public void Configure(EntityTypeBuilder<PullRequestReview> builder)
    {
        builder.HasKey("Id");
        builder.ToTable("PullRequestReviews");

    }
}

public class PullRequestFileMap : IEntityTypeConfiguration<PullRequestFile>
{
    public void Configure(EntityTypeBuilder<PullRequestFile> builder)
    {
        builder.HasKey("Id");
        builder.ToTable("PullRequestFiles");


        builder.Navigation(b => b.Reviews).AutoInclude();

        builder
            .HasMany(x => x.Reviews)
            .WithOne("PullRequestFile")
            .HasForeignKey("IdPullRequestFile");
    }
}

public class PullRequestFileReviewMap : IEntityTypeConfiguration<PullRequestFileReview>
{
    public void Configure(EntityTypeBuilder<PullRequestFileReview> builder)
    {
        builder.HasKey("Id");
        builder.ToTable("PullRequestFileReviews");
    }
}
