using UnityEngine;

namespace DoubleDash.LogicalPhysics2D
{
    [System.Serializable]
    public class RectArea2DChecker<T> : BaseArea2DChecker<T>
    {
        [Header("Box Area Config")] public Vector2 Size;


        protected override int DoPhysicsLogic(Vector3 origin, Collider2D[] colliders, LayerMask mask)
        {
            var size = Size / 2f;
            var pointA = origin.ToVector2() - size;
            var pointB = origin.ToVector2() + size;
            return Physics2D.OverlapAreaNonAlloc(pointA, pointB, colliders, mask);
        }
    }
}