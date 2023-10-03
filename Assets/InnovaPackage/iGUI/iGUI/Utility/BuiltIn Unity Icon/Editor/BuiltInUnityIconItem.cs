using System.Collections;
using System.Collections.Generic;
using InnovaFramework.iGUI;
using UnityEditor;
using UnityEngine;

public class BuiltInUnityIconItem : iObject
{
    private iBox background;
    private iBox icon;
    private iLabel txtName;
    private iButton btnCopy;

    public string iconName;
    public Texture2D iconTexture;

    public BuiltInUnityIconItem(string name, Texture2D iconTexture)
    {
        icon       = new iBox();
        background = new iBox();
        txtName    = new iLabel();
        btnCopy    = new iButton();

        this.iconName = name;
        this.iconTexture = iconTexture;

        size = new Vector2(32, 32);
    }


    public override void Start()
    {
        base.Start();

        background.size = size;
        background.position = position;
        background.backgroundColor = new Color(0.05f, 0.05f, 0.05f);

        icon.style = new GUIStyle();
        icon.style.normal.background = iconTexture;
        icon.size = icon.style.CalcSize(new GUIContent("", iconTexture));

        if (icon.size.y > 20)
        {
            icon.size = icon.size.RatioY(20);
        }
        else if (icon.size.y < 5)
        {
            icon.size = icon.size.RatioY(10);
        }

        icon.RelativePosition(iRelativePosition.LEFT_IN    , background);
        icon.RelativePosition(iRelativePosition.CENTER_Y_OF, background);

        btnCopy.size.x = 50;
        btnCopy.text = "Copy";
        btnCopy.miniButton = true;
        btnCopy.RelativePosition(iRelativePosition.RIGHT_IN, background);
        btnCopy.RelativePosition(iRelativePosition.CENTER_Y_OF,background);
        btnCopy.OnClicked = OnClickCopy;

        txtName.size.x = iGUIUtility.WidthBetween2Objects(icon, btnCopy);
        txtName.size.y = 18;
        txtName.fontSize = 16;
        txtName.SetText(iconName, false);
        txtName.RelativePosition(iRelativePosition.LEFT_IN    , background, 150);
        txtName.RelativePosition(iRelativePosition.CENTER_Y_OF, btnCopy);
    }


    public override void Render()
    {
        base.Render();
        background.size = size;
        background.position = position;
        btnCopy.ReRelative();


        background.Render();
        icon.Render();
        btnCopy.Render();
        txtName.Render();
    }


    public override void ReRelative()
    {
        base.ReRelative();
        background.ReRelative();
        icon.ReRelative();
        btnCopy.ReRelative();
        txtName.ReRelative();
    }


    private void OnClickCopy(iObject sender)
    {
        EditorGUIUtility.systemCopyBuffer = iconName;
        Debug.Log("Copied: " + iconName);
    }
}
