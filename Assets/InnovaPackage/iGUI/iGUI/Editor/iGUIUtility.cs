using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace InnovaFramework.iGUI
{
    public class iGUIUtility
    {
        public static float space = 8;
        public static float spaceX2 { get { return space * 2; } }
        public static float spaceX3 { get { return space * 3; } }
        public static float spaceX4 { get { return space * 4; } }
        public static float spaceX5 { get { return space * 5; } }
        public static float spaceHalf { get { return space * 0.5f; } }


        public static float SplitSize(float size, int splitTo)
        {
            return (size - (iGUIUtility.space * (splitTo - 1))) / splitTo;;
        }


        public static Vector2 SplitSize(Vector2 size, int splitTo)
        {
            Vector2 vec = new Vector2();
            vec.x = (size.x - (iGUIUtility.space * (splitTo - 1))) / splitTo;
            vec.y = (size.y - (iGUIUtility.space * (splitTo - 1))) / splitTo;
            return vec;
        }


        public static float[] SplitSizeByPercentage(float size, params float[] percentage)
        {
            List<float> percents = new List<float>();
            size -= (percentage.Length - 1) * iGUIUtility.space;

            float sum = percentage.Sum();
            for(int i = 0; i < percentage.Length; i++)
            {
                float s = size * (percentage[i] / sum);
                percents.Add( s );
            }

            return percents.ToArray();
        }


        public static float HeightBetween2Objects(iObject above, iObject bottom, float space = 8)
        {
            float p1 = above.position.y + above.height;
            float p2 = bottom.position.y;
            return HeightBetween2Objects(p1, p2, space);
        }


        public static float HeightBetween2Objects(float above, iObject bottom, float space = 8)
        {
            float p2 = bottom.position.y;
            return HeightBetween2Objects(above, p2, space);
        }


        public static float HeightBetween2Objects(iObject above, float bottom, float space = 8)
        {
            float p1 = above.position.y + above.height;
            return HeightBetween2Objects(p1, bottom, space);
        }


        public static float HeightBetween2Objects(float above, float bottom, float space = 8)
        {
            return bottom - above - (space * 2);
        }


        public static float WidthBetween2Objects(iObject left, iObject right, float space = 8)
        {
            float p1 = left.position.x + left.width;
            float p2 = right.position.x;
            return WidthBetween2Objects(p1, p2, space);
        }


        public static float WidthBetween2Objects(float left, iObject right, float space = 8)
        {
            float p2 = right.position.x;
            return WidthBetween2Objects(left, p2, space);
        }


        public static float WidthBetween2Objects(iObject left, float right, float space = 8)
        {
            float p1 = left.position.x + left.width;
            return WidthBetween2Objects(p1, right);
        }


        public static float WidthBetween2Objects(float left, float right, float space = 8)
        {
            return right - left - (space * 2);
        }


        public static Vector2 CalculateSizeFromObjects(params iObject[] mObjects)
        {
            Vector2 size = new Vector2();

            Vector2 minPos = new Vector2();
            Vector3 maxSize = new Vector3();
            for(int i = 0; i < mObjects.Length; i++)
            {
                if(maxSize.x < mObjects[i].right)
                {
                    maxSize.x = mObjects[i].right;
                }

                if(maxSize.y < mObjects[i].bottom)
                {
                    maxSize.y = mObjects[i].bottom;
                }

                if(i == 0)
                {
                    minPos = mObjects[i].position;
                }
                else
                {
                    if(minPos.x > mObjects[i].position.x)
                    {
                        minPos.x = mObjects[i].position.x;
                    }

                    if(minPos.y > mObjects[i].position.y)
                    {
                        minPos.y = mObjects[i].position.y;
                    }
                }
            }

            size.x = maxSize.x - minPos.x;
            size.y = maxSize.y - minPos.y;

            return size;
        }


        public static Texture2D DuplicateTexture(Texture2D source)
        {
            RenderTexture renderTex = RenderTexture.GetTemporary(
                        source.width,
                        source.height,
                        0,
                        RenderTextureFormat.Default,
                        RenderTextureReadWrite.Linear);
        
            Graphics.Blit(source, renderTex);
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = renderTex;
            Texture2D readableText = new Texture2D(source.width, source.height);
            readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
            readableText.Apply();
            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(renderTex);
            return readableText;
        }


        public static Texture2D LoadBuiltInIcon(string textureName, bool isReadable = false)
        {
            Texture2D tex = (Texture2D)EditorGUIUtility.IconContent(textureName).image;
            if (!isReadable)
            {
                return tex;
            }

            return DuplicateTexture(tex);
        }


        public static Texture2D DarkerTexture(Texture2D tex, float percent = 0.2f)
        {
            Color[] color = tex.GetPixels();
            for(int i = 0; i < color.Length; i++)
            {
                color[i].r -= (color[i].r * percent);
                color[i].g -= (color[i].g * percent);
                color[i].b -= (color[i].b * percent);
            }

            Texture2D newTex = new Texture2D(tex.width, tex.height);
            newTex.SetPixels(color);
            newTex.Apply();
            return newTex;
        }


        public static Texture2D LighterTexture(Texture2D tex, float percent = 0.2f)
        {
            Color[] color = tex.GetPixels();
            for(int i = 0; i < color.Length; i++)
            {
                color[i].r += (color[i].r * percent);
                color[i].g += (color[i].g * percent);
                color[i].b += (color[i].b * percent);
            }

            Texture2D newTex = new Texture2D(tex.width, tex.height);
            newTex.SetPixels(color);
            newTex.Apply();
            return newTex;
        }


        public static GUIContent CreateGUIContent(string text, string textureName, string tooltips = "")
        {
            GUIContent content = new GUIContent();
            content.text = text;
            content.image = EditorGUIUtility.IconContent(textureName).image;
            content.tooltip = tooltips;
            return content;
        }


        public static GUIContent CreateGUIContent(string text, Texture texture, string tooltips = "")
        {
            GUIContent content = new GUIContent();
            content.text = text;
            content.image = texture;
            content.tooltip = tooltips;
            return content;
        }


        public static Matrix4x4 BeginZoom(float scale, Vector2 vanishingPoint)
        {
            Matrix4x4 oldMatrix = GUI.matrix;

            Matrix4x4 Translation = Matrix4x4.TRS(vanishingPoint,Quaternion.identity,Vector3.one);
            Matrix4x4 Scale = Matrix4x4.Scale(new Vector3(scale, scale, 1.0f));
            GUI.matrix = Translation * Scale * Translation.inverse;

            return oldMatrix;
        }


        public static void ResetZoom(Matrix4x4 matrix)
        {
            GUI.matrix = matrix;
        }
    }
}