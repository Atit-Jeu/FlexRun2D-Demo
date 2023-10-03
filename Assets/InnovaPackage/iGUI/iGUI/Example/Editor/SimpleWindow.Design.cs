using System.Collections;
using System.Collections.Generic;
using InnovaFramework.iGUI;
using UnityEditor;
using UnityEngine;

public partial class SimpleWindow : iWindow
{
    public static SimpleWindow window;

    private static Rect windowRect = new Rect(0, 0, 900, 300);

    private iTab         tabs        = new iTab();
    private iBox         panel       = new iBox();
    private iProgressBar progressBar = new iProgressBar();

    [MenuItem("iGUI/Example")]
    public static void OpenWindow()
    {
        window = GetWindow<SimpleWindow>();
        window.titleContent = new GUIContent("Simple Window");
        window.maxSize = windowRect.size;
        window.minSize = window.maxSize;
    }


    private void OnEnable()
    {
        if(EditorApplication.isCompiling) return;
    }


    private void OnGUI()
    {
        // Use this for fix size
        if(window != null)
        {
            window.minSize = window.maxSize;
        }

        base.Render();
    }


    protected override void OnInitializeUI()
    {
        tabs.SetHeaders
        (
            iGUIUtility.CreateGUIContent(" Common"      , "PreMatCube@2x"           ),
            iGUIUtility.CreateGUIContent(" Input Field" , "InputField Icon"         ),
            iGUIUtility.CreateGUIContent(" Foldout"     , "FolderOpened On Icon"    ),
            iGUIUtility.CreateGUIContent(" Array"       , "VerticalLayoutGroup Icon"),
            iGUIUtility.CreateGUIContent(" Drag & Drop" , "ViewToolMove"),
            iGUIUtility.CreateGUIContent(" Image"       , "RawImage Icon"),
            iGUIUtility.CreateGUIContent(" Zoom Scale"  , "ViewToolZoom On"),
            iGUIUtility.CreateGUIContent(" Layout"      , "FreeformLayoutGroup Icon")
        );

        tabs.size.x  = windowRect.width.space();
        tabs.size.y  = 24;
        tabs.RelativePosition(iRelativePosition.LEFT_IN, windowRect);
        tabs.RelativePosition(iRelativePosition.TOP_IN, windowRect);
        tabs.OnChange = OnTabChange;

        panel.size.x = windowRect.width.space();
        panel.size.y = windowRect.height.space() - tabs.height;
        panel.RelativePosition(iRelativePosition.LEFT_IN, windowRect);
        panel.RelativePosition(iRelativePosition.BOTTOM_OF, tabs);

        AddChild(tabs);
        AddChild(panel);

        RenderPageCommon();
        RenderPageInputField();
        RenderPageFoldout();
        RenderPageArray();
        RenderPageDragNDrop();
        RenderPageImage();
        RenderPageZoom();
        RenderPageLayout();
    }
}
