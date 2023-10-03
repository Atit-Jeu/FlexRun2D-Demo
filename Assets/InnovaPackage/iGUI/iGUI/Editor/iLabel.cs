using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace InnovaFramework.iGUI
{
    public class iLabel : iObject 
    {
        public int fontSize = -1;

        private bool cachedResize = false;
        public void Resize()
        {
            if(style == null)
            {
                style = new GUIStyle(EditorStyles.label);
            }

            if(style != null)
            {
                size = style.CalcSize(new GUIContent(text));
                GUI.changed = true;
            }
            cachedResize = false;
            RePositionWithLastTwoRelativePosition();
        }

        public void SetText(string text, bool autoSize = true)
        {
            this.text = text;
            if(autoSize)
            {
                try
                {
                    Resize();
                }
                catch
                {
                    cachedResize = true;
                }
            }
        }

        public override void Render()
        {
            if(style == null)
            {
                style = new GUIStyle(EditorStyles.label);
            }

            if (fontSize != -1) 
            {
                style.fontSize = fontSize;
            }

            if(!active) return;


            BeginProcessProperty();
            if(cachedResize)
            {
                Resize();
                RePositionWithLastTwoRelativePosition();
            }

            GUI.Label
            ( 
                new Rect(position, size),
                new GUIContent(text, texture, tooltips),
                style
            );
            EndProcessProperty();
        }
    }
}
