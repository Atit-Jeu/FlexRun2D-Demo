using System.Collections;
using System.Collections.Generic;
using System.Linq;
using InnovaFramework.iGUI;
using UnityEditor;
using UnityEngine;

internal class BuiltInIconContainer
{
    public string name;
    public Texture2D texture;
}


public partial class BuiltInUnityIcon: iWindow
{
    private List<BuiltInIconContainer> icons = new List<BuiltInIconContainer>();

    // This function will call after UI initialized
    private void OnBegin()
    {
        ReloadIcon();
    }


    private void ReloadIcon()
    {
        icons.Clear();

        foreach(var s in iconsList) 
        {
			Debug.unityLogger.logEnabled = false;
            GUIContent gc = EditorGUIUtility.IconContent(s);
			Debug.unityLogger.logEnabled = true;

            if (gc == null)
            {
                continue;
            }
            if (gc.image == null)
            {
                continue;
            }

            icons.Add(new BuiltInIconContainer() 
            {
                name = s,
                texture = (Texture2D)gc.image
            });
        }

        icons = icons.OrderBy(o => o.name).ToList();
        System.GC.Collect();
        inputSearch.stringValue = "";
        GUI.FocusControl(null);
        GUI.changed = true;
        RenderIcon(icons);
    }


    private void OnSearchChange(iObject sender)
    {
        string search = inputSearch.value.ToString();

        var filtered = icons.FindAll( o => 
        {
            if (string.IsNullOrEmpty(search))
            {
                return true;
            }

            return o.name.ToLower().Contains(search.ToLower());
        });

        RenderIcon(filtered);
    }


    private void OnRefreshIcon(iObject sender)
    {
        ReloadIcon();
    }


    private void RenderIcon(List<BuiltInIconContainer> icons)
    {
        scvContainer.RemoveChildAll();
        for(int i = 0; i < icons.Count; i++)
        {
            var item = new BuiltInUnityIconItem(icons[i].name, icons[i].texture);
            scvContainer.AddChild(item);
        }
        labelFound.SetText("Icons: " + icons.Count);
    }


}