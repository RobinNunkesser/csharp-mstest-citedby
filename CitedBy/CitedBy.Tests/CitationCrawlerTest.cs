using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CitedBy.Tests;

[TestClass]
[TestSubject(typeof(CitationCrawler))]
public class CitationCrawlerTest
{
    [TestMethod]
    public async Task
        GetCitations_ShouldReturnCitations_WhenValidIEEEUrlProvided()
    {
        // Arrange
        var crawler = new CitationCrawler(new Uri(
            "https://ieeexplore.ieee.org/document/8543456/citations#citations"));

        // Act
        var citations = await crawler.RetrieveCitedBy();

        // Assert
        Assert.IsNotNull(citations);
    }

    [TestMethod]
    public async Task
        GetCitations_ShouldReturnCitations_WhenValidACMUrlProvided()
    {
        // Arrange
        var crawler = new CitationCrawler(new Uri(
            "https://dl.acm.org/doi/10.1145/3197231.3197260"));

        // Act
        var citations = await crawler.RetrieveCitedBy();

        // Assert
        Assert.IsNotNull(citations);
    }
}