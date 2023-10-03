using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class PageSwiper : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private Canvas canvas;
    private Vector3 panelLocation;
    public ScrollVertical targetVertical;
    public float percentThreshold = 0.2f;
    public float easing = 0.5f;
    public int totalPagesY = 1;
    private int currentPageY = 1;
    public int totalPagesX = 1;
    private int currentPageX = 1;
    public bool isOpenScrollViewUpDown;
    public bool isOpenScrollViewLeftRight;

    public List<ScrollVertical> scrollVerticalList = new List<ScrollVertical>();

    // Start is called before the first frame update
    void Start()
    {
        canvas = FindObjectOfType<Canvas>();
        panelLocation = transform.position;
        currentPageX = 2;
        targetVertical = scrollVerticalList[currentPageX - 1];
    }
    public void OnDrag(PointerEventData data)
    {
        if (targetVertical.isSnaping) return;

        float differenceY = data.pressPosition.y - data.position.y;
        float differenceX = data.pressPosition.x - data.position.x;
        //Debug.Log("Y: " + differenceY);

        if (isOpenScrollViewUpDown)
        {
            List<RectTransform> listCard = targetVertical.cards;
            
            for(int i = 0; i < listCard.Count; i++)
            {
                
                listCard[i].anchoredPosition += new Vector2(0,data.delta.y);
            }
            
            return;
        }
        else if (isOpenScrollViewLeftRight)
        {
            transform.position = panelLocation - new Vector3(differenceX, 0, 0);
            return;
        }
        if (Mathf.Abs(differenceY) > 30f && !isOpenScrollViewLeftRight && !isOpenScrollViewUpDown)
        {
            isOpenScrollViewUpDown = true;
        }
        else if (Mathf.Abs(differenceX) > 30f && !isOpenScrollViewLeftRight && !isOpenScrollViewUpDown)
        {
            if (currentPageX == 1 && differenceX < 0f)
            {
                return;
            }
            else if (currentPageX == totalPagesX && differenceX > 0f)
            {
                return;
            }
            isOpenScrollViewLeftRight = true;
        }
        
        Debug.Log("X: " + differenceX);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (targetVertical.isSnaping) return;
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        targetVertical.isSnaping = true;

        if (isOpenScrollViewUpDown)
        {
            float diff = targetVertical.cards[0].anchoredPosition.y - targetVertical.startPosition.y;
            float gap = targetVertical.containSize.y / 3f;

            if (diff > gap)
            {
                targetVertical.ScrollUp();
            }
            else if (diff < -gap)
            {
                targetVertical.ScrollDown();
            }
        }

        isOpenScrollViewUpDown = false;
    }
    public void OnEndDrag(PointerEventData data)
    {
        if (isOpenScrollViewUpDown)
        {
            /*float percentage = (data.pressPosition.y - data.position.y) / Screen.height;
            Debug.Log("Percentage : " + percentage);
            //Debug.Log(data.delta.y);
            isOpenScrollViewUpDown = false;
            targetVertical.isSnaping = true;
            targetVertical.SnapPosition();*/
            
            /*if (Mathf.Abs(percentage) >= percentThreshold)
            {
                Vector3 newLocation = panelLocation;
                if (percentage > 0 && currentPageY <= totalPagesY)
                {
                    Debug.Log("1");
                    currentPageY--;
                    newLocation -= new Vector3(0, Screen.height, 0);
                }
                else if (percentage < 0 && currentPageY > 0)
                {
                    Debug.Log("2");
                    currentPageY++;
                    newLocation -= new Vector3(0, -Screen.height, 0);
                }
                StartCoroutine(SmoothMove(transform.position, newLocation, easing));
                panelLocation = newLocation;
            }
            else
            {
                StartCoroutine(SmoothMove(transform.position, panelLocation, easing));
            }*/
        }
        else if (isOpenScrollViewLeftRight)
        {
            float percentage = (data.pressPosition.x - data.position.x) / Screen.width;
            if (Mathf.Abs(percentage) >= percentThreshold)
            {
                Vector3 newLocation = panelLocation;
                if (percentage > 0 && currentPageX < totalPagesX)
                {
                    currentPageX++;
                    newLocation += new Vector3(-Screen.width, 0, 0);
                }
                else if (percentage < 0 && currentPageX > 1)
                {
                    currentPageX--;
                    newLocation += new Vector3(Screen.width, 0, 0);
                }
                StartCoroutine(SmoothMove(transform.position, newLocation, easing));
                panelLocation = newLocation;
            }
            else
            {
                StartCoroutine(SmoothMove(transform.position, panelLocation, easing));
            }
        }
        
    }
    public void AutoMoveLeftPanel()
    {
        float canvasXSize = canvas.GetComponent<RectTransform>().sizeDelta.x;
        RectTransform thisGameObject = this.gameObject.GetComponent<RectTransform>();
        Vector2 targetPos = new Vector2(canvasXSize,0);
        //thisGameObject.anchoredPosition = targetPos;
        StartCoroutine(AutoSmoothMove(thisGameObject.anchoredPosition, targetPos, easing));
        
    }

    IEnumerator AutoSmoothMove(Vector2 startpos, Vector2 endpos, float seconds)
    {
        float t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            this.gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(startpos, endpos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
        panelLocation = transform.position;
        currentPageX = 1;
        targetVertical = scrollVerticalList[currentPageX - 1];
    }
    IEnumerator SmoothMove(Vector3 startpos, Vector3 endpos, float seconds)
    {
        float t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            transform.position = Vector3.Lerp(startpos, endpos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
        isOpenScrollViewUpDown = false;
        isOpenScrollViewLeftRight = false;
        targetVertical = scrollVerticalList[currentPageX - 1];
    }
}
