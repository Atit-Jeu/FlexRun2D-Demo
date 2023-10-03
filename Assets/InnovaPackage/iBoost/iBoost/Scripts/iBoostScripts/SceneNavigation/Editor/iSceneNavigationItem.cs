using System.Collections;
using System.Collections.Generic;
using InnovaFramework.iGUI;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class iSceneNavigationItem : iObject
{
    public string path;
    private iButton button;
    private iImageBox icon;
    private iLabel label;

    private void Init()
    {
        button = new iButton();
        button.OnClicked = (sender) => 
        {
            if(EditorSceneManager.SaveModifiedScenesIfUserWantsTo(new Scene[] {SceneManager.GetActiveScene()} ))
            {
                EditorSceneManager.OpenScene(path);
                if(iSceneNavigation.window != null)
                {
                    iSceneNavigation.window.Close();
                }
            }
        };

        icon = new iImageBox();
        icon.size = new Vector2(16, 16);
        icon.style = new GUIStyle();

        icon.SetImage( iGUIUtility.LoadBuiltInIcon("SceneAsset Icon"));

        label = new iLabel();
    }


    private void Update()
    {
        button.size = this.size;
        button.size.x -= 16 + 8;
        button.position = this.position + new Vector2(16 + 8, 0);

        icon.position = this.position + new Vector2(2, 0);
        icon.RelativePosition(iRelativePosition.CENTER_Y_OF, button);

        label.SetText(this.text);
        label.RelativePosition(iRelativePosition.CENTER_Y_OF, button);
        label.RelativePosition(iRelativePosition.LEFT_IN, button);
        label.position.y -= 0.1f;
    }

    private void Draw()
    {
        button.Render();
        icon.Render();
        label.Render();
    }


    public override void Render()
    {
        if(button == null)
        {
            Init();
        }

        Update();
        Draw(); 
    }
}
