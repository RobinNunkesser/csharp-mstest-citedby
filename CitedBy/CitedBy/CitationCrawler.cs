using HtmlAgilityPack;
using Microsoft.Playwright;

namespace CitedBy;

public enum CitationSource
{
    Unknown,
    IEEE,
    ACM
}

public class CitationCrawler
{
    private readonly ICrawler _crawler;

    public CitationCrawler(Uri uri)
    {
        if (uri.Host.Contains("ieeexplore.ieee.org",
                StringComparison.OrdinalIgnoreCase))
            Source = CitationSource.IEEE;
        else if (uri.Host.Contains("dl.acm.org",
                     StringComparison.OrdinalIgnoreCase))
            Source = CitationSource.ACM;
        else
            Source = CitationSource.Unknown;

        _crawler = Source switch
        {
            CitationSource.IEEE => new IEEECrawler(uri),
            CitationSource.ACM => new ACMCrawler(uri),
            _ => new NullCrawler(uri)
        };
    }

    public CitationSource Source { get; }

    public Task<List<string>> RetrieveCitedBy()
    {
        return _crawler.RetrieveCitedBy();
    }
}

public class ACMCrawler : AbstractCrawler
{
    public ACMCrawler(Uri uri) : base(uri)
    {
        var doi = uri.AbsolutePath.Replace("/doi/", "");
        Uri = new Uri($"https://dl.acm.org/action/ajaxShowCitedBy?doi={doi}");
    }

    public Uri Uri { get; set; }

    protected override List<string> GetCitationsFromHtml(string html)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(html);
        var citationNodes =
            doc.DocumentNode.SelectNodes("//div[@class='citation__title']");

        var citations = citationNodes
            ?.Select(node => node.InnerText.Trim())
            .ToList() ?? new List<string>();

        return citations;
    }
}

public interface ICrawler
{
    public Uri Uri { get; }
    Task<List<string>> RetrieveCitedBy();
}

public abstract class AbstractCrawler : ICrawler
{
    public AbstractCrawler(Uri uri)
    {
        Uri = uri;
    }

    public virtual async Task<List<string>> RetrieveCitedBy()
    {
        try
        {
            var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(
                new BrowserTypeLaunchOptions
                {
                    Headless =
                        true, // Oder true mit zusätzlichen Stealth-Optionen
                    Args = new[]
                    {
                        "--no-sandbox", "--disable-setuid-sandbox",
                        "--disable-blink-features=AutomationControlled"
                    }
                });
            var context = await browser.NewContextAsync(
                new BrowserNewContextOptions
                {
                    UserAgent =
                        "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36"
                });
            var page = await context.NewPageAsync();
            await page.GotoAsync(Uri.ToString(),
                new PageGotoOptions { WaitUntil = WaitUntilState.NetworkIdle });
            await Task.Delay(2000); // Kurze Verzögerung
            var html = await page.ContentAsync();
            var citations = GetCitationsFromHtml(html);
            return citations;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Playwright-Fehler: {ex.Message}");
            throw;
        }

        // Default implementation does nothing
        // <div _ngcontent-ng-c3980643759="" id="anchor-paper-citations-ieee" data-analytics_identifier="citations_publisher_ieee" class="pushBtm20 stats-citations-publisher-ieee"><h2 _ngcontent-ng-c3980643759="" class="hide-desktop">Cites in Papers - IEEE (5)</h2><div _ngcontent-ng-c3980643759="" class="hide-mobile"><input _ngcontent-ng-c3980643759="" type="checkbox" aria-label="Citation Select All By Papers" class="u-mr-1 ng-untouched ng-pristine ng-valid"><span _ngcontent-ng-c3980643759="">Select All</span></div><div _ngcontent-ng-c3980643759="" class="reference-container"><div _ngcontent-ng-c3980643759="" class="d-flex pushTop10"><div _ngcontent-ng-c3980643759="" class="hide-mobile u-pr-1"><input _ngcontent-ng-c3980643759="" type="checkbox" aria-label="Select citation records" class="xpl-checkbox-default search-results-group ng-untouched ng-pristine ng-valid"><!----></div><div _ngcontent-ng-c3980643759=""><b _ngcontent-ng-c3980643759="">1</b>.</div><div _ngcontent-ng-c3980643759="" class="col u-px-1"><span _ngcontent-ng-c3980643759="" class="description">Elliott Wen, Jiaxiang Zhou, Xiapu Luo, Giovanni Russello, Jens Dietrich, "Keep Me Updated: An Empirical Study on Embedded JavaScript Engines in Android Apps", <i>2024 IEEE/ACM 21st International Conference on Mining Software Repositories (MSR)</i>, pp.361-372, 2024.</span><div _ngcontent-ng-c3980643759="" data-analytics_identifier="citations_links_container" class="ref-links-container stats-citations-links-container"><!----><!----><!----><span _ngcontent-ng-c3980643759="" class="ref-link"><a _ngcontent-ng-c3980643759="" target="_self" class="stats-citations-link-viewArticle" href="/document/10555696"> Show Article </a></span><!----><span _ngcontent-ng-c3980643759="" class="ref-link view-pdf"><xpl-view-pdf _ngcontent-ng-c3980643759="" _nghost-ng-c3856927191=""><div _ngcontent-ng-c3856927191=""><!----><div _ngcontent-ng-c3856927191="" class="u-flex-display-flex u-flex-align-items-center"><a _ngcontent-ng-c3856927191="" aria-label="PDF" tooltipclass="document-toolbar-tooltip" class="stats_PDF_10555696 u-flex-display-flex" href="/stamp/stamp.jsp?tp=&amp;arnumber=10555696"><i _ngcontent-ng-c3856927191="" class="icon-size-md xpl-pdf-icon fas fa-file-pdf"></i></a><!----></div><!----><!----></div><!----><!----></xpl-view-pdf></span><!----><span _ngcontent-ng-c3980643759="" class="ref-link"><a _ngcontent-ng-c3980643759="" target="_blank" class="stats-citations-link-googleScholar" href="https://scholar.google.com/scholar?as_q=%22Keep+Me+Updated%3A+An+Empirical+Study+on+Embedded+JavaScript+Engines+in+Android+Apps%22&amp;as_occt=title&amp;hl=en&amp;as_sdt=0%2C31"> Google Scholar <i _ngcontent-ng-c3980643759="" class="fas fa-external-link-alt fa-sm"></i></a></span><!----></div></div><!----></div></div><div _ngcontent-ng-c3980643759="" class="reference-container"><div _ngcontent-ng-c3980643759="" class="d-flex pushTop10"><div _ngcontent-ng-c3980643759="" class="hide-mobile u-pr-1"><input _ngcontent-ng-c3980643759="" type="checkbox" aria-label="Select citation records" class="xpl-checkbox-default search-results-group ng-untouched ng-pristine ng-valid"><!----></div><div _ngcontent-ng-c3980643759=""><b _ngcontent-ng-c3980643759="">2</b>.</div><div _ngcontent-ng-c3980643759="" class="col u-px-1"><span _ngcontent-ng-c3980643759="" class="description">Prithvi Srinivas G, Sai Nitya M, Ranjith J, "Passenger Demand Application for Public Transportation System in Smart Cities", <i>2023 IEEE Technology &amp; Engineering Management Conference - Asia Pacific (TEMSCON-ASPAC)</i>, pp.1-7, 2023.</span><div _ngcontent-ng-c3980643759="" data-analytics_identifier="citations_links_container" class="ref-links-container stats-citations-links-container"><!----><!----><!----><span _ngcontent-ng-c3980643759="" class="ref-link"><a _ngcontent-ng-c3980643759="" target="_self" class="stats-citations-link-viewArticle" href="/document/10531473"> Show Article </a></span><!----><span _ngcontent-ng-c3980643759="" class="ref-link view-pdf"><xpl-view-pdf _ngcontent-ng-c3980643759="" _nghost-ng-c3856927191=""><div _ngcontent-ng-c3856927191=""><!----><div _ngcontent-ng-c3856927191="" class="u-flex-display-flex u-flex-align-items-center"><a _ngcontent-ng-c3856927191="" aria-label="PDF" tooltipclass="document-toolbar-tooltip" class="stats_PDF_10531473 u-flex-display-flex" href="/stamp/stamp.jsp?tp=&amp;arnumber=10531473"><i _ngcontent-ng-c3856927191="" class="icon-size-md xpl-pdf-icon fas fa-file-pdf"></i></a><!----></div><!----><!----></div><!----><!----></xpl-view-pdf></span><!----><span _ngcontent-ng-c3980643759="" class="ref-link"><a _ngcontent-ng-c3980643759="" target="_blank" class="stats-citations-link-googleScholar" href="https://scholar.google.com/scholar?as_q=%22Passenger+Demand+Application+for+Public+Transportation+System+in+Smart+Cities%22&amp;as_occt=title&amp;hl=en&amp;as_sdt=0%2C31"> Google Scholar <i _ngcontent-ng-c3980643759="" class="fas fa-external-link-alt fa-sm"></i></a></span><!----></div></div><!----></div></div><div _ngcontent-ng-c3980643759="" class="reference-container"><div _ngcontent-ng-c3980643759="" class="d-flex pushTop10"><div _ngcontent-ng-c3980643759="" class="hide-mobile u-pr-1"><input _ngcontent-ng-c3980643759="" type="checkbox" aria-label="Select citation records" class="xpl-checkbox-default search-results-group ng-untouched ng-pristine ng-valid"><!----></div><div _ngcontent-ng-c3980643759=""><b _ngcontent-ng-c3980643759="">3</b>.</div><div _ngcontent-ng-c3980643759="" class="col u-px-1"><span _ngcontent-ng-c3980643759="" class="description">Parsa Karami, Ikram Darif, Cristiano Politowski, Ghizlane El Boussaidi, Sègla Kpodjedo, Imen Benzarti, "On the Impact of Development Frameworks on Mobile Apps", <i>2023 30th Asia-Pacific Software Engineering Conference (APSEC)</i>, pp.131-140, 2023.</span><div _ngcontent-ng-c3980643759="" data-analytics_identifier="citations_links_container" class="ref-links-container stats-citations-links-container"><!----><!----><!----><span _ngcontent-ng-c3980643759="" class="ref-link"><a _ngcontent-ng-c3980643759="" target="_self" class="stats-citations-link-viewArticle" href="/document/10479362"> Show Article </a></span><!----><span _ngcontent-ng-c3980643759="" class="ref-link view-pdf"><xpl-view-pdf _ngcontent-ng-c3980643759="" _nghost-ng-c3856927191=""><div _ngcontent-ng-c3856927191=""><!----><div _ngcontent-ng-c3856927191="" class="u-flex-display-flex u-flex-align-items-center"><a _ngcontent-ng-c3856927191="" aria-label="PDF" tooltipclass="document-toolbar-tooltip" class="stats_PDF_10479362 u-flex-display-flex" href="/stamp/stamp.jsp?tp=&amp;arnumber=10479362"><i _ngcontent-ng-c3856927191="" class="icon-size-md xpl-pdf-icon fas fa-file-pdf"></i></a><!----></div><!----><!----></div><!----><!----></xpl-view-pdf></span><!----><span _ngcontent-ng-c3980643759="" class="ref-link"><a _ngcontent-ng-c3980643759="" target="_blank" class="stats-citations-link-googleScholar" href="https://scholar.google.com/scholar?as_q=%22On+the+Impact+of+Development+Frameworks+on+Mobile+Apps%22&amp;as_occt=title&amp;hl=en&amp;as_sdt=0%2C31"> Google Scholar <i _ngcontent-ng-c3980643759="" class="fas fa-external-link-alt fa-sm"></i></a></span><!----></div></div><!----></div></div><div _ngcontent-ng-c3980643759="" class="reference-container"><div _ngcontent-ng-c3980643759="" class="d-flex pushTop10"><div _ngcontent-ng-c3980643759="" class="hide-mobile u-pr-1"><input _ngcontent-ng-c3980643759="" type="checkbox" aria-label="Select citation records" class="xpl-checkbox-default search-results-group ng-untouched ng-pristine ng-valid"><!----></div><div _ngcontent-ng-c3980643759=""><b _ngcontent-ng-c3980643759="">4</b>.</div><div _ngcontent-ng-c3980643759="" class="col u-px-1"><span _ngcontent-ng-c3980643759="" class="description">Benedikt Dornauer, Michael Felderer, "Energy-Saving Strategies for Mobile Web Apps and their Measurement: Results from a Decade of Research", <i>2023 IEEE/ACM 10th International Conference on Mobile Software Engineering and Systems (MOBILESoft)</i>, pp.75-86, 2023.</span><div _ngcontent-ng-c3980643759="" data-analytics_identifier="citations_links_container" class="ref-links-container stats-citations-links-container"><!----><!----><!----><span _ngcontent-ng-c3980643759="" class="ref-link"><a _ngcontent-ng-c3980643759="" target="_self" class="stats-citations-link-viewArticle" href="/document/10172954"> Show Article </a></span><!----><span _ngcontent-ng-c3980643759="" class="ref-link view-pdf"><xpl-view-pdf _ngcontent-ng-c3980643759="" _nghost-ng-c3856927191=""><div _ngcontent-ng-c3856927191=""><!----><div _ngcontent-ng-c3856927191="" class="u-flex-display-flex u-flex-align-items-center"><a _ngcontent-ng-c3856927191="" aria-label="PDF" tooltipclass="document-toolbar-tooltip" class="stats_PDF_10172954 u-flex-display-flex" href="/stamp/stamp.jsp?tp=&amp;arnumber=10172954"><i _ngcontent-ng-c3856927191="" class="icon-size-md xpl-pdf-icon fas fa-file-pdf"></i></a><!----></div><!----><!----></div><!----><!----></xpl-view-pdf></span><!----><span _ngcontent-ng-c3980643759="" class="ref-link"><a _ngcontent-ng-c3980643759="" target="_blank" class="stats-citations-link-googleScholar" href="https://scholar.google.com/scholar?as_q=%22Energy-Saving+Strategies+for+Mobile+Web+Apps+and+their+Measurement%3A+Results+from+a+Decade+of+Research%22&amp;as_occt=title&amp;hl=en&amp;as_sdt=0%2C31"> Google Scholar <i _ngcontent-ng-c3980643759="" class="fas fa-external-link-alt fa-sm"></i></a></span><!----></div></div><!----></div></div><div _ngcontent-ng-c3980643759="" class="reference-container"><div _ngcontent-ng-c3980643759="" class="d-flex pushTop10"><div _ngcontent-ng-c3980643759="" class="hide-mobile u-pr-1"><input _ngcontent-ng-c3980643759="" type="checkbox" aria-label="Select citation records" class="xpl-checkbox-default search-results-group ng-untouched ng-pristine ng-valid"><!----></div><div _ngcontent-ng-c3980643759=""><b _ngcontent-ng-c3980643759="">5</b>.</div><div _ngcontent-ng-c3980643759="" class="col u-px-1"><span _ngcontent-ng-c3980643759="" class="description">Mohammed A. AboArab, Vassiliki T. Potsika, Nikola Petrović, Dimitrios I. Fotiadis, "DECODE cloud platform: A new cloud platform to combat the burden of peripheral artery disease", <i>2022 Panhellenic Conference on Electronics &amp; Telecommunications (PACET)</i>, pp.1-6, 2022.</span><div _ngcontent-ng-c3980643759="" data-analytics_identifier="citations_links_container" class="ref-links-container stats-citations-links-container"><!----><!----><!----><span _ngcontent-ng-c3980643759="" class="ref-link"><a _ngcontent-ng-c3980643759="" target="_self" class="stats-citations-link-viewArticle" href="/document/9976356"> Show Article </a></span><!----><span _ngcontent-ng-c3980643759="" class="ref-link view-pdf"><xpl-view-pdf _ngcontent-ng-c3980643759="" _nghost-ng-c3856927191=""><div _ngcontent-ng-c3856927191=""><!----><div _ngcontent-ng-c3856927191="" class="u-flex-display-flex u-flex-align-items-center"><a _ngcontent-ng-c3856927191="" aria-label="PDF" tooltipclass="document-toolbar-tooltip" class="stats_PDF_9976356 u-flex-display-flex" href="/stamp/stamp.jsp?tp=&amp;arnumber=9976356"><i _ngcontent-ng-c3856927191="" class="icon-size-md xpl-pdf-icon fas fa-file-pdf"></i></a><!----></div><!----><!----></div><!----><!----></xpl-view-pdf></span><!----><span _ngcontent-ng-c3980643759="" class="ref-link"><a _ngcontent-ng-c3980643759="" target="_blank" class="stats-citations-link-googleScholar" href="https://scholar.google.com/scholar?as_q=%22DECODE+cloud+platform%3A+A+new+cloud+platform+to+combat+the+burden+of+peripheral+artery+disease%22&amp;as_occt=title&amp;hl=en&amp;as_sdt=0%2C31"> Google Scholar <i _ngcontent-ng-c3980643759="" class="fas fa-external-link-alt fa-sm"></i></a></span><!----></div></div><!----></div></div><!----><!----></div>
    }


    public Uri Uri { get; }

    protected abstract List<string> GetCitationsFromHtml(string html);
}

public class IEEECrawler : AbstractCrawler
{
    public IEEECrawler(Uri uri) : base(uri)
    {
    }

    protected override List<string> GetCitationsFromHtml(string html)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(html);
        var citationsDiv =
            doc.DocumentNode.SelectSingleNode(
                "//div[@id='anchor-paper-citations-ieee']");

        var descriptions = citationsDiv
            ?.SelectNodes(".//span[@class='description']//i")
            ?.Select(node => node.InnerText.Trim())
            .ToList() ?? new List<string>();

        return descriptions;
    }
}

public class NullCrawler : AbstractCrawler
{
    public NullCrawler(Uri uri) : base(uri)
    {
        Console.Error.WriteLine($"Unknown citation source for URI: {Uri}");
    }

    protected override List<string> GetCitationsFromHtml(string html)
    {
        return [string.Empty];
    }
}