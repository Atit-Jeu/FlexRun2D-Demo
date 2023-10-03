using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using InnovaFramework.iGUI;

public partial class BuiltInUnityIcon: iWindow
{
    public static BuiltInUnityIcon window;

    private iInputField inputSearch;
    private iBox boxBackground;
    private iScrollView scvContainer;
    private iLabel labelFound;
    private iButton btnRefresh;


    [MenuItem("iGUI/BuiltIn Unity Icon", false, 1)]
    public static void OpenWindow()
    {
        window = GetWindow<BuiltInUnityIcon>();
        window.rect = new Rect(0, 0, 600, 1000);
        window.titleContent = new GUIContent("BuiltIn Unity Icon");
        window.maxSize = window.rect.size;
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

    // Initialize function
    protected override void OnAfterInitializedUI()
    {
        OnBegin();
    }

    protected override void OnInitializeUI()
    {
        inputSearch = new iInputField(iInputType.STRING);
        boxBackground = new iBox();
        scvContainer = new iScrollView();
        labelFound = new iLabel();
        btnRefresh = new iButton();

        inputSearch.text = "Search";
        inputSearch.size.x = rect.width.space();
        inputSearch.RelativePosition(iRelativePosition.LEFT_IN, rect);
        inputSearch.RelativePosition(iRelativePosition.TOP_IN , rect);

        labelFound.size.x = 150;
        labelFound.size.y = 16;
        labelFound.text = "Icons: 0";
        labelFound.RelativePosition(iRelativePosition.BOTTOM_OF, inputSearch);
        labelFound.RelativePosition(iRelativePosition.LEFT_IN, rect);

        btnRefresh.text = "Refresh";
        btnRefresh.RelativePosition(iRelativePosition.RIGHT_IN, rect);
        btnRefresh.RelativePosition(iRelativePosition.BOTTOM_OF, inputSearch);

        boxBackground.size.y = iGUIUtility.HeightBetween2Objects(labelFound, rect.height);
        boxBackground.size.x = rect.width.space();
        boxBackground.RelativePosition(iRelativePosition.BOTTOM_OF, labelFound);
        boxBackground.RelativePosition(iRelativePosition.LEFT_IN, rect);

        scvContainer.direction = iScrollViewDirection.VERTICAL;
        scvContainer.autoSizeMode = iScrollViewAutoSize.HORIZONTAL;
        scvContainer.padding = new iPadding(0, 0, 0, 0, 4);
        scvContainer.size = boxBackground.size.space();
        scvContainer.RelativePosition(iRelativePosition.CENTER_X_OF, boxBackground);
        scvContainer.RelativePosition(iRelativePosition.CENTER_Y_OF, boxBackground);

        inputSearch.OnChanged = OnSearchChange;
        btnRefresh.OnClicked = OnRefreshIcon;

        AddChild(inputSearch);
        AddChild(btnRefresh);
        AddChild(labelFound);
        AddChild(boxBackground);
        AddChild(scvContainer);
    }
}
