using UnityEditor;
using UnityEngine;

namespace InnovaFramework.iGUI
{
    public class iRawTexture : iObject
    {
        public Camera cam;
        public ScaleMode scaleMode;

        private iBox box = new iBox();

        public iRawTexture()
        {
            size = new Vector2(256, 256);
            scaleMode = ScaleMode.StretchToFill;
        }

        public override void Render()
        {
            if(style == null)
            {
                style = new GUIStyle();
            }

            if(!active) return;

            GUI.enabled  = enabled;
            box.size     = size;
            box.position = this.position;
            box.Render();
            GUI.enabled = true;
        }
    }

}