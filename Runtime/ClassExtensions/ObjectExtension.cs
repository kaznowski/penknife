using System;

public static class ObjectExtension
{
    public static void ThrowIfNull(this object o, string name)
    {
        if (o == null) throw new NullReferenceException(name);
    }

    public static void ThrowIfNegative(this int n, string name)
    {
        if (n < 0) throw new ArgumentOutOfRangeException(name, n, "argument cannot be negative");
    }

    public static void ThrowIfNegative(float x, string name)
    {
        if (x < 0) throw new ArgumentOutOfRangeException(name, x, "argument cannot be negative");
    }
}

