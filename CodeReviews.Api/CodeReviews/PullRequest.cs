namespace CodeReviews.Api.CodeReviews;

public class PullRequest
{
    private PullRequest()
    {
        
    }

    private PullRequest(string title, string status, IEnumerable<PullRequestFile> files, IEnumerable<PullRequestReview> reviews)
    {
        Title = title;
        Status = status;
        Files = files.ToArray();
        Reviews = reviews.ToArray();
    }

    public static PullRequest Create(string title, IEnumerable<PullRequestFile> files, IEnumerable<PullRequestReview> reviews)
    {
        if (title.Contains("PR"))
        {
            throw new CodeReviewForbiddenWordException("L'abreviation PR est interdite!");
        }

        return new(title, "Pending", files, reviews);
    }

    public int Id { get; set; }
    public string? Title { get; set; }
    public string Status { get; set; } = "Pending";
    public ICollection<PullRequestFile> Files { get; set; } = new List<PullRequestFile>();
    public ICollection<PullRequestReview> Reviews { get; set; } = new List<PullRequestReview>();
}

public class PullRequestReview
{
    public int Id { get; set; }
    public int IdPullRequest { get; set; }
    private PullRequest PullRequest { get; set; }
    public int IdReviewer { get; set; }
    public string Status { get; set; } = "Pending";
}

public class PullRequestFile
{
    public int Id { get; set; }
    public int IdPullRequest { get; set; }
    private PullRequest PullRequest { get; set; }
    public string Filename { get; set; }
    public ICollection<PullRequestFileReview> Reviews { get; set; } = new List<PullRequestFileReview>();
}

public class PullRequestFileReview
{
    public int Id { get; set; }
    public int IdPullRequestFile { get; set; }
    private PullRequestFile PullRequestFile { get; set; }
    public int IdReviewer { get; set; }
    public string Comment { get; set; }
}
