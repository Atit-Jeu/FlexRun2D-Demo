using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.SerializableAttribute]
public class PolarCoordinates
{
    public float radius;
    public float degree;

    public PolarCoordinates(float radius = 0, float degree = 0)
    {
        this.radius = radius;
        this.degree = degree;
    }
}

namespace iBoost
{
    public static class iMath
    {
        // ========== COORDINATE ========== //
        public static PolarCoordinates RectToPolar(Vector3 vec, Vector3 origin) { return RectToPolar((Vector2)vec, (Vector2)origin); }
        public static PolarCoordinates RectToPolar(Vector2 vec, Vector3 origin) { return RectToPolar(vec, (Vector2)origin); }
        public static PolarCoordinates RectToPolar(Vector3 vec, Vector2 origin) { return RectToPolar((Vector2)vec, origin); }
        public static PolarCoordinates RectToPolar(Vector2 vec, Vector2 origin)
        {
            var polar = new PolarCoordinates();

            Vector2 deltaPosition = vec - origin;
            float degree = Mathf.Atan2(deltaPosition.y, deltaPosition.x) * Mathf.Rad2Deg;
            float radius = Mathf.Sqrt( deltaPosition.x * deltaPosition.x + deltaPosition.y * deltaPosition.y );

            polar.radius = radius;
            polar.degree = degree;

            return polar;
        }

        public static Vector2 PolarToRect(PolarCoordinates polar)
        {
            var rect = new Vector2();
            rect.x = polar.radius * Mathf.Cos(polar.degree * Mathf.Deg2Rad);
            rect.y = polar.radius * Mathf.Sin(polar.degree * Mathf.Deg2Rad);
            return rect;
        }

        // ============ FLOOR ROUND CEIL ========== //
        public static Vector3 RoundVector3(Vector3 vector)
        {
            vector.x = Mathf.Round(vector.x);
            vector.y = Mathf.Round(vector.y);
            vector.z = Mathf.Round(vector.z);
            return vector;
        }
        public static Vector3 FloorVector3(Vector3 vector)
        {
            vector.x = Mathf.Floor(vector.x);
            vector.y = Mathf.Floor(vector.y);
            vector.z = Mathf.Floor(vector.z);
            return vector;
        }
        public static Vector3 CeilVector3(Vector3 vector)
        {
            vector.x = Mathf.Ceil(vector.x);
            vector.y = Mathf.Ceil(vector.y);
            vector.z = Mathf.Ceil(vector.z);
            return vector;
        }
        public static Vector2 RoundVector2(Vector2 vector)
        {
            vector.x = Mathf.Round(vector.x);
            vector.y = Mathf.Round(vector.y);
            return vector;
        }
        public static Vector2 FloorVector2(Vector2 vector)
        {
            vector.x = Mathf.Floor(vector.x);
            vector.y = Mathf.Floor(vector.y);
            return vector;
        }
        public static Vector2 CeilVector2(Vector2 vector)
        {
            vector.x = Mathf.Ceil(vector.x);
            vector.y = Mathf.Ceil(vector.y);
            return vector;
        }
    }
}
