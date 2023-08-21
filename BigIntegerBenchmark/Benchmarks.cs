// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using BenchmarkDotNet;
using BenchmarkDotNet.Attributes;

namespace BigIntegerBenchmark;

[MemoryDiagnoser]
public class Benchmarks
{
    [Params(0, 1, 2, 3, 4, 5, 6, 7, 8, 16, 24)]
    public int NumberOfZeroElements { get; set; }
    private const int ArrayLength = 256;
    private Random _random = null!;
    private uint GetRandomNonZeroUint() => (uint)_random.NextInt64(1, uint.MaxValue);
    private uint[] _input = null!;

    private uint[] GetFilledArray()
    {
        var result = new uint[ArrayLength];
        for (var i = 0; i < ArrayLength; i++)
        {
            result[i] = GetRandomNonZeroUint();
        }
        return result;
    }

    private uint[] GetFilledArrayWithNumberOfZeroElements()
    {
        var result = GetFilledArray();
        var lastIdx = result.Length - 1;
        for (var i = 0; i < NumberOfZeroElements; i++)
        {
            var idx = lastIdx - i;
            result[idx] = 0;
        }
        return result;
    }

    [GlobalSetup]
    public void GlobalSetup()
    {
        _random = new Random(8964);
        _input = GetFilledArrayWithNumberOfZeroElements();
    }

    [Benchmark(Baseline = true)]
    public int Current()
    {
        return Implementations.Current(_input);
    }

    // [Benchmark]
    // public int SpanIndexOf()
    // {
    //     return Implementations.UsingLastIndexOfAnyExcept(_input);
    // }

    // [Benchmark]
    // public int SpanIndexOfOpt()
    // {
    //     return Implementations.UsingLastIndexOfAnyExcept_WithOptimizedNoZeroes(_input);
    // }

    [Benchmark]
    public int SpanIndexOfOpt1()
    {
        return Implementations.UsingLastIndexOfAnyExcept_WithOptimizedUpTo1Zeroes(_input);
    }

    // [Benchmark]
    // public int SpanIndexOfOpt3()
    // {
    //     return Implementations.UsingLastIndexOfAnyExcept_WithOptimizedUpTo3Zeroes(_input);
    // }
}
