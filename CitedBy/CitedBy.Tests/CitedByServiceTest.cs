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
        var dois = new[]
        {
            //"10.1109/MOBILESoft.2017.1",
            //"10.1145/2095697.2095754"
            //"10.1145/3001854.3001857",
            //"10.1145/1710035.1710084"
            //"10.1109/MOBILESoft.2019.00031"
            //"10.1109/MOBILESoft.2017.11"
            //"10.1145/3387905.3388612"
            //"10.1145/3197231.3197251"
            //"10.1145/2516760.2516774"
            //"10.1145/1543137.1543161"
            //"10.1109/MOBILESoft.2019.00021"
            //"10.1109/MobileSoft.2015.8"
            //"10.1145/2804345.2804348"
            //"10.1109/MobileSoft52590.2021.00010"
            //"10.1145/1999995.2000018"
            //"10.1145/2593902.2593910"
            //"10.1145/2516760.2516768"
            //"10.1145/3243218.3243219"
            //"10.1145/2536853.2536881"
            //"10.1145/2661694.2661695"
            //"10.1145/2381934.2381950"
            //"10.1145/3007120.3011072"
            "10.1145/2307636.2307661"
        };
        // Arrange
        var service = new CitedByService();

        // Act
        var result = await
            service.ComputeCitationsAndMobileQuota(dois);

        Console.WriteLine(string.Join(Environment.NewLine,
            result.Select(kvp => $"{kvp.Key}: {kvp.Value}")));

        // Assert
        Assert.IsNotNull(result);
    }
}