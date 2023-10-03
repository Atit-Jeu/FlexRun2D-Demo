using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewSnap : MonoBehaviour 
{
    private bool isInit = false;
    public PageSwiper swiper;
    public Scrollbar scrollbar;
    float scrollPos = 0;
    float[] pos;
    float distance;

    public List<Transform> Contents;

    private void Awake() {
        pos = new float[Contents.Count];
        distance = 1f / (Contents.Count - 1f);
        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = distance * i;
        }
        scrollbar.value = 1f;
        scrollPos = 1f;
    }

    private void Update() 
    {
#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
            scrollPos = scrollbar.value;
        else
        {
            for (int i = 0; i < pos.Length; i++)
            {
                if (scrollPos < pos[i] + (distance / 2) && scrollPos > pos[i] - (distance / 2))
                {
                    scrollbar.value = Mathf.Lerp(scrollbar.value, pos[i], 0.1f);
                    //swiper.isOpenScrollView = false;
                    //this.gameObject.GetComponent<ScrollRect>().enabled = false;
                }
            }
            
        }

#else
        if (Input.touchCount > 0)
        {
            scrollPos = scrollbar.value;
        }
        else
        {
            for (int i = 0; i < pos.Length; i++)
            {
                if (scrollPos < pos[i] + (distance / 2) && scrollPos > pos[i] - (distance / 2))
                {
                    scrollbar.value = Mathf.Lerp(scrollbar.value, pos[i], 0.1f);
                    this.gameObject.GetComponent<ScrollRect>().enabled = false;
                }
            }
            
        }
#endif
    }

}