using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using InnovaFramework.iGUI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class iSceneNavigation : iWindow
{
    #region Public Field
    public static iSceneNavigation window;
    #endregion

    private HashSet<string> cachedScenes = new HashSet<string>();
    

    #region MonoBehavior Callback
    [MenuItem("Innova Engine/iBoost/Scene Navagation _1")]
    public static void OpenWindow()
    {
        window = GetWindow<iSceneNavigation>();
        window.titleContent = new GUIContent("Scene Navigation");
        window.rect    = new Rect(0f, 0f, 300, 700);
        window.maxSize = window.rect.size;
        window.minSize = window.maxSize;
    }

    private void OnGUI()
    {
        if(EditorApplication.isCompiling)
        {
            if(window != null)
            {
                window.Close();
                return;
            }
        }

        if(window != null)
        {
            window.minSize = window.minSize;
        }

        base.Render();
    }
    #endregion



    #region Public Method
    #endregion



    #region Private Method
    protected override void OnInitializeUI()
    {
        base.OnInitializeUI();

        iBox        background    = new iBox();
        iBox        backgroundAll = new iBox();
        iScrollView scrollView    = new iScrollView();
        iScrollView scrollView2   = new iScrollView();
        iInputField searchInput   = new iInputField(iInputType.STRING);

        searchInput.size.x = this.rect.width.space();
        searchInput.text = "Search";
        searchInput.RelativePosition(iRelativePosition.TOP_IN     , this);
        searchInput.RelativePosition(iRelativePosition.CENTER_X_OF, this);
        searchInput.OnChanged = (sender) =>
        {
            if (searchInput.stringValue == "")
            {
                scrollView.ShowOnly(null);
                scrollView2.ShowOnly(null);
                return;
            }
            scrollView.ShowOnly(scrollView.children.FindAll( o => o.text.ToLower().Contains(searchInput.stringValue.ToLower()) ).ToArray());
            scrollView2.ShowOnly(scrollView2.children.FindAll( o => o.text.ToLower().Contains(searchInput.stringValue.ToLower()) ).ToArray());
        };

        float height = iGUIUtility.SplitSize(iGUIUtility.HeightBetween2Objects(searchInput, this.rect.height), 2);
        background.size.y = height;
        background.size.x = this.rect.width.space();
        background.RelativePosition(iRelativePosition.BOTTOM_OF  , searchInput);
        background.RelativePosition(iRelativePosition.CENTER_X_OF, this);

        backgroundAll.size.x = this.rect.width.space();
        backgroundAll.size.y = height;
        backgroundAll.RelativePosition(iRelativePosition.BOTTOM_OF  , background);
        backgroundAll.RelativePosition(iRelativePosition.CENTER_X_OF, this);

        scrollView.size = background.size.space();
        scrollView.padding = new iPadding(0, 0, 0, 0, 4f);
        scrollView.autoSizeMode = iScrollViewAutoSize.HORIZONTAL;
        scrollView.RelativePosition(iRelativePosition.CENTER_X_OF, background);
        scrollView.RelativePosition(iRelativePosition.CENTER_Y_OF, background);

        scrollView2.size = backgroundAll.size.space();
        scrollView2.padding = new iPadding(0, 0, 0, 0, 4f);
        scrollView2.autoSizeMode = iScrollViewAutoSize.HORIZONTAL;
        scrollView2.RelativePosition(iRelativePosition.CENTER_X_OF, backgroundAll);
        scrollView2.RelativePosition(iRelativePosition.CENTER_Y_OF, backgroundAll);

        DrawSceneItem(scrollView);
        DrawSceneAll(scrollView2);

        RegisterGUI(searchInput, background, backgroundAll, scrollView, scrollView2);
    }

    private void DrawSceneItem(iScrollView scrollView)
    {
        cachedScenes.Clear();
        for(int i = 0; i < SceneManager.sceneCountInBuildSettings; ++i)
        {
            string scene = SceneUtility.GetScenePathByBuildIndex(i);

            if(!File.Exists(scene))
            {
                continue;
            }

            iSceneNavigationItem sceneItem = new iSceneNavigationItem();
            sceneItem.size = new Vector2(18, 18);
            sceneItem.text = Path.GetFileNameWithoutExtension(scene);
            sceneItem.path = scene;

            cachedScenes.Add(scene);
            scrollView.AddChild(sceneItem);
        }
    }

    private void DrawSceneAll(iScrollView scrollView)
    {
        List<string> sceneGUIDs = AssetDatabase.FindAssets("t:Scene").ToList();
        sceneGUIDs = sceneGUIDs.Select( o => AssetDatabase.GUIDToAssetPath(o)).ToList();
        sceneGUIDs = sceneGUIDs.OrderBy( o => Path.GetFileNameWithoutExtension(o)).ToList();

        foreach (string scene in sceneGUIDs)
        {
            if(!File.Exists(scene))
            {
                continue;
            }

            if (cachedScenes.Contains(scene)) continue;

            iSceneNavigationItem sceneItem = new iSceneNavigationItem();
            sceneItem.size = new Vector2(18, 18);
            sceneItem.text = Path.GetFileNameWithoutExtension(scene);
            sceneItem.path = scene;

            scrollView.AddChild(sceneItem);
        }
    }

    private void RegisterGUI(params iObject[] objects)
    {
        for(int i = 0, j = objects.Length; i < j; ++i)
        {
            this.AddChild(objects[i]);
        }
    }
    #endregion
}
