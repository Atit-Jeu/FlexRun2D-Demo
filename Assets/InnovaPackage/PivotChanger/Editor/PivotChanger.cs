using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InnovaFramework.iGUI;
using UnityEditor;
using iBoost;

internal enum PivotPosition
{
    TOP_TOP_LEFT,
    TOP_TOP_CENTER,
    TOP_TOP_RIGHT,
    TOP_CENTER_LEFT,
    TOP_CENTER_CENTER,
    TOP_CENTER_RIGHT,
    TOP_BOTTOM_LEFT,
    TOP_BOTTOM_CENTER,
    TOP_BOTTOM_RIGHT,

    CENTER_TOP_LEFT,
    CENTER_TOP_CENTER,
    CENTER_TOP_RIGHT,
    CENTER_CENTER_LEFT,
    CENTER_CENTER_CENTER,
    CENTER_CENTER_RIGHT,
    CENTER_BOTTOM_LEFT,
    CENTER_BOTTOM_CENTER,
    CENTER_BOTTOM_RIGHT,

    BOTTOM_TOP_LEFT,
    BOTTOM_TOP_CENTER,
    BOTTOM_TOP_RIGHT,
    BOTTOM_CENTER_LEFT,
    BOTTOM_CENTER_CENTER,
    BOTTOM_CENTER_RIGHT,
    BOTTOM_BOTTOM_LEFT,
    BOTTOM_BOTTOM_CENTER,
    BOTTOM_BOTTOM_RIGHT,

    CUSTOM
}

public class PivotChanger : iWindow
{

    public static PivotChanger window;
    private static Rect windowRect = new Rect(0f, 0f, 600, 176);

    private static iInputField inputObject;
    private static iInputField inputCustomPivotObject;
    private static iSlider sldRotateX;
    private static iSlider sldRotateY;
    private static iSlider sldRotateZ;
    private static iDropDown ddPosition;
    private static iCheckBox cbIncludeMats;
    private static Mesh oldMesh = null;
    private static Mesh workingMesh = null;

    [MenuItem("Innova Engine/Pivot Changer")]
    public static void OpenWindw()
    {
        window = GetWindow<PivotChanger>();
        window.titleContent = new GUIContent("Pivot Changer");
        window.maxSize      = windowRect.size;
        window.minSize      = window.maxSize;
    }

    private void OnGUI()
    {
        if(window != null)
        {
            window.minSize = window.maxSize;
        }

        base.Render();
    }

    private void OnEnable()
    {
        if (EditorApplication.isCompiling) return;

        InitializeUIGenerator();
    }

    private void OnDestroy()
    {
        if (inputObject.objectValue == null) return;
        if (oldMesh == null) return;

        GameObject target = inputObject.objectValue as GameObject;
        target.GetComponent<MeshFilter>().sharedMesh = oldMesh;
    }

    private void InitializeUIGenerator()
    {
        // -- Declared UI --- //
        iButton btn = new iButton();
        iButton btnExport = new iButton();
        ddPosition = new iDropDown();
        sldRotateX = new iSlider(iSliderType.FLOAT);
        sldRotateY = new iSlider(iSliderType.FLOAT);
        sldRotateZ = new iSlider(iSliderType.FLOAT);
        cbIncludeMats = new iCheckBox();

        inputCustomPivotObject = new iInputField(iInputType.OBJECT);
        inputCustomPivotObject.typeObject = typeof(GameObject);

        inputObject = new iInputField(iInputType.OBJECT);
        inputObject.typeObject = typeof(GameObject);

        // --- Positioning --- //
        inputObject.text = "Target";
        inputObject.size.x = windowRect.width - iGUIUtility.spaceX2;
        inputObject.RelativePosition(iRelativePosition.LEFT_IN, windowRect);
        inputObject.RelativePosition(iRelativePosition.TOP_IN, windowRect);
        inputObject.OnChanged = (e) =>
        {
            GameObject target = inputObject.objectValue as GameObject;
            Mesh mesh = target.GetComponent<MeshFilter>().sharedMesh;
            oldMesh = mesh;
            workingMesh = oldMesh;
        };


        ddPosition.size.x = 148;
        ddPosition.RelativePosition(iRelativePosition.BOTTOM_OF, inputObject);
        ddPosition.RelativePosition(iRelativePosition.LEFT_IN, windowRect);
        ddPosition.AddOption("Top Top Left", PivotPosition.TOP_TOP_LEFT);
        ddPosition.AddOption("Top Top Center", PivotPosition.TOP_TOP_CENTER);
        ddPosition.AddOption("Top Top Right", PivotPosition.TOP_TOP_RIGHT);
        ddPosition.AddOption("Top Center Left", PivotPosition.TOP_CENTER_LEFT);
        ddPosition.AddOption("Top Center Center", PivotPosition.TOP_CENTER_CENTER);
        ddPosition.AddOption("Top Center Right", PivotPosition.TOP_CENTER_RIGHT);
        ddPosition.AddOption("Top Bottom Left", PivotPosition.TOP_BOTTOM_LEFT);
        ddPosition.AddOption("Top Bottom Center", PivotPosition.TOP_BOTTOM_CENTER);
        ddPosition.AddOption("Top Bottom Right", PivotPosition.TOP_BOTTOM_RIGHT);
        ddPosition.AddOption("Center Top Left", PivotPosition.CENTER_TOP_LEFT);
        ddPosition.AddOption("Center Top Center", PivotPosition.CENTER_TOP_CENTER);
        ddPosition.AddOption("Center Top Right", PivotPosition.CENTER_TOP_RIGHT);
        ddPosition.AddOption("Center Center Left", PivotPosition.CENTER_CENTER_LEFT);
        ddPosition.AddOption("Center Center Center", PivotPosition.CENTER_CENTER_CENTER);
        ddPosition.AddOption("Center Center Right", PivotPosition.CENTER_CENTER_RIGHT);
        ddPosition.AddOption("Center Bottom Left", PivotPosition.CENTER_BOTTOM_LEFT);
        ddPosition.AddOption("Center Bottom Center", PivotPosition.CENTER_BOTTOM_CENTER);
        ddPosition.AddOption("Center Bottom Right", PivotPosition.CENTER_BOTTOM_RIGHT);
        ddPosition.AddOption("Bottom Top Left", PivotPosition.BOTTOM_TOP_LEFT);
        ddPosition.AddOption("Bottom Top Center", PivotPosition.BOTTOM_TOP_CENTER);
        ddPosition.AddOption("Bottom Top Right", PivotPosition.BOTTOM_TOP_RIGHT);
        ddPosition.AddOption("Bottom Center Left", PivotPosition.BOTTOM_CENTER_LEFT);
        ddPosition.AddOption("Bottom Center Center", PivotPosition.BOTTOM_CENTER_CENTER);
        ddPosition.AddOption("Bottom Center Right", PivotPosition.BOTTOM_CENTER_RIGHT);
        ddPosition.AddOption("Bottom Bottom Left", PivotPosition.BOTTOM_BOTTOM_LEFT);
        ddPosition.AddOption("Bottom Bottom Center", PivotPosition.BOTTOM_BOTTOM_CENTER);
        ddPosition.AddOption("Bottom Bottom Right", PivotPosition.BOTTOM_BOTTOM_RIGHT);
        ddPosition.AddOption("Custom", PivotPosition.CUSTOM);
        ddPosition.OnChanged = (e) =>
        {
            inputCustomPivotObject.enabled = (PivotPosition)ddPosition.selectedObject == PivotPosition.CUSTOM;
        };


        inputCustomPivotObject.enabled = false;
        inputCustomPivotObject.text = "Custom Pivot";
        inputCustomPivotObject.labelSpace = 88;
        inputCustomPivotObject.size.x = windowRect.width - ddPosition.width - iGUIUtility.spaceX3;
        inputCustomPivotObject.RelativePosition(iRelativePosition.RIGHT_OF, ddPosition);
        inputCustomPivotObject.RelativePosition(iRelativePosition.BOTTOM_OF, inputObject);
        inputCustomPivotObject.OnChanged = (e) =>
        {
        };

        sldRotateX.text = "Rotate X";
        sldRotateX.floatMin = -180;
        sldRotateX.floatMax = 180;
        sldRotateX.floatValue = 0;
        sldRotateX.size.x = windowRect.width - iGUIUtility.spaceX2;
        sldRotateX.RelativePosition(iRelativePosition.BOTTOM_OF, ddPosition);
        sldRotateX.RelativePosition(iRelativePosition.LEFT_IN, windowRect);

        sldRotateY.text = "Rotate Y";
        sldRotateY.floatMin = -180;
        sldRotateY.floatMax = 180;
        sldRotateY.floatValue = 0;
        sldRotateY.size.x = windowRect.width - iGUIUtility.spaceX2;
        sldRotateY.RelativePosition(iRelativePosition.BOTTOM_OF, sldRotateX);
        sldRotateY.RelativePosition(iRelativePosition.LEFT_IN, windowRect);

        sldRotateZ.text = "Rotate Z";
        sldRotateZ.floatMin = -180;
        sldRotateZ.floatMax = 180;
        sldRotateZ.floatValue = 0;
        sldRotateZ.size.x = windowRect.width - iGUIUtility.spaceX2;
        sldRotateZ.RelativePosition(iRelativePosition.BOTTOM_OF, sldRotateY);
        sldRotateZ.RelativePosition(iRelativePosition.LEFT_IN, windowRect);

        btn.text = "Submit";
        btn.size.x = windowRect.width - iGUIUtility.spaceX2;
        btn.RelativePosition(iRelativePosition.BOTTOM_OF, sldRotateZ);
        btn.RelativePosition(iRelativePosition.LEFT_IN, windowRect);

        btnExport.text = "Export";
        btnExport.size.x = 128f;
        btnExport.RelativePosition(iRelativePosition.RIGHT_IN, windowRect);
        btnExport.RelativePosition(iRelativePosition.BOTTOM_OF, btn);

        cbIncludeMats.size.x = 128;
        cbIncludeMats.isChecked = true;
        cbIncludeMats.text = "Include Materials";
        cbIncludeMats.RelativePosition(iRelativePosition.LEFT_OF, btnExport);
        cbIncludeMats.RelativePosition(iRelativePosition.BOTTOM_OF, btn);


        // --- Event --- //
        btn.OnClicked = OnChangePosition;

        sldRotateX.OnChanged = OnRotate;
        sldRotateY.OnChanged = sldRotateX.OnChanged;
        sldRotateZ.OnChanged = sldRotateX.OnChanged;

        btnExport.OnClicked = OnExport;


        // --- Render --- //
        AddChild(inputObject);
        AddChild(ddPosition);
        AddChild(btn);
        AddChild(inputCustomPivotObject);
        AddChild(sldRotateX);
        AddChild(sldRotateY);
        AddChild(sldRotateZ);
        AddChild(btnExport);
        AddChild(cbIncludeMats);
    }


    private void OnExport(iObject e)
    {
        if (inputObject.objectValue == null) return;
        GameObject target = inputObject.objectValue as GameObject;
        if (oldMesh == null) return;

        Mesh meshToSave = target.GetComponent<MeshFilter>().sharedMesh;
        if (oldMesh == meshToSave) return;


        string meshAssetPath = EditorUtility.SaveFilePanelInProject(
            "Save mesh asset", 
            "pivot_changed_" + oldMesh.name + ".asset" , 
            "asset", 
            "Please select path to save mesh"
        );

        if (meshAssetPath.Length == 0)
        {
            Log.Error("Canceled");
            return;
        }

        AssetDatabase.CreateAsset(meshToSave, meshAssetPath);

        if (cbIncludeMats.isChecked)
        {
            MeshRenderer renderer = target.GetComponent<MeshRenderer>();
            Material[] mats = renderer.sharedMaterials;

            for(int i = 0; i < mats.Length; i++)
            {
                string name = mats[i].name;
                Material mat = new Material(mats[i]);
                mat.name = name.Replace(" (Instance)", "");
                mats[i] = mat;
                AssetDatabase.AddObjectToAsset(mat, meshAssetPath);
                AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(mat));
            }

            renderer.sharedMaterials = mats;
        }

        oldMesh = meshToSave;
        workingMesh = oldMesh;

        AssetDatabase.Refresh();
    }

    private void OnRotate(iObject e)
    {
        if (inputObject.objectValue == null) return;
        GameObject target = inputObject.objectValue as GameObject;

        Mesh mesh = CloneMesh(workingMesh);
        if (mesh == null) return;

        Mesh newMesh = new Mesh();
        Vector3[] vertices = new Vector3[oldMesh.vertexCount];

        for (int i = 0; i < mesh.vertexCount; i++)
        {
            vertices[i].x = mesh.vertices[i].x;
            vertices[i].y = mesh.vertices[i].y;
            vertices[i].z = mesh.vertices[i].z;

            vertices[i] = Quaternion.Euler(sldRotateX.floatValue, sldRotateY.floatValue, sldRotateZ.floatValue) * vertices[i];
        }

        ApplyMesh(ref newMesh, mesh, vertices);

        target.GetComponent<MeshFilter>().sharedMesh = newMesh;
    }

    private void OnChangePosition(iObject e)
    {
        if (inputObject.objectValue == null) return;

        GameObject target = inputObject.objectValue as GameObject;

        Mesh mesh = CloneMesh(oldMesh);
        if (mesh == null) return;

        Mesh newMesh = new Mesh();
        Vector3[] vertices = new Vector3[oldMesh.vertexCount];
        Vector3[] w_vertices = new Vector3[oldMesh.vertexCount];

        Bounds bounds = mesh.bounds;

        Vector3 targetBounds = new Vector3();

        PivotPosition pp = (PivotPosition)ddPosition.selectedObject;
        switch(pp)
        {
            // TOP
            case PivotPosition.TOP_TOP_LEFT:
                {
                    targetBounds.x = bounds.center.x - bounds.extents.x;
                    targetBounds.y = bounds.center.y + bounds.extents.y;
                    targetBounds.z = bounds.center.z + bounds.extents.z;
                    break;
                }
            case PivotPosition.TOP_TOP_CENTER:
                {
                    targetBounds.x = bounds.center.x;
                    targetBounds.y = bounds.center.y + bounds.extents.y;
                    targetBounds.z = bounds.center.z + bounds.extents.z;
                    break;
                }
            case PivotPosition.TOP_TOP_RIGHT:
                {
                    targetBounds.x = bounds.center.x + bounds.extents.x;
                    targetBounds.y = bounds.center.y + bounds.extents.y;
                    targetBounds.z = bounds.center.z + bounds.extents.z;
                    break;
                }
            case PivotPosition.TOP_CENTER_LEFT:
                {
                    targetBounds.x = bounds.center.x - bounds.extents.x;
                    targetBounds.y = bounds.center.y + bounds.extents.y;
                    targetBounds.z = bounds.center.z;
                    break;
                }
            case PivotPosition.TOP_CENTER_CENTER:
                {
                    targetBounds.x = bounds.center.x;
                    targetBounds.y = bounds.center.y + bounds.extents.y;
                    targetBounds.z = bounds.center.z;
                    break;
                }
            case PivotPosition.TOP_CENTER_RIGHT:
                {
                    targetBounds.x = bounds.center.x + bounds.extents.x;
                    targetBounds.y = bounds.center.y + bounds.extents.y;
                    targetBounds.z = bounds.center.z;
                    break;
                }
            case PivotPosition.TOP_BOTTOM_LEFT:
                {
                    targetBounds.x = bounds.center.x - bounds.extents.x;
                    targetBounds.y = bounds.center.y + bounds.extents.y;
                    targetBounds.z = bounds.center.z - bounds.extents.z;
                    break;
                }
            case PivotPosition.TOP_BOTTOM_CENTER:
                {
                    targetBounds.x = bounds.center.x;
                    targetBounds.y = bounds.center.y + bounds.extents.y;
                    targetBounds.z = bounds.center.z - bounds.extents.z;
                    break;
                }
            case PivotPosition.TOP_BOTTOM_RIGHT:
                {
                    targetBounds.x = bounds.center.x + bounds.extents.x;
                    targetBounds.y = bounds.center.y + bounds.extents.y;
                    targetBounds.z = bounds.center.z - bounds.extents.z;
                    break;
                }
            // CENTER
            case PivotPosition.CENTER_TOP_LEFT:
                {
                    targetBounds.x = bounds.center.x - bounds.extents.x;
                    targetBounds.y = bounds.center.y;
                    targetBounds.z = bounds.center.z + bounds.extents.z;
                    break;
                }
            case PivotPosition.CENTER_TOP_CENTER:
                {
                    targetBounds.x = bounds.center.x;
                    targetBounds.y = bounds.center.y;
                    targetBounds.z = bounds.center.z + bounds.extents.z;
                    break;
                }
            case PivotPosition.CENTER_TOP_RIGHT:
                {
                    targetBounds.x = bounds.center.x + bounds.extents.x;
                    targetBounds.y = bounds.center.y;
                    targetBounds.z = bounds.center.z + bounds.extents.z;
                    break;
                }
            case PivotPosition.CENTER_CENTER_LEFT:
                {
                    targetBounds.x = bounds.center.x - bounds.extents.x;
                    targetBounds.y = bounds.center.y;
                    targetBounds.z = bounds.center.z;
                    break;
                }
            case PivotPosition.CENTER_CENTER_CENTER:
                {
                    targetBounds.x = bounds.center.x;
                    targetBounds.y = bounds.center.y;
                    targetBounds.z = bounds.center.z;
                    break;
                }
            case PivotPosition.CENTER_CENTER_RIGHT:
                {
                    targetBounds.x = bounds.center.x + bounds.extents.x;
                    targetBounds.y = bounds.center.y;
                    targetBounds.z = bounds.center.z;
                    break;
                }
            case PivotPosition.CENTER_BOTTOM_LEFT:
                {
                    targetBounds.x = bounds.center.x - bounds.extents.x;
                    targetBounds.y = bounds.center.y;
                    targetBounds.z = bounds.center.z - bounds.extents.z;
                    break;
                }
            case PivotPosition.CENTER_BOTTOM_CENTER:
                {
                    targetBounds.x = bounds.center.x;
                    targetBounds.y = bounds.center.y;
                    targetBounds.z = bounds.center.z - bounds.extents.z;
                    break;
                }
            case PivotPosition.CENTER_BOTTOM_RIGHT:
                {
                    targetBounds.x = bounds.center.x + bounds.extents.x;
                    targetBounds.y = bounds.center.y;
                    targetBounds.z = bounds.center.z - bounds.extents.z;
                    break;
                }
            // BOTTOM
            case PivotPosition.BOTTOM_TOP_LEFT:
                {
                    targetBounds.x = bounds.center.x - bounds.extents.x;
                    targetBounds.y = bounds.center.y - bounds.extents.y;
                    targetBounds.z = bounds.center.z + bounds.extents.z;
                    break;
                }
            case PivotPosition.BOTTOM_TOP_CENTER:
                {
                    targetBounds.x = bounds.center.x;
                    targetBounds.y = bounds.center.y - bounds.extents.y;
                    targetBounds.z = bounds.center.z + bounds.extents.z;
                    break;
                }
            case PivotPosition.BOTTOM_TOP_RIGHT:
                {
                    targetBounds.x = bounds.center.x + bounds.extents.x;
                    targetBounds.y = bounds.center.y - bounds.extents.y;
                    targetBounds.z = bounds.center.z + bounds.extents.z;
                    break;
                }
            case PivotPosition.BOTTOM_CENTER_LEFT:
                {
                    targetBounds.x = bounds.center.x - bounds.extents.x;
                    targetBounds.y = bounds.center.y - bounds.extents.y;
                    targetBounds.z = bounds.center.z;
                    break;
                }
            case PivotPosition.BOTTOM_CENTER_CENTER:
                {
                    targetBounds.x = bounds.center.x;
                    targetBounds.y = bounds.center.y - bounds.extents.y;
                    targetBounds.z = bounds.center.z;
                    break;
                }
            case PivotPosition.BOTTOM_CENTER_RIGHT:
                {
                    targetBounds.x = bounds.center.x + bounds.extents.x;
                    targetBounds.y = bounds.center.y - bounds.extents.y;
                    targetBounds.z = bounds.center.z;
                    break;
                }
            case PivotPosition.BOTTOM_BOTTOM_LEFT:
                {
                    targetBounds.x = bounds.center.x - bounds.extents.x;
                    targetBounds.y = bounds.center.y - bounds.extents.y;
                    targetBounds.z = bounds.center.z - bounds.extents.z;
                    break;
                }
            case PivotPosition.BOTTOM_BOTTOM_CENTER:
                {
                    targetBounds.x = bounds.center.x;
                    targetBounds.y = bounds.center.y - bounds.extents.y;
                    targetBounds.z = bounds.center.z - bounds.extents.z;
                    break;
                }
            case PivotPosition.BOTTOM_BOTTOM_RIGHT:
                {
                    targetBounds.x = bounds.center.x + bounds.extents.x;
                    targetBounds.y = bounds.center.y - bounds.extents.y;
                    targetBounds.z = bounds.center.z - bounds.extents.z;
                    break;
                }
            case PivotPosition.CUSTOM:
                {
                    if (inputCustomPivotObject.objectValue == null)
                    {
                        return;
                    }

                    GameObject pivot = inputCustomPivotObject.objectValue as GameObject;
                    Vector3 center = target.transform.position + bounds.center;
                    Vector3 diff = pivot.transform.position - center;
                    targetBounds = bounds.center + diff;

                    pivot.transform.position = target.transform.position;
                    break;
                }
        }

        for (int i = 0; i < mesh.vertexCount; i++)
        {
            vertices[i].x = mesh.vertices[i].x;
            vertices[i].y = mesh.vertices[i].y;
            vertices[i].z = mesh.vertices[i].z;

            vertices[i].x -= targetBounds.x;
            vertices[i].y -= targetBounds.y;
            vertices[i].z -= targetBounds.z;

            w_vertices[i] = vertices[i];
            vertices[i] = Quaternion.Euler(sldRotateX.floatValue, sldRotateY.floatValue, sldRotateZ.floatValue) * vertices[i];
        }

        ApplyMesh(ref newMesh, mesh, vertices);

        workingMesh = new Mesh();
        ApplyMesh(ref workingMesh, mesh, w_vertices);

        target.GetComponent<MeshFilter>().sharedMesh = newMesh;
    }

    private void ApplyMesh(ref Mesh newMesh, Mesh mesh, Vector3[] vertices)
    {
        newMesh.vertices = vertices;
        newMesh.normals = mesh.normals;
        newMesh.uv = mesh.uv;
        newMesh.triangles = mesh.triangles;
        newMesh.tangents = mesh.tangents;
        newMesh.RecalculateBounds();
        newMesh.RecalculateNormals();
        newMesh.Optimize();
    }

    private Mesh CloneMesh(Mesh mesh)
    {
        if (mesh == null)
        {
            return null;
        }
        Mesh m = new Mesh();
        m.vertices = mesh.vertices;
        m.normals = mesh.normals;
        m.uv = mesh.uv;
        m.triangles = mesh.triangles;
        m.tangents = mesh.tangents;

        m.RecalculateBounds();
        m.RecalculateNormals();
        m.Optimize();
        return m;
    }
}
