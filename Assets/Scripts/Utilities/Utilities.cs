using UnityEngine;

namespace ElementProject
{
    public class Utilities
    {
    public static Vector2 RotateVector(Vector2 v, float degrees)
    {
    float radians = degrees * Mathf.Deg2Rad;
    float sin = Mathf.Sin(radians);
    float cos = Mathf.Cos(radians);
    return new Vector2(
        v.x * cos - v.y * sin,
        v.x * sin + v.y * cos
    );
    }
    }
}
