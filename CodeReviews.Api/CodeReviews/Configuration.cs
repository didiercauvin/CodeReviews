using CodeReviews.Api.CodeReviews.CreatingPullRequest;
using CodeReviews.Api.CodeReviews.GettingPulllRequests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeReviews.Api.CodeReviews;

public static class PullRequestEndpoints
{
    public static IEndpointRouteBuilder UsePullRequestEndpoints(this IEndpointRouteBuilder endpoints)
        => endpoints
            .UseCreatePullRequestEndpoint()
            .UseGetPullRequestsEndpoint();
}

public class CodeReviewDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Initial Catalog=demo-code-reviews;Integrated Security=true; TrustServerCertificate=True;");
        }

        
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
