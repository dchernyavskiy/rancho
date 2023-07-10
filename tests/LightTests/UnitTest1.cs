using FluentAssertions;
using Fare;
using Xunit.Abstractions;


namespace LightTests;

public class UnitTest1
{
    private readonly ITestOutputHelper _outputHelper;

    public UnitTest1(ITestOutputHelper outputHelperHelper)
    {
        _outputHelper = outputHelperHelper;
    }

    [Fact]
    public void Test1()
    {
        nameof(Animal.Name).Should().Be("Name");
    }

    [Fact]
    public void Test2()
    {
        var d1 = new DateTime(2020, 1, 1);
        var d2 = DateTime.Today;

        d1.Should().BeOnOrBefore(d2);
        d1.CompareTo(d2).Should().BeLessThan(0);
    }

    [Fact]
    public void Test3()
    {
        var regex = @"[A-Z]{2}\d{4}";

        var xeger = new Xeger(regex, new Random());
        _outputHelper.WriteLine(xeger.Generate());
        _outputHelper.WriteLine(xeger.Generate());
        _outputHelper.WriteLine(xeger.Generate());
        _outputHelper.WriteLine(xeger.Generate());

        xeger.Generate().Should().MatchRegex(regex);
    }

    [Fact]
    public void Test4()
    {
        _outputHelper.WriteLine(Guid.NewGuid().ToString());
        _outputHelper.WriteLine(Guid.NewGuid().ToString());
        _outputHelper.WriteLine(Guid.NewGuid().ToString());
        _outputHelper.WriteLine(Guid.NewGuid().ToString());
        _outputHelper.WriteLine(Guid.NewGuid().ToString());
    }
}

public class Animal
{
    public string Name { get; set; }
}
