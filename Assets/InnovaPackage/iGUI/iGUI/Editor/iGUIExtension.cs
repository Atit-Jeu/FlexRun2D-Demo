
using UnityEngine;

namespace InnovaFramework.iGUI
{
    public static class iGUIExtension
    {
        public static float space(this float self, float spaceX = 2)
        {
            return self - (iGUIUtility.space * spaceX);
        }

        public static float spaceH(this float self, float spaceX = 2)
        {
            return self - (iGUIUtility.spaceHalf * spaceX);
        }


        public static float Split(this float self, int splitTo)
        {
            return iGUIUtility.SplitSize(self, splitTo);
        }


        public static Vector2 Split(this Vector2 self, int splitTo)
        {
            return iGUIUtility.SplitSize(self, splitTo);
        }


        public static Vector2 space(this Vector2 self, float space = 2)
        {
            float s = (iGUIUtility.space * space);
            return self - new Vector2(s, s);
        }


        public static Vector2 spaceH(this Vector2 self, float space = 2)
        {
            float s = (iGUIUtility.spaceHalf * space);
            return self - new Vector2(s, s);
        }


        public static Vector2 RatioY(this Vector2 self, float to)
        {
            float ratio = to / self.y;
            return new Vector2(self.x * ratio, to);
        }


        public static Vector2 RatioX(this Vector2 self, float to)
        {
            float ratio = to / self.x;
            return new Vector2(to, self.y * ratio);
        }


        public static void LoadClickableTexture(this GUIStyle self, string textureName, float lighter = 0.2f, float darker = 0.3f)
        {
            self.LoadClickableTexture(iGUIUtility.LoadBuiltInIcon(textureName, true));
        }


        public static void LoadClickableTexture(this GUIStyle self, Texture2D texture, float lighter = 0.2f, float darker = 0.3f)
        {
            if (texture == null) return;
            if (!texture.isReadable) return;

            self.normal.background = texture;
            self.hover.background  = iGUIUtility.LighterTexture(texture, lighter);
            self.active.background = iGUIUtility.DarkerTexture(texture, darker);
        }
    }
}