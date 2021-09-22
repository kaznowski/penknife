using System;

namespace DoubleDash.GameplayTools
{
    [System.Serializable]
    public class RangeValue<T> : BaseRangeValue<T> where T : IComparable<T>
    {
        public RangeValue(T min, T max) : base(min, max)
        {
            
        }

        protected override bool InsideInternal(T target)
        {
            return target.CompareTo(Min) >= 0 && target.CompareTo(Max) < 0;
        }

        public override bool InsideMaxInclude(T target)
        {
            return target.CompareTo(Min) >= 0 && target.CompareTo(Max) <= 0;
        }

        public override T Clamp(T target)
        {
            if (target.CompareTo(Max) > 0)
                return Max;
            if (target.CompareTo(Min) < 0)
                return Min;
            return target;
        }
    }
}