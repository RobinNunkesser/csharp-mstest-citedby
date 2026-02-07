using System;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CitedBy.Tests;

[TestClass]
[TestSubject(typeof(CitedByService))]
public class CitedByServiceTest
{
    [TestMethod]
    public async Task TestGetCitedBy()
    {
        // Arrange
        var service = new CitedByService();

        // Act
        var result = await
            service.ComputeCitationsAndMobileQuota([
                "10.1109/MOBILESoft.2017.1"
            ]);

        Console.WriteLine(string.Join(Environment.NewLine,
            result.Select(kvp => $"{kvp.Key}: {kvp.Value}")));

        // Assert
        Assert.IsNotNull(result);
    }
}