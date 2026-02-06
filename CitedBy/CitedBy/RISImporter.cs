namespace CitedBy;

public class RISImporter
{
    private readonly string _filePath;

    public RISImporter(string filePath)
    {
        _filePath = filePath;
    }

    public List<Article> Import()
    {
        var entries = new List<Article>();
        var lines = File.ReadAllLines(_filePath);
        Article? currentEntry = null;

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            if (line.StartsWith("TY  - "))
            {
                currentEntry = new Article { Type = line.Substring(6) };
            }
            else if (line.StartsWith("ER  -") && currentEntry != null)
            {
                entries.Add(currentEntry);
                currentEntry = null;
            }
            else if (currentEntry != null && line.Length > 6 &&
                     line[2] == ' ' && line[3] == ' ')
            {
                var tag = line.Substring(0, 2);
                var value = line.Substring(6);

                switch (tag)
                {
                    case "AU":
                        currentEntry.Authors.Add(value);
                        break;
                    case "TI":
                        currentEntry.Title = value;
                        break;
                    case "PY":
                        currentEntry.Year = value;
                        break;
                    case "JO":
                        currentEntry.Journal = value;
                        break;
                    case "T2":
                    case "T3":
                    case "C3":
                        if (string.IsNullOrEmpty(currentEntry.Journal))
                            currentEntry.Journal = value;
                        break;
                    case "DO":
                        currentEntry.DOI = value;
                        break;
                    case "SP":
                        if (int.TryParse(value, out var startPage))
                            currentEntry.StartPage = startPage;
                        break;
                    case "EP":
                        if (int.TryParse(value, out var endPage))
                            currentEntry.EndPage = endPage;
                        break;
                }
            }
        }


        return entries;
    }
}