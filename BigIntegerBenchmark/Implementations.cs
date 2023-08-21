// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace BigIntegerBenchmark;

public class Implementations
{
    /// <summary>
    /// Existing code in src/System.Runtime.Numerics/src/System/Numerics/BigInteger.cs line aprox 425
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    public static int Current(uint[] val)
    {
        int len = val.Length - 1;
        while (len >= 0 && val[len] == 0) len--;
        len++;
        return len;
    }

    /// <summary>
    /// find len replaced by Span method
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    public static int UsingLastIndexOfAnyExcept(uint[] val)
    {
        int len = val.AsSpan().LastIndexOfAnyExcept((uint)0);
        len++;
        return len;
    }

    /// <summary>
    /// find len replaced by Span method with optimized "no zeroes" case
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    public static int UsingLastIndexOfAnyExcept_WithOptimizedNoZeroes(uint[] val)
    {
        int len = val.Length;
        if (val[len - 1] == 0)
        {
            len = val.AsSpan().LastIndexOfAnyExcept((uint)0);
            len++;
        }
        return len;
    }

    /// <summary>
    /// find len replaced by Span method with optimized "no zeroes" and up to 1 zeroes cases
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    public static int UsingLastIndexOfAnyExcept_WithOptimizedUpTo1Zeroes(uint[] val)
    {
        int len = val.Length;
        if (val[len - 1] == 0)
        {
            len--;
            if (len > 0 && val[len - 1] == 0)
            {
                len = val.AsSpan().LastIndexOfAnyExcept((uint)0);
                len++;
            }
        }
        return len;
    }

    /// <summary>
    /// find len replaced by Span method with optimized "no zeroes" and up to 3 zeroes cases
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    public static int UsingLastIndexOfAnyExcept_WithOptimizedUpTo3Zeroes(uint[] val)
    {
        int len = val.Length;
        if (val[len - 1] == 0)
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            bool IsPreviousElementZeroAfterDecrementLenSideeffect()
            {
                len--;
                return len > 0 && val[len - 1] == 0;
            }
            if (IsPreviousElementZeroAfterDecrementLenSideeffect())
            {
                if (IsPreviousElementZeroAfterDecrementLenSideeffect())
                {
                    if (IsPreviousElementZeroAfterDecrementLenSideeffect())
                    {
                        len = val.AsSpan().LastIndexOfAnyExcept((uint)0);
                        len++;
                    }
                }
            }
        }
        return len;
    }
}
