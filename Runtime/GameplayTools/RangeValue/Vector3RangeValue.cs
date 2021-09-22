using UnityEditor;
using UnityEngine;

namespace DoubleDash.GameplayTools
{
    public class Vector3RangeValue : BaseRangeValue<Vector3>
    {
        public Vector3RangeValue(Vector3 min, Vector3 max) : base(min, max)
        {
        }

        protected override bool InsideInternal(Vector3 target)
        {
            return CheckInsideFloat(target.x, Min.x, Max.x) && CheckInsideFloat(target.y, Min.y, Max.y) &&
                   CheckInsideFloat(target.z, Min.z, Max.z);
        }

        private bool CheckInsideFloat(float value, float min, float max)
        {
            return value >= min && value < max;
        }
        
        private bool CheckInsideFloatInclusiveMax(float value, float min, float max)
        {
            return value >= min && value <= max;
        }

        public override bool InsideMaxInclude(Vector3 target)
        {
            return CheckInsideFloatInclusiveMax(target.x, Min.x, Max.x) && CheckInsideFloatInclusiveMax(target.y, Min.y, Max.y) &&
                   CheckInsideFloatInclusiveMax(target.z, Min.z, Max.z);
        }

        public override Vector3 Clamp(Vector3 target)
        {
            return new Vector3(Mathf.Clamp(target.x, Min.x, Max.x), Mathf.Clamp(target.y, Min.y, Max.y),
                Mathf.Clamp(target.z, Min.z, Max.z));
        }
    }
}