using System;
using UnityEngine;

namespace DoubleDash.LogicalPhysics2D
{
    /// <summary>
    /// Struct that storage a trigger area result, with all objects of "TargetType" inside area.
    /// </summary>
    /// <typeparam name="TargetType">Type of object that you want. Note: this use GetComponent on collider to find</typeparam>
    [System.Serializable]
    public struct Area2DResult<TargetType>
    {
        public int Count;
        public Area2DResultInfo<TargetType>[] ResultInfos;
        public bool Any => Count != 0;

        public Area2DResult(int count, Collider2D[] rawArray, TargetType[] results)
        {
            var resultsCount = 0;
            if (results != null)
                resultsCount = results.Length;
            Count = count;
            ResultInfos = new Area2DResultInfo<TargetType>[resultsCount];
            int currentIndex = 0;
            for (int i = 0; i < count; i++)
            {
                TargetType result = rawArray[i].GetComponent<TargetType>();
                if (result == null)
                {
                    continue;
                }

                ResultInfos[currentIndex] =
                    new Area2DResultInfo<TargetType>(result, rawArray[i], Time.realtimeSinceStartupAsDouble);
                currentIndex++;
            }

            Count = currentIndex;
        }

        public TargetType this[int index]
        {
            get => ResultInfos[index].Value;
        }

        public bool Contains(TargetType obj)
        {
            for (int i = 0; i < Count; i++)
            {
                if (ResultInfos[i].Value.Equals(obj))
                    return true;
            }

            return false;
        }

        public void ForEach(Action<TargetType> lambda)
        {
            for (int i = 0; i < Count; i++)
            {
                var obj = ResultInfos[i].Value;
                if (obj == null)
                    continue;
                lambda(obj);
            }
        }

        public void OrganizeByEnterTime()
        {
            for (var i = 0; i < Count; i++)
            {
                var min = i;
                for (var j = i + 1; j < Count; j++)
                {
                    if (ResultInfos[min].FindAt >= ResultInfos[j].FindAt)
                    {
                        min = j;
                    }
                }

                if (min != i)
                {
                    var lowerValue = ResultInfos[min];
                    ResultInfos[min] = ResultInfos[i];
                    ResultInfos[i] = lowerValue;
                }
            }
        }

        public void OrganizeBy(Func<TargetType, float> logic)
        {
            for (var i = 0; i < Count; i++)
            {
                var min = i;
                for (var j = i + 1; j < Count; j++)
                {
                    if (logic(ResultInfos[min].Value) >= logic(ResultInfos[j].Value))
                    {
                        min = j;
                    }
                }

                if (min != i)
                {
                    var lowerValue = ResultInfos[min];
                    ResultInfos[min] = ResultInfos[i];
                    ResultInfos[i] = lowerValue;
                }
            }
        }

        public TargetType[] GetResults(int size)
        {
            var s = Mathf.Min(size, Count);
            var result = new TargetType[s];
            for (int i = 0; i < s; i++)
            {
                result[i] = ResultInfos[i].Value;
            }

            return result;
        }

        public bool TryGetInfo(TargetType currentObj, out Area2DResultInfo<TargetType> o)
        {
            o = default;
            for (int i = 0; i < Count; i++)
            {
                if (ResultInfos[i].Value.Equals(currentObj))
                {
                    o = ResultInfos[i];
                    return true;
                }
            }

            return false;
        }
    }

    [System.Serializable]
    public struct Area2DResultInfo<T>
    {
        public T Value;
        public Collider2D Collider2D;
        public double FindAt;

        public Area2DResultInfo(T value, Collider2D collider2D, double findAt)
        {
            Value = value;
            Collider2D = collider2D;
            FindAt = findAt;
        }
    }
}