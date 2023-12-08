namespace CodeReviews.Api.CodeReviews;

public class PullRequest
{

    public int Id { get; set; }
    public string Status { get; set; } = "Pending";
    public List<PullRequestFile> Files { get; set; }
    public List<PullRequestReview> Reviews { get; set; }
}

public class PullRequestReview
{
    public int Id { get; set; }
    public int IdPullRequest { get; set; }
    private PullRequest PullRequest { get; set; }
    public int IdReviewer { get; set; }
    public string Status { get; set; } = "OK";
}

public class PullRequestFile
{
    public int Id { get; set; }
    public int IdPullRequest { get; set; }
    private PullRequest PullRequest { get; set; }
    public string Filename { get; set; }
    public List<PullRequestFileReview> Reviews { get; set; } = new();
}

public class PullRequestFileReview
{
    public int Id { get; set; }
    public int IdPullRequestFile { get; set; }
    private PullRequestFile PullRequestFile { get; set; }
    public int IdReviewer { get; set; }
    public string Comment { get; set; }
}
