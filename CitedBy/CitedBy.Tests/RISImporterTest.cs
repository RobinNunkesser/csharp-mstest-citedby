using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CitedBy.Tests;

[TestClass]
[TestSubject(typeof(RISImporter))]
public class RISImporterTest
{
    [TestMethod]
    public void TestImportRISFile()
    {
        // Arrange
        var importer =
            new RISImporter("/Users/nunkesser/Downloads/savedrecs-4.ris");

        // Act
        var result = importer.Import();

        // Assert
        Assert.IsNotNull(result);
    }
}