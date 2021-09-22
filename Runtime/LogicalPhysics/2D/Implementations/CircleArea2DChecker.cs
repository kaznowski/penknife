using UnityEngine;

namespace DoubleDash.LogicalPhysics2D
{
    [System.Serializable]
    public class CircleArea2DChecker<T> : BaseArea2DChecker<T>
    {
        [Header("Area Circle")]
        public float Range;
    
        protected override int DoPhysicsLogic(Vector3 origin, Collider2D[] colliders, LayerMask mask)
        {
            return Physics2D.OverlapCircleNonAlloc(origin, Range, colliders, mask);
        }
    }
}