using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using iBoost;

public class iBoostMenu : MonoBehaviour
{
    // Anchors
	[MenuItem("Innova Engine/iBoost/GUI/Anchor/Anchors to Corners %[")]
	private static void AnchorsToCorners()
    {
        if( !IsUIOnly(Selection.transforms) ) return;

		foreach(Transform transform in Selection.transforms)
        {
            var rect = transform as RectTransform;
            var parent = Selection.activeTransform.parent as RectTransform;

			if(rect == null || parent == null) return;

            float minX = rect.anchorMin.x + rect.offsetMin.x / parent.rect.width;
            float minY = rect.anchorMin.y + rect.offsetMin.y / parent.rect.height;
            float maxX = rect.anchorMax.x + rect.offsetMax.x / parent.rect.width;
            float maxY = rect.anchorMax.y + rect.offsetMax.y / parent.rect.height;
			
            var newAnchorsMin = new Vector2(minX, minY);
            var newAnchorsMax = new Vector2(maxX, maxY);

			rect.anchorMin = newAnchorsMin;
			rect.anchorMax = newAnchorsMax;
			rect.offsetMin = rect.offsetMax = new Vector2(0, 0);
		}
	}

	[MenuItem("Innova Engine/iBoost/GUI/Anchor/Corners to Anchors %]")]
	private static void CornersToAnchors()
    {
        if( !IsUIOnly(Selection.transforms) ) return;

		foreach(Transform transform in Selection.transforms)
        {
            var rect = transform as RectTransform;

			if(rect == null) return;

			rect.offsetMin = rect.offsetMax = new Vector2(0, 0);
		}
	}

    // Align
	[MenuItem("Innova Engine/iBoost/GUI/Align/Space Between Horizontal")]
    private static void SpaceBetweenHorizontal()
    {
        if(Selection.transforms.Length == 0) return;
        if(!IsSameType(Selection.transforms)) return;

        var list_transform = new List<Transform>(Selection.transforms);

        list_transform = list_transform.OrderBy( o => o.position.x ).ToList();

        int last_index = list_transform.Count - 1;

        bool isUI = IsUI(list_transform[0]);

        float minX = isUI ? list_transform[0].localPosition.x          : list_transform[0].position.x;
        float maxX = isUI ? list_transform[last_index].localPosition.x : list_transform[last_index].position.x;

        Transform object_first = list_transform[0];
        Transform object_last  = list_transform[last_index];

        float delta = maxX - minX;
        float space = delta / (last_index);

        for(int i = 0; i < last_index; ++i)
        {
            var pos = isUI ? list_transform[i].localPosition : list_transform[i].position;
            pos.x = minX + (i * space);

            if(isUI)
            {
                list_transform[i].localPosition = pos;
            }
            else
            {
                list_transform[i].position = pos;
            }
        }
    }

	[MenuItem("Innova Engine/iBoost/GUI/Align/Space Between Vertical")]
    private static void SpaceBetweenVertical()
    {
        if(Selection.transforms.Length == 0) return;
        if(!IsSameType(Selection.transforms)) return;

        var list_transform = new List<Transform>(Selection.transforms);

        list_transform = list_transform.OrderBy( o => o.position.y ).ToList();

        int last_index = list_transform.Count - 1;

        bool isUI = IsUI(list_transform[0]);

        float minY = isUI ? list_transform[0].localPosition.y          : list_transform[0].position.y;
        float maxY = isUI ? list_transform[last_index].localPosition.y : list_transform[last_index].position.y;

        Transform object_first = list_transform[0];
        Transform object_last  = list_transform[last_index];

        float delta = maxY - minY;
        float space = delta / (last_index);

        for(int i = 0; i < last_index; ++i)
        {
            var pos = isUI ? list_transform[i].localPosition : list_transform[i].position;
            pos.y = minY + (i * space);

            if(isUI)
            {
                list_transform[i].localPosition = pos;
            }
            else
            {
                list_transform[i].position = pos;
            }
        }
    }

	[MenuItem("Innova Engine/iBoost/GUI/Align/Sync Horizontal")]
    private static void SyncHorizontal()
    {
        if(Selection.transforms.Length == 0 || Selection.activeTransform == null) return;
        if(!IsSameType(Selection.transforms)) return;

        var list_transform = new List<Transform>(Selection.transforms);

        bool isUI = IsUI(list_transform[0]);
        float posY = isUI ? Selection.activeTransform.localPosition.y : Selection.activeTransform.position.y;

        foreach(var t in list_transform)
        {
            Vector3 pos = isUI ? t.localPosition : t.position;
            pos.y = posY;

            if(isUI)
            {
                t.localPosition = pos;
            }
            else
            {
                t.position = pos;
            }
        }
    }
    
	[MenuItem("Innova Engine/iBoost/GUI/Align/Sync Vertical")]
    private static void SyncVertical()
    {
        if(Selection.transforms.Length == 0 || Selection.activeTransform == null) return;
        if(!IsSameType(Selection.transforms)) return;

        var list_transform = new List<Transform>(Selection.transforms);

        bool isUI = IsUI(list_transform[0]);
        float posX = isUI ? Selection.activeTransform.localPosition.x : Selection.activeTransform.position.x;

        foreach(var t in list_transform)
        {
            Vector3 pos = isUI ? t.localPosition : t.position;
            pos.x = posX;

            if(isUI)
                t.localPosition = pos;
            else
                t.position = pos;
        }
    }

	[MenuItem("Innova Engine/iBoost/GUI/Format/Round Transform")]
    private static void RoundPosition()
    {
        if(Selection.transforms.Length == 0) return;

        var list_transform = new List<Transform>(Selection.transforms);
        foreach(var t in list_transform)
        {
            bool isUI = IsUI(t);

            if(isUI)
            {
                RectTransform rect = t.GetComponent<RectTransform>();
                rect.offsetMax        = iMath.RoundVector2(rect.offsetMax);
                rect.offsetMin        = iMath.RoundVector2(rect.offsetMin);
                rect.anchoredPosition = iMath.RoundVector3(rect.anchoredPosition);
                rect.localScale       = iMath.RoundVector3(rect.localScale);
                rect.rotation         = Quaternion.Euler(iMath.RoundVector3(t.rotation.eulerAngles));
            }
            else
            {
                t.position   = iMath.RoundVector3(t.position);
                t.localScale = iMath.RoundVector3(t.localScale);
                t.rotation   = Quaternion.Euler(iMath.RoundVector3(t.rotation.eulerAngles));
            }
        }
    }

    // Scale
	[MenuItem("Innova Engine/iBoost/GUI/Scale/Linear Size Horizontal (UI)")]
    private static void LinearSizeHorizontal()
    {
        if(Selection.transforms.Length == 0) return;
        if(!IsSameType(Selection.transforms)) return;

        var list_transform = new List<Transform>(Selection.transforms);

        list_transform = list_transform.OrderBy( o => o.position.x ).ToList();

        int last_index = list_transform.Count - 1;

        bool isUI = IsUI(list_transform[0]);
        if(!isUI) return;

        RectTransform object_first = list_transform[0].GetComponent<RectTransform>();
        RectTransform object_last  = list_transform[last_index].GetComponent<RectTransform>();

        Vector2 startPos = object_first.anchoredPosition;
        float halfSize = object_first.sizeDelta.x / 2f;

        float minX = object_first.anchoredPosition.x - halfSize;
        float maxX = object_last.anchoredPosition.x + object_last.sizeDelta.x / 2f;


        float delta = maxX - minX;
        float space = delta / (last_index + 1);

        for(int i = 0; i < last_index + 1; ++i)
        {
            var rect = list_transform[i].GetComponent<RectTransform>();
            var size = rect.sizeDelta;
            size.x = space;
            rect.sizeDelta = size;

            Vector2 pos = rect.anchoredPosition;
            pos.x = startPos.x - halfSize + (space / 2) + (i * space);
            rect.anchoredPosition = pos;
        }
    }

    // ============== Other ================= //
    private static bool IsUI(Transform transform)
    {
        return transform.GetComponent<RectTransform>() != null;
    }

    private static bool IsSameType(Transform[] list)
    {
        if(list == null) return false;
        if(list.Length == 0) return false;

        bool first = IsUI(list[0]);

        foreach(var t in list)
        {
            if( first != IsUI(t) )
            {
                return false;
            }
        }

        return true;
    }

    private static bool IsUIOnly(Transform[] list)
    {
        if(list == null) return false;
        if(list.Length == 0) return false;

        foreach(var t in list)
        {
            if(!IsUI(t)) return false;
        }

        return true;
    }
}
