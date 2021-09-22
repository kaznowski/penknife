using UnityEngine;

namespace DoubleDash.LogicalPhysics2D
{
    [System.Serializable]
    public class CapsuleArea2DChecker<T> : BaseArea2DChecker<T>
    {
        [Header("Capsule2D Area Config")] 
        public Vector2 Size;
        public float Angle;
        public CapsuleDirection2D Direction2D;

        protected override int DoPhysicsLogic(Vector3 origin, Collider2D[] colliders, LayerMask mask)
        {
            return Physics2D.OverlapCapsuleNonAlloc(origin, Size, Direction2D, Angle, colliders, mask);
        }
    }
}