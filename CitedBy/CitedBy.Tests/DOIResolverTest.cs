using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CitedBy.Tests;

[TestClass]
[TestSubject(typeof(DOIResolver))]
public class DOIResolverTest
{
    [TestMethod]
    public async Task TestResolveDOI_ValidDOI_ReturnsMetadata()
    {
        // Arrange
        var doi = "10.1145/3197231.3197260";
        // Act
        var result = await DOIResolver.ResolveUriAsync(doi);

        // Assert
        Assert.IsNotNull(result);
    }
}

// https://doi.org/10.1145/3197231.3197260
// https://dl.acm.org/action/ajaxShowCitedBy?doi=10.1145/3197231.3197260