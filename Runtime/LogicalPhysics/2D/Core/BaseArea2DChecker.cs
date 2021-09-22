using UnityEngine;

namespace DoubleDash.LogicalPhysics2D
{
    [System.Serializable]
    public abstract class BaseArea2DChecker<T>
    {
        [Header("Basic Variables")]
        public Transform CenterPosition;
        public LayerMask Mask;
        public int MaxResults = 10;

        [Header("Events")]
        public GameEvent<Area2DResultInfo<T>> OnEnterArea = new GameEvent<Area2DResultInfo<T>>();
        public GameEvent<Area2DResultInfo<T>> OnStayArea = new GameEvent<Area2DResultInfo<T>>();
        public GameEvent<Area2DResultInfo<T>> OnExitArea = new GameEvent<Area2DResultInfo<T>>();

        [Header("Internal Variables")]
        [SerializeField] private Collider2D[] collidersArray;
        [SerializeField] private T[] resultsArray;
        [SerializeField] private Area2DResult<T> lastResult = new Area2DResult<T>(0, new Collider2D[0], new T[0]);
    
    
    
        public void SetTransform(Transform transform)
        {
            this.CenterPosition = transform;
        }

        public Area2DResult<T> GetAreaResultFromTransform()
        {
            if (CenterPosition == null)
                return new Area2DResult<T>(0, collidersArray, resultsArray);
            return GetAreaResult(CenterPosition.position);
        }

        public Area2DResult<T> GetAreaResult(Vector3 origin)
        {
            if (collidersArray == null || resultsArray == null)
            {
                collidersArray = new Collider2D[MaxResults];
                resultsArray = new T[MaxResults];
            }
            var count = DoPhysicsLogic(origin, collidersArray, Mask);
            var currentResult = new Area2DResult<T>(count, collidersArray, resultsArray);
            NormalizeCurrentResultByLastResult(ref currentResult);
            lastResult = currentResult;
            return currentResult;
        }

        protected abstract int DoPhysicsLogic(Vector3 origin, Collider2D[] colliders, LayerMask mask);

        private void NormalizeCurrentResultByLastResult(ref Area2DResult<T> currentResult)
        {
            //Check which objects has left from the current detection list this frame and trigger OnExit 
            for (int i = 0; i < lastResult.Count; i++)
            {
                var oldObj = lastResult[i];
                if (currentResult.Contains(oldObj) || oldObj == null)
                    continue;
                OnExitArea.Trigger(lastResult.ResultInfos[i]);
            }

            //Check current object list for trigger OnStay or OnEnter.
            for (int i = 0; i < currentResult.Count; i++)
            {
                var currentObj = currentResult[i];
                if (currentObj == null)
                    continue;
                if (lastResult.TryGetInfo(currentObj, out var info))
                {
                    currentResult.ResultInfos[i].FindAt = info.FindAt;
                    OnStayArea.Trigger(currentResult.ResultInfos[i]);
                }
                else
                {
                    OnEnterArea.Trigger(currentResult.ResultInfos[i]);
                }
            }
        }
    }
}