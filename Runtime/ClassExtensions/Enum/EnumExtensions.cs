using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnumExtensions
{
    public static void ForeachFlag<T>(T enumTarget,  Action<T> action)
    {
        var checkTypeValues = Enum.GetValues(typeof(T));
        foreach (T value in checkTypeValues)
        {
            if (((int)(object)enumTarget & (int)(object)value) == (int)(object)value)
            {
                action(value);
            }
        }
    }

    public static bool Has<T>(this System.Enum type, T value)
    {
        try
        {
            return (((int)(object)type & (int)(object)value) == (int)(object)value);
        }
        catch
        {
            return false;
        }
    }

    public static IEnumerable<Enum> GetFlags(this System.Enum input)
    {
        foreach (Enum value in Enum.GetValues(input.GetType()))
            if (input.HasFlag(value))
                yield return value;
    }

}
