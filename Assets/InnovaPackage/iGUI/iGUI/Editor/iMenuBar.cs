using System.Collections;
using System.Collections.Generic;
using InnovaFramework.iGUI;
using UnityEngine;

public class iMenuBar : iObject
{
    private iBox backgroundMenu;

    public iMenuBar(Rect windowRect)
    {
        backgroundMenu = new iBox();
    }


    public override void Start()
    {
    }


    public override void Render()
    {
        base.Render();
        UpdatePosition();
    }


    private void UpdatePosition()
    {

    }
}
