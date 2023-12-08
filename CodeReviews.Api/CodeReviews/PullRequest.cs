namespace CodeReviews.Api.CodeReviews;

public class PullRequest
{

    public int Id { get; set; }
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
