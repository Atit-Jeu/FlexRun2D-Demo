using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollVertical : MonoBehaviour
{
    public static int   GAB   = 100;
    public static float SPEED = 800f;
    public static bool  IsDragging;

    public Canvas     canvas;
    public GameObject prefab;

    public bool isSnaping = false;

    private int                 targetIndex        = 2;
    public  Vector2             containSize        = Vector2.zero;
    public  Vector2             startPosition      = Vector2.zero;
    private Vector2             startMousePosition = Vector2.zero;
    public  List<RectTransform> cards              = new List<RectTransform>();


    public GameObject CurrentObject
    {
        get
        {
            return cards[targetIndex].gameObject;
        }
    }


    void Start()
    {
        canvas        = GameObject.FindObjectOfType<Canvas>();
        containSize   = canvas.GetComponent<RectTransform>().sizeDelta;
        startPosition = -(targetIndex * containSize);

        GenerateCard(5);
    }

    
    void Update()
    {
        SnapPosition();
    }


    // =========== Method ========== //
    public void GenerateCard(int count)
    {
        for(int i = 0; i < count; i++)
        {
            Vector2 position = startPosition;
            if (cards.Count > 0)
            {
                RectTransform lastCards = cards[cards.Count - 1];
                position = lastCards.anchoredPosition + lastCards.sizeDelta;
            }

            GameObject    createdCards = Instantiate(prefab, this.transform);
            RectTransform rectCards    = createdCards.GetComponent<RectTransform>();

            rectCards.sizeDelta        = containSize;
            rectCards.anchoredPosition = position;

            createdCards.GetComponent<Image>().color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1);

            Vector3 vec3   = createdCards.transform.localPosition;
                    vec3.x = 0;

            createdCards.transform.localPosition = vec3;

            cards.Add(rectCards);
        }
    }


    public void SnapPosition()
    {
        if (!isSnaping) return;

        bool isFinish = true;
        for(int i = 0; i < cards.Count; i++)
        {
            float targetY            = containSize.y * i + startPosition.y;
            Vector2 targetPosition   = cards[i].anchoredPosition;
                    targetPosition.y = targetY;

            cards[i].anchoredPosition = Vector2.Lerp(cards[i].anchoredPosition, targetPosition, 0.1f);

            if (Mathf.Abs(cards[i].anchoredPosition.y - targetPosition.y) > 5f)
            {
                isFinish = false;
            }
            else
            {
                cards[i].anchoredPosition = targetPosition;
            }
        }

        if (isFinish)
        {
            isSnaping = false;
        }
    }


    public void ScrollDown()
    {
        var tempRect = cards[0];
        cards.RemoveAt(0);

        Vector2 vec2   = tempRect.anchoredPosition;
                vec2.y = containSize.y * cards.Count + startPosition.y;

        tempRect.anchoredPosition = vec2;

        cards.Add(tempRect);
        isSnaping = true;
    }


    public void ScrollUp()
    {
        var tempRect = cards[cards.Count - 1];
        cards.RemoveAt(cards.Count - 1);

        Vector2 vec2   = tempRect.anchoredPosition;
                vec2.y = startPosition.y;

        tempRect.anchoredPosition = vec2;

        cards.Insert(0, tempRect);
        isSnaping = true;
    }


    // =========== Interface ========== //
    /*public void OnDrag(PointerEventData eventData)
    {
        if (isSnaping) return;

        float distance = Mathf.Abs(eventData.position.y - startMousePosition.y);
        if (distance > GAB)
        {
            IsDragging = true;
        }

        if (IsDragging)
        {
            for(int i = 0; i < cards.Count; i++)
            {
                cards[i].anchoredPosition += new Vector2(0, eventData.delta.y * SPEED * Time.deltaTime);
            }
        }
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if (isSnaping) return;

        startMousePosition = eventData.position;
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        isSnaping  = true;

        if (IsDragging)
        {
            float diff = cards[0].anchoredPosition.y - startPosition.y;
            float gap  = containSize.y / 3f;

            if (diff > gap)
            {
                ScrollUp();
            }
            else if (diff < -gap)
            {
                ScrollDown();
            }
        }

        IsDragging = false;
    }*/
}
