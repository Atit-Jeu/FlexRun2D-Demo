using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommentMenuController : MonoBehaviour
{
    private Canvas canvas;
    private Vector2 containSize = Vector2.zero;
    private Vector2 startPos = Vector2.zero;
    public Vector2 opentargetPos = Vector2.zero;
    public Vector2 closetargetPos = Vector2.zero;

    public GameObject commentPanel;
    public RectTransform moveTransfrom;
    public float moveSpeed = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        canvas = FindObjectOfType<Canvas>();
        containSize = canvas.GetComponent<RectTransform>().sizeDelta;
        
        OpenCommentPanel();
    }


    public void OpenCommentPanel()
    {
        commentPanel.SetActive(true);
        startPos = new Vector2(0, -containSize.y);
        moveTransfrom.anchoredPosition = startPos;
        StartCoroutine(AutoSmoothMove(startPos, opentargetPos, moveSpeed, true));
    }
    public void CloseCommentPanel()
    {
        startPos = new Vector2(0, moveTransfrom.anchoredPosition.y);
        StartCoroutine(AutoSmoothMove(startPos, closetargetPos, moveSpeed, false));
    }

    IEnumerator AutoSmoothMove(Vector2 startpos, Vector2 endpos, float seconds, bool OpenOrClos)
    {
        float t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            moveTransfrom.anchoredPosition = Vector2.Lerp(startpos, endpos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
        moveTransfrom.anchoredPosition = endpos;
        commentPanel.SetActive(OpenOrClos);
    }
}
