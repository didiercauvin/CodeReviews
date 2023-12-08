using CodeReviews.Api.CodeReviews;

namespace CodeReviews.Api.Core;

public static class TutelleDbContextExtensions
{
    public static async Task AddAndSaveAsync<T>(this CodeReviewDbContext context, T entity, CancellationToken ct) where T : class
    {
        await context.AddAsync(entity, ct);
        await context.SaveChangesAsync(ct);
    }

    public static async Task GetAndUpdateAsync<T>(this CodeReviewDbContext context, int id, Action<T> handle) where T : class
    {
        var entity = await context.Set<T>().FindAsync(id);

        if (entity == null)
            throw new InvalidOperationException($"{nameof(T)} avec l'id '{id}' introuvable!");

        handle(entity);

        await context.SaveChangesAsync();
    }
}
