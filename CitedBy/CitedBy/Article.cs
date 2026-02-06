namespace CitedBy;

public class Article
{
    public string Type { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public List<string> Authors { get; set; } = new();
    public string Year { get; set; } = string.Empty;
    public string Journal { get; set; } = string.Empty;
    public string DOI { get; set; } = string.Empty;
    public int StartPage { get; set; }
    public int EndPage { get; set; }
    public int PageCount => EndPage > StartPage ? EndPage - StartPage + 1 : 0;
}