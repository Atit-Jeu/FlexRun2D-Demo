using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace iBoost
{
    public enum Axis
    {
        X,
        Y,
        Z,
        XY,
        XZ,
        YZ
    }

    public enum FadeMode
    {
        LERP,
        MOVE_TOWARD
    }

    public static class iExtension
    {
        // ============ TRANSFORM ============ //
        public static void SetPosition(this Transform transform, Axis axis, float value1, float value2 = 0)
        {
            float x = 0,
            y = 0,
            z = 0;
            switch(axis)
            {
                case Axis.X:
                {
                    x = value1;
                    y = transform.position.y;
                    z = transform.position.z;
                    break;
                }
                case Axis.Y:
                {
                    x = transform.position.x;
                    y = value1;
                    z = transform.position.z;
                    break;
                }
                case Axis.Z:
                {
                    x = transform.position.x;
                    y = transform.position.y;
                    z = value1;
                    break;
                }
                case Axis.XY:
                {
                    x = value1;
                    y = value2;
                    z = transform.position.z;
                    break;
                }
                case Axis.XZ:
                {
                    x = value1;
                    y = transform.position.y;
                    z = value2;
                    break;
                }
                case Axis.YZ:
                {
                    x = transform.position.x;
                    y = value1;
                    z = value2;
                    break;
                }
            }
            transform.position = new Vector3(x, y, z);
        }


        public static void SetRotation(this Transform transform, Axis axis, float value, float value2 = 0f)
        {
            Vector3 euler = transform.rotation.eulerAngles;
            switch (axis)
            {
                case Axis.X: transform.rotation = Quaternion.Euler(new Vector3(value, euler.y, euler.z)); break;
                case Axis.Y: transform.rotation = Quaternion.Euler(new Vector3(euler.x, value, euler.z)); break;
                case Axis.Z: transform.rotation = Quaternion.Euler(new Vector3(euler.x, euler.y, value)); break;
                case Axis.XY: transform.rotation = Quaternion.Euler(new Vector3(value, value2, euler.z)); break;
                case Axis.XZ: transform.rotation = Quaternion.Euler(new Vector3(value, euler.y, value2)); break;
                case Axis.YZ: transform.rotation = Quaternion.Euler(new Vector3(euler.x, value, value2)); break;
            }
        }

        // =========== MOVING ========== //
        public static void MoveSin(this Transform transform, Axis axis, float speed, float range)
        {
            switch (axis)
            {
                case Axis.X: transform.position += new Vector3(Mathf.Sin(Time.time * speed) * range * Time.deltaTime, 0f, 0f); break;
                case Axis.Y: transform.position += new Vector3(0f, Mathf.Sin(Time.time * speed) * range * Time.deltaTime, 0f); break;
                case Axis.Z: transform.position += new Vector3(0f, 0f, Mathf.Sin(Time.time * speed) * range * Time.deltaTime); break;
                case Axis.XY: transform.position += new Vector3(Mathf.Sin(Time.time * speed) * range * Time.deltaTime, Mathf.Sin(Time.time * speed) * range * Time.deltaTime, 0f); break;
                case Axis.XZ: transform.position += new Vector3(Mathf.Sin(Time.time * speed) * range * Time.deltaTime, 0f, Mathf.Sin(Time.time * speed) * range * Time.deltaTime); break;
                case Axis.YZ: transform.position += new Vector3(0f, Mathf.Sin(Time.time * speed) * range * Time.deltaTime, Mathf.Sin(Time.time * speed) * range * Time.deltaTime); break;
            }
        }


        public static void LookAt2D(this Transform transform, Vector3 target, bool convertToScreen = true, Camera camera = null)
        {
            if(camera == null) 
            {
                camera = Camera.main;
            }

            var pos = camera.WorldToScreenPoint(transform.position);

            if(convertToScreen) target = camera.WorldToScreenPoint(target);

            var dir = target - pos;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward); 
        }

        public static void MoveForward(this Transform transform, float speed)
        {
            transform.position += transform.forward * speed;
        }
        public static void MoveBackward(this Transform transform, float speed)
        {
            transform.position += transform.forward * -speed;
        }

        public static void MoveRight(this Transform transform, float speed)
        {
            transform.position += transform.right * speed;
        }
        public static void MoveLeft(this Transform transform, float speed)
        {
            transform.position += transform.right * -speed;
        }

        public static void MoveUp(this Transform transform, float speed)
        {
            transform.position += transform.up * speed;
        }
        public static void MoveDown(this Transform transform, float speed)
        {
            transform.position += transform.up * -speed;
        }

        // =========== GRAPHIC ========= //
        public static IEnumerator FadeAlpha(this Graphic graphic, float value, float step, FadeMode fadeMode = FadeMode.LERP)
        {
            while(!graphic.color.a.SafeEquals(value))
            {
                Color c = graphic.color;
                c.a = (fadeMode == FadeMode.LERP) ? Mathf.Lerp(c.a, value, step) : 
                                                    Mathf.MoveTowards(c.a, value, step);
                graphic.color = c;
                yield return null;
            }
        }

        // =========== MATH ========== //
        public static bool SafeEquals(this float t, float value, float equalRate = 0.001f)
        {
            return Mathf.Abs(t - value) < equalRate;
        }
    }
}