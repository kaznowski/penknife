using DoubleDash.LogicalPhysics2D;
using UnityEngine;

namespace LogicalPhysics._2D.Implementations
{
    [System.Serializable]
    public class BoxArea2DChecker<T> : BaseArea2DChecker<T>
    {
        [Header("Box Area Config")] 
        public Vector2 Size;
        public float Angle;

        protected override int DoPhysicsLogic(Vector3 origin, Collider2D[] colliders, LayerMask mask)
        {
            return Physics2D.OverlapBoxNonAlloc(origin, Size, Angle, colliders, mask);
        }
    }
}