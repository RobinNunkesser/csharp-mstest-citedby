using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CitedBy.Tests;

[TestClass]
[TestSubject(typeof(SourceClassifier))]
public class SourceClassifierTest
{
    private readonly List<string> _testData = new()
    {
        "2024 IEEE/ACM 21st International Conference on Mining Software Repositories (MSR)",
        "2023 IEEE Technology & Engineering Management Conference - Asia Pacific (TEMSCON-ASPAC)",
        "2023 30th Asia-Pacific Software Engineering Conference (APSEC)",
        "2023 IEEE/ACM 10th International Conference on Mobile Software Engineering and Systems (MOBILESoft)",
        "2022 Panhellenic Conference on Electronics & Telecommunications (PACET)"
    };

    [TestMethod]
    public void TestIsMSE_ShouldIdentifyMSEConference()
    {
        // Arrange
        var classifier = new SourceClassifier();

        // Act
        var result = classifier.IsMSE(_testData[3]);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void TestRatioMSE_ShouldCalculateCorrectRatio()
    {
        // Arrange
        var classifier = new SourceClassifier();

        // Act
        var ratio = classifier.RatioMSE(_testData);

        // Assert
        Assert.AreEqual(0.2, ratio);
    }

    [TestMethod]
    public void TestRealDataRIS()
    {
        var importer =
            new RISImporter("/Users/nunkesser/Downloads/energeff.ris");


        var result = importer.Import();
        var classifier = new SourceClassifier();

        var entriesWithoutJournal = result
            .Where(entry => string.IsNullOrEmpty(entry.Journal)).ToList();
        var mseEntries = result.Where(entry => classifier.IsMSE(entry.Journal))
            .ToList();
        Assert.IsNotNull(mseEntries);
    }

    [TestMethod]
    public void TestRealDataEndNote()
    {
        /*var importer =
            new EndNoteImporter("/Users/nunkesser/Downloads/sda.enw");*/

        var importer =
            new EndNoteImporter("/Users/nunkesser/Downloads/main.enw");

        var result = importer.Import();
        var classifier = new SourceClassifier();

        var entriesWithoutJournal = result
            .Where(entry => string.IsNullOrEmpty(entry.Journal)).ToList();
        var mseEntries = result.Where(entry => classifier.IsMSE(entry.Journal))
            .ToList();
        Assert.IsNotNull(mseEntries);
    }
}