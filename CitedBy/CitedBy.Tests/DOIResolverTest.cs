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
        var doi = "10.1109/MOBILESoft.2017.52";
        // Act
        var result = await DOIResolver.ResolveUriAsync(doi);

        // Assert
        Assert.IsNotNull(result);
    }
}