using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullScreenImage : MonoBehaviour
{
    private RectTransform rectTransform;
    private Canvas canvas;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        SetUIToFullScreen();
    }

    private void SetUIToFullScreen()
    {
        canvas = FindObjectOfType<Canvas>();
        
        rectTransform.sizeDelta = canvas.GetComponent<RectTransform>().sizeDelta;
    }
}
