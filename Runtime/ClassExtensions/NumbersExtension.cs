public static class NumbersExtensions
{
    public static int SafeDiv(this int a, int b)
    {
        if (b == 0)
        {
            return 0;
        }
        return a/b;
    }

    public static float SafeDiv(this float a, float b)
    {
        if (b == 0)
        {
            return 0f;
        }
        return a / b;
    }

    public static double SafeDiv(this double a, double b)
    {
        if (b == 0)
        {
            return 0d;
        }
        return a / b;
    }
}
