namespace CitedBy;

public class SourceClassifier
{
    public bool IsMSE(string JournalName)
    {
        return JournalName.Contains("MOBILESoft",
            StringComparison.OrdinalIgnoreCase);
    }

    public double RatioMSE(List<string> JournalNames)
    {
        if (JournalNames.Count == 0) return 0.0;
        var mseCount = JournalNames.Count(j => IsMSE(j));
        return (double)mseCount / JournalNames.Count;
    }
}