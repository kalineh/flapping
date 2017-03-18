using UnityEngine;
using System.Collections;

public static class MathE
{
    public static float Clamp11(float f)
    {
        f = Mathf.Min(f, +1.0f);
        f = Mathf.Max(f, -1.0f);
        return f;
    }
}

public static class VectorExtension
{
    public static Vector3 UnitCircleXY(float degrees)
    {
        var rad = degrees * Mathf.Deg2Rad;
        var s = Mathf.Sin(rad);
        var c = Mathf.Cos(rad);
        var result = new Vector3(c, s, 0.0f);

        return result;
    }

    public static Vector3 UnitCircleXZ(float degrees)
    {
        var rad = degrees * Mathf.Deg2Rad;
        var s = Mathf.Sin(rad);
        var c = Mathf.Cos(rad);
        var result = new Vector3(c, 0.0f, s);

        return result;
    }

    public static Vector3 _X00(this Vector3 self) { return new Vector3(self.x, 0.0f, 0.0f); }
    public static Vector3 _0Y0(this Vector3 self) { return new Vector3(0.0f, self.y, 0.0f); }
    public static Vector3 _00Z(this Vector3 self) { return new Vector3(0.0f, 0.0f, self.z); }

    public static Vector3 WithX(this Vector3 self, float x) { return new Vector3(x, self.y, self.z); }
    public static Vector3 WithY(this Vector3 self, float y) { return new Vector3(self.x, y, self.z); }
    public static Vector3 WithZ(this Vector3 self, float z) { return new Vector3(self.x, self.y, z); }

    public static Vector3 FlattenX(this Vector3 self)
    {
        return new Vector3(self.x, 0f, 0f);
    }

    public static Vector3 FlattenXY(this Vector3 self)
    {
        return new Vector3(self.x, self.y, 0.0f);
    }

    public static Vector3 FlattenXZ(this Vector3 self)
    {
        return new Vector3(self.x, 0.0f, self.z);
    }

    public static float LargestDimensionXYZ(this Vector3 self)
    {
        var x = self.x < 0.0f ? -self.x : self.x;
        var y = self.y < 0.0f ? -self.y : self.y;
        var z = self.z < 0.0f ? -self.z : self.z;

        if (x > y && x > z)
            return x;
        if (y > z)
            return y;
        return z;
    }

    public static Vector3 LargestDimensionOnly(this Vector3 self)
    {
        var x = Mathf.Abs(self.x);
        var y = Mathf.Abs(self.y);
        var z = Mathf.Abs(self.z);

        if (x >= y && x >= z)
            return new Vector3(self.x, 0.0f, 0.0f);
        if (y >= z)
            return new Vector3(0.0f, self.y, 0.0f);

        return new Vector3(0.0f, 0.0f, self.z);
    }

    public static bool IsZeroEpsilon(this Vector3 self)
    {
        float sqm = self.sqrMagnitude;
        if (sqm < Vector3.kEpsilon)
            return true;
        return false;
    }

    public static Vector3 Snap(this Vector3 self, float step)
    {
        if (Mathf.Approximately(step, 0.0f))
            return self;

        return new Vector3(
            Mathf.Round(self.x / step) * step,
            Mathf.Round(self.y / step) * step,
            Mathf.Round(self.z / step) * step
        );
    }

    public static Vector3 Divide(Vector3 a, Vector3 b)
    {
        return new Vector3(
            a.x / b.x,
            a.y / b.y,
            a.z / b.z
        );
    }

    public static Vector3 Wave(float t, float x, float y, float z)
    {
        float time = Time.time + t;
        return new Vector3(
            Mathf.Sin(time * x),
            Mathf.Sin(time * y),
            Mathf.Sin(time * z)
        );
    }

    public static Vector3 WaveRawT(float t, float x, float y, float z)
    {
        float time = t;
        return new Vector3(
            Mathf.Sin(time * x),
            Mathf.Sin(time * y),
            Mathf.Sin(time * z)
        );
    }

    public static float SafeMagnitude(this Vector3 v)
    {
        if (v.IsZeroEpsilon())
            return 0.0f;

        return v.magnitude;
    }

    public static Vector3 SafeNormalize(this Vector3 v)
    {
        if (v.IsZeroEpsilon())
            return Vector3.zero;

        return v.normalized;
    }

    public static Vector3 Direction(Vector3 start, Vector3 end)
    {
        return (end - start).normalized;
    }

    public static Vector3 ClampMagnitudeXY(Vector3 v, float len)
    {
        var y = v.y;

        v.y = 0.0f;
        v = Vector3.ClampMagnitude(v, len);
        v.y = y;

        return v;
    }

    public static Vector3 Abs(Vector3 v)
    {
        return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
    }
}