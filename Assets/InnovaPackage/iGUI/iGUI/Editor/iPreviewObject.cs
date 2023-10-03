using UnityEditor;
using UnityEngine;

namespace InnovaFramework.iGUI
{
    public class iPreviewObject : iObject
    {
        private GameObject _targetObject;
        public GameObject   targetObject
        {
            get { return _targetObject;}
            set 
            {
                _targetObject = value;
                objectPreviewEditor = Editor.CreateEditor(_targetObject);
            }
        }

        private iBox   box;
        private Editor objectPreviewEditor;

        public iPreviewObject()
        { 
            box  = new iBox();
            size = new Vector2(300, 300);
        }

        public override void Start()
        {
            if (style == null)
            {
                style = new GUIStyle();
                style.normal.background = EditorGUIUtility.whiteTexture;
            }
        }

        public override void Render()
        {
            base.Render();

            if(!active) return;

            BeginProcessProperty();
            box.size     = size;
            box.position = this.position;
            box.Render();
            if (_targetObject != null && objectPreviewEditor != null)
            {
                objectPreviewEditor.OnPreviewGUI(rect, style);
            }
            EndProcessProperty();
        }

        public void Repaint()
        {
            if (objectPreviewEditor != null)
            {
                objectPreviewEditor.ReloadPreviewInstances();
            }
        }
    }

}