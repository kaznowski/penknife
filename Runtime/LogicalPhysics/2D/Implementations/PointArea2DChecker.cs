using UnityEngine;

namespace DoubleDash.LogicalPhysics2D
{
    [System.Serializable]
    public class PointArea2DChecker<T> : BaseArea2DChecker<T>
    {
        protected override int DoPhysicsLogic(Vector3 origin, Collider2D[] colliders, LayerMask mask)
        {
            return Physics2D.OverlapPointNonAlloc(origin, colliders, mask);
        }
    }
}