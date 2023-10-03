using InnovaFramework.iGUI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class iTab : iObject
{
    public int index = 0;

    public Action<iTab> OnChange = null;

    private List<GUIContent>               contentHeaders  = new List<GUIContent>();
    private Dictionary<iObject, int>       cachedObj2Index = new Dictionary<iObject, int>();
    private Dictionary<int, List<iObject>> tabs            = new Dictionary<int, List<iObject>>();


    public iTab()
    {
        size = new Vector2(24, EditorGUIUtility.singleLineHeight * 2);
    }


    public override void Render()
    {
        if(!active) return;

        BeginProcessProperty();
        EditorGUI.BeginChangeCheck();
        index = GUI.Toolbar(rect, index, contentHeaders.ToArray());
        if(EditorGUI.EndChangeCheck())
        {
            OnChange?.Invoke(this);
        }

        if (tabs.ContainsKey(index))
        {
            foreach(var obj in tabs[index])
            {
                obj.Render();
            }
        }
        EndProcessProperty();
    }


    public GUIContent[] GetHeaders()
    {
        return contentHeaders.ToArray();
    }


    public void SetHeaders(params string[] texts)
    {
        contentHeaders.Clear();

        for(int i = 0; i < texts.Length; i++)
        {
            GUIContent content = new GUIContent();
            content.text = texts[i];
            contentHeaders.Add(content);
        }
    }


    public void SetHeaders(params Texture[] textures)
    {
        for(int i = 0; i < textures.Length; i++)
        {
            GUIContent content = new GUIContent();
            content.image = textures[i];
            contentHeaders.Add(content);
        }
    }


    public void SetHeaders(params GUIContent[] contents)
    {
        contentHeaders.Clear();
        contentHeaders.AddRange(contents);
    }


    public override void ReRelative()
    {
        base.ReRelative();

        foreach(var kv in cachedObj2Index)
        {
            kv.Key.ReRelative();
        }
    }


    public void AddChild(iObject obj, int index)
    {
        if (!tabs.ContainsKey(index))
        {
            tabs[index] = new List<iObject>();
        }

        if (cachedObj2Index.ContainsKey(obj))
        {
            return;
        }

        tabs[index].Add(obj);
        cachedObj2Index[obj] = index;
        obj.window = window;
    }


    public void RemoveChild(iObject obj)
    {
        if (!cachedObj2Index.ContainsKey(obj))
        {
            return;
        }

        int index = cachedObj2Index[obj];
        cachedObj2Index.Remove(obj);
        tabs[index].Remove(obj);
        obj.window = null;
    }
}
