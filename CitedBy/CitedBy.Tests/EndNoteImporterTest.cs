using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CitedBy.Tests;

[TestClass]
[TestSubject(typeof(EndNoteImporter))]
public class EndNoteImporterTest
{
    [TestMethod]
    public void TestImportEndNoteFile()
    {
        // Arrange
        var importer =
            new EndNoteImporter("/Users/nunkesser/Downloads/sda.enw");

        // Act
        var result = importer.Import();

        // Assert
        Assert.IsNotNull(result);
    }
}