using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Rancho.Services.Management.IntegrationTests;
using Rancho.Services.Management.Shared.Data;
using Tests.Shared.Fixtures;
using Xunit.Abstractions;

Console.WriteLine("Bullshit");
BenchmarkRunner.Run<Benchmark>();

public class Benchmark
{
    private int[] _array;

    [GlobalSetup]
    public void Setup()
    {
        _array = Enumerable.Range(0, int.MaxValue).ToArray();
    }

    [Benchmark]
    public void Benchmark1()
    {
        foreach (var i in _array)
        {
            var s = i * i;
        }
    }

    [Benchmark]
    public void Benchmark2()
    {
        for (int i = 0; i < _array.Length; i++)
        {
            var s = _array[i] * _array[i];
        }
    }


}
