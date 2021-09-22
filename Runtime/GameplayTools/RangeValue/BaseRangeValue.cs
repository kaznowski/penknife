using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DoubleDash.GameplayTools
{
    [System.Serializable]
    public abstract class BaseRangeValue<T>
    {
        [SerializeField] private T min;
        [SerializeField] private T max;

        public T Max
        {
            get { return max; }
        }

        public T Min
        {
            get { return min; }
        }

        public BaseRangeValue(T min, T max)
        {
            this.min = min;
            this.max = max;
        }

        public bool Inside(T target, bool maxInclusive = false)
        {
            if (maxInclusive)
                return InsideMaxInclude(target);
            return InsideInternal(target);
        }

        protected abstract bool InsideInternal(T target);

        public abstract bool InsideMaxInclude(T target);

        public abstract T Clamp(T target);
    }
}