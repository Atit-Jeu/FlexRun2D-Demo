using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PicktoplayController : MonoBehaviour
{
    private Canvas canvas;
    private Vector2 containSize = Vector2.zero;
    private Vector2 startPos = Vector2.zero;
    private Vector2 targetPos = Vector2.zero;

    public GameObject picktoPlayPanel;
    public float moveSpeed = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        canvas = FindObjectOfType<Canvas>();
        containSize = canvas.GetComponent<RectTransform>().sizeDelta;
        startPos = new Vector2(containSize.x, 0);
        targetPos = new Vector2(0, 0);
        this.gameObject.GetComponent<RectTransform>().anchoredPosition = startPos;
        //OpenPicktoPlayPanel();
    }

    
    public void OpenPicktoPlayPanel()
    {
        picktoPlayPanel.SetActive(true);
        StartCoroutine(AutoSmoothMove(startPos, targetPos, moveSpeed, true));
    }
    public void ClosePicktoPlayPanel()
    {
        StartCoroutine(AutoSmoothMove(this.gameObject.GetComponent<RectTransform>().anchoredPosition, startPos, moveSpeed, false));
    }

    IEnumerator AutoSmoothMove(Vector2 startpos, Vector2 endpos, float seconds, bool OpenOrClos)
    {
        float t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            this.gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(startpos, endpos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
        this.gameObject.GetComponent<RectTransform>().anchoredPosition = endpos;
        picktoPlayPanel.SetActive(OpenOrClos);
    }
}
