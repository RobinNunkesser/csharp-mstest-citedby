namespace CitedBy;

public class EndNoteImporter
{
    private readonly string _filePath;

    public EndNoteImporter(string filePath)
    {
        _filePath = filePath;
    }

    public List<Article> Import()
    {
        var publications = new List<Article>();
        var lines = File.ReadAllLines(_filePath);

        Article? currentPublication = null;

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            if (line.StartsWith("%0"))
            {
                if (currentPublication != null)
                    publications.Add(currentPublication);
                currentPublication = new Article
                    { Type = line.Substring(3).Trim() };
            }
            else if (currentPublication != null)
            {
                if (line.StartsWith("%A"))
                {
                    currentPublication.Authors.Add(line.Substring(3).Trim());
                }
                else if (line.StartsWith("%T"))
                {
                    currentPublication.Title = line.Substring(3).Trim();
                }
                else if (line.StartsWith("%D"))
                {
                    currentPublication.Year = line.Substring(3).Trim();
                }
                else if (line.StartsWith("%J") || line.StartsWith("%B"))
                {
                    currentPublication.Journal = line.Substring(3).Trim();
                }
                else if (line.StartsWith("%R"))
                {
                    currentPublication.DOI = line.Substring(3).Trim();
                }
                else if (line.StartsWith("%P"))
                {
                    var pages = line.Substring(3).Trim().Split('-');
                    if (pages.Length == 2 &&
                        int.TryParse(pages[0], out var startPage) &&
                        int.TryParse(pages[1], out var endPage))
                    {
                        currentPublication.StartPage = startPage;
                        currentPublication.EndPage = endPage;
                    }
                }
            }
        }

        if (currentPublication != null)
            publications.Add(currentPublication);

        return publications;
    }
}