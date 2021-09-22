using UnityEngine;

namespace DoubleDash.GameplayTools
{
    public static class NumericRangeValuesExtensions
    {
        public static float GetRandom(this RangeValue<float> me)
        {
            return Random.Range(me.Min, me.Max);
        }
        
        public static int GetRandom(this RangeValue<int> me)
        {
            return Random.Range(me.Min, me.Max);
        }

        public static Vector3 GetRandom(this Vector3RangeValue me)
        {
            return new Vector3(Random.Range(me.Min.x, me.Max.x), Random.Range(me.Min.y, me.Max.y),
                Random.Range(me.Min.z, me.Max.z));
        }
    }
}