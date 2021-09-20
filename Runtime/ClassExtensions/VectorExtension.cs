using UnityEngine;

public static class VectorExtension
{
    public const float _zeroAprox = 0.001f;
    #region Vector3
    public static Vector2 ToVector2(this Vector3 self)
    {
        return new Vector2(self.x, self.y);
    }

    public static bool IsZero(this Vector3 self)
    {
        return self.magnitude <= _zeroAprox;
    }

    public static Vector3 ToWorldPosition(this Vector3 origin, Camera cam)
    {
        if (cam == null)
        {
            Debug.Log("Cannot set To WorldPosition without camera");
            return Vector3.zero;
        }
        return cam.ScreenToWorldPoint(origin);
    }

    public static Vector3 ToWorldPosition(this Vector3 origin)
    {
        return ToWorldPosition(origin, Camera.main);
    }

    public static float DotProduct(this Vector3 a, Vector3 b)
    {
        return (a.x * b.x) + (a.y * b.y) + (a.z * b.z);
    }

    public static bool PassPoint(this Vector3 start, Vector3 newPos, Vector3 target)
    {
        var dir1 = (target - start).normalized;
        var dir2 = (target - newPos).normalized;
        return dir1 == dir2;
    }

    public static Vector3 MoveTo(this Vector3 start, Vector3 speed, Vector3 target)
    {
        if (PassPoint(start, start + speed, target))
            return target;
        return start + speed;
    }

    public static Vector3 MoveTo(this Vector3 start, Vector3 speed, Vector3 target, out bool finish)
    {
        var newPos = MoveTo(start, speed, target);
        finish = newPos == target;
        return newPos;
    }

    public static Vector3 MoveTo(this Vector3 start, float speed, Vector3 target)
    {
        var speedVector = (target - start).normalized * speed;
        return MoveTo(start, speedVector, target);
    }

    public static Vector3 MoveTo(this Vector3 start, float speed, Vector3 target, out bool finish)
    {
        var newPos = MoveTo(start, speed, target);
        finish = newPos == target;
        return newPos;
    }

    public static Vector3 WithX(this Vector3 v, float x)
    {
        return new Vector3(x, v.y, v.z);
    }

    public static Vector3 WithY(this Vector3 v, float y)
    {
        return new Vector3(v.x, y, v.z);
    }

    public static Vector3 WithZ(this Vector3 v, float z)
    {
        return new Vector3(v.x, v.y, z);
    }

    public static Vector3 Abs(this Vector3 a)
    {
        return new Vector3(Mathf.Abs(a.x), Mathf.Abs(a.y), Mathf.Abs(a.z));
    }

    public static Vector3 Filter(this Vector3 a, float range, float defaultValue = 0)
    {
        if (Mathf.Abs(a.x) <= range)
            a.x = defaultValue;
        if (Mathf.Abs(a.y) <= range)
            a.y = defaultValue;
        if (Mathf.Abs(a.z) <= range)
            a.z = defaultValue;
        return a;
    }

    public static Vector3 Repeat(this Vector3 a, float t = 0)
    {
        a.x = Mathf.Repeat(a.x, t);
        a.y = Mathf.Repeat(a.y, t);
        a.z = Mathf.Repeat(a.z, t);

        return a;
    }
    #endregion

    #region Vector2
    public static Vector2 Rotate(this Vector2 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }

    public static Vector3 ToVector3(this Vector2 other, float z)
    {
        return new Vector3(other.x, other.y, z);
    }

    public static Vector2 ToWorldPosition(this Vector2 origin, Camera cam)
    {
        if (cam == null)
        {
            Debug.Log("Cannot set To WorldPosition without camera");
            return Vector2.zero;
        }
        return cam.ScreenToWorldPoint(origin);
    }

    public static Vector2 ToWorldPosition(this Vector2 origin)
    {
        return ToWorldPosition(origin, Camera.main);
    }

    public static float DotProduct(this Vector2 a, Vector2 b)
    {
        return (a.x * b.x) + (a.y * b.y);
    }

    public static bool PassPoint(this Vector2 start, Vector2 newPos, Vector2 target)
    {
        var dir1 = (target - start).normalized;
        var dir2 = (target - newPos).normalized;
        return dir1 != dir2;
    }

    public static Vector2 MoveTo(this Vector2 start, Vector2 speed, Vector2 target)
    {
        if (PassPoint(start, start + speed, target))
            return target;
        return start + speed;
    }

    public static Vector2 MoveTo(this Vector2 start, Vector2 speed, Vector2 target, out bool finish)
    {
        var newPos = MoveTo(start, speed, target);
        finish = newPos == start;
        return newPos;
    }

    public static Vector2 MoveTo(this Vector2 start, float speed, Vector2 target)
    {
        var speedVector = (target - start).normalized * speed;
        return MoveTo(start, speedVector, target);
    }

    public static Vector2 MoveTo(this Vector2 start, float speed, Vector2 target, out bool finish)
    {
        var newPos = MoveTo(start, speed, target);
        finish = newPos == start;
        return newPos;
    }

    public static Vector2 WithX(this Vector2 v, float x)
    {
        return new Vector2(x, v.y);
    }

    public static Vector2 WithY(this Vector2 v, float y)
    {
        return new Vector2(v.x, y);
    }

    public static Vector2 Abs(this Vector2 a)
    {
        return new Vector2(Mathf.Abs(a.x), Mathf.Abs(a.y));
    }

    public static bool IsZero(this Vector2 self)
    {
        return self.magnitude <= _zeroAprox;
    }

    public static Vector2 Filter(this Vector2 a, float range, float defaultValue = 0)
    {
        if (Mathf.Abs(a.x) <= range)
            a.x = defaultValue;
        if (Mathf.Abs(a.y) <= range)
            a.y = defaultValue;
        return a;
    }
    
    public static Vector2 Repeat(this Vector2 a, float t = 0)
    {
        a.x = Mathf.Repeat(a.x, t);
        a.y = Mathf.Repeat(a.y, t);

        return a;
    }
    #endregion
}
