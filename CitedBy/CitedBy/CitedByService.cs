namespace CitedBy;

public class CitedByService
{
    public async Task<Dictionary<string, (int Citations, double MobileQuota)>>
        ComputeCitationsAndMobileQuota(
            IEnumerable<string> dois)
    {
        var results =
            new Dictionary<string, (int Citations, double MobileQuota)>();

        foreach (var doi in dois)
        {
            // Resolve the DOI
            var result = await DOIResolver.ResolveUriAsync(doi);

            // Retrieve citations using the CitationCrawler
            var crawler = new CitationCrawler(result!);
            var citations = await crawler.RetrieveCitedBy();

            // Compute the Mobile Quote using the SourceClassifier
            var classifier = new SourceClassifier();
            var ratio = classifier.RatioMSE(citations);

            results[doi] = (citations.Count, ratio);
        }

        return results;
    }
}