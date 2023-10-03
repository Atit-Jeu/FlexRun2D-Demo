using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace InnovaFramework.iGUI
{
    public class iMaskField : iObject 
    {
        public List<string> options { get; private set; }
        public Action<iObject> OnChanged;
        private Dictionary<string, object> data = new Dictionary<string, object>();

        public int flags = 0;
        public string[] selectedItem
        {
            get
            {
                List<string> selected = new List<string>();
                for(int i = 0; i < options.Count; i++)
                {
                    int currentFlags = 1 << i;
                    if ((flags & currentFlags) != 0)
                    {
                        selected.Add(options[i]);
                    }
                }

                return selected.ToArray();
            }
        }


        public iMaskField()
        {
            options = new List<string>();
            size = new Vector2(150, EditorGUIUtility.singleLineHeight);
            labelSpace = 50;
        }


        public int GetIndexByOptionName(string option)
        {
            return options.IndexOf(option);
        }


        public bool AddOption(string option, object value = null)
        {
            if (options.Contains(option)) return false;
            options.Add(option);
            data[option] = value;
            return true;
        }


        public void SetOptions(string[] items)
        {
            options.Clear();
            data.Clear();

            for(int i = 0; i < items.Length; i++)
            {
                AddOption(items[i]);
            }
        }


        public void RemoveOption(string option)
        {
            options.Remove(option);
            if(data.ContainsKey(option))
            {
                data.Remove(option);
            }
        }


        public bool IsSelected (string value)
        {
            int index = options.IndexOf(value);
            if (index == -1)
            {
                return false;
            }

            return (flags & (1 << index)) != 0;
        }


        public void RemoveOptionAt(int index)
        {
            if(index < 0 || index >= options.Count)
            {
                return;
            }

            if(data.ContainsKey(options[index]))
            {
                data.Remove(options[index]);
            }

            options.RemoveAt(index);
        }


        public void ClearOption()
        {
            options.Clear();
            data.Clear();
        }


        public void SelectAll()
        {
            flags = -1;
        }


        public override void Start()
        {
            if (style == null)
            {
                style = new GUIStyle(EditorStyles.layerMaskField);
            }
        }


        public override void Render()
        {
            base.Render();

            if (!active) return;

            float labelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = text == "" ? 0 : labelSpace;

            if(style != null && contentColor.a != 0)
            {
                style.normal.textColor = contentColor;
            }

            GUI.enabled = enabled;
            EditorGUI.BeginChangeCheck();
            List<string> renderer = new List<string>(options);
            if (renderer.Count == 0)
            {
                renderer.Add("No Items");
            }

            flags = EditorGUI.MaskField(rect, new GUIContent(text, texture, tooltips), flags, renderer.ToArray(), style);
            if (EditorGUI.EndChangeCheck())
            {
                OnChanged?.Invoke(this);
            }
            GUI.enabled = true;
            EditorGUIUtility.labelWidth = labelWidth;
        }
    }
}
