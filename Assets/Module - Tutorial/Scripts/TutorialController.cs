using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private ScrollSnapRect scrollSnap;
    [SerializeField] private int current;
    [SerializeField] private int count;
    public List<Sprite> tutorialSprites = new List<Sprite>();
    [Header("GUI")]
    [SerializeField] private GameObject root;
    [SerializeField] private GameObject _tutorial;
    [SerializeField] private GameObject _start_btn;
    [Header("Prefab")]
    [SerializeField] private Image imagePrefab;
    [SerializeField] private Image btnPrefab;
    [SerializeField] private GameObject _perv_btnPrefab;
    [SerializeField] private GameObject _next_btnPrefab;
    [SerializeField] private GameObject _skip_btnPrefab;
    [Header("Sprite")]
    [Tooltip("Sprite for unselected page (optional)")]
    public Sprite unselectedPage;
    [Tooltip("Sprite for selected page (optional)")]
    public Sprite selectedPage;
    public Sprite scrollrectBG;

    #region Test
    /*void Start()
    {
        TestSetup();
    }
    public void TestSetup()
    {
        Setup(TestStart);
    }*/
    public void TestStart()
    {
        Debug.Log("Start game. Game pause = false");
    }
    #endregion
    public void Setup(
        Action startAction
        )
    {
        root.SetActive(true);
        _start_btn.SetActive(false);
        Button startBTN = _start_btn.GetComponent<Button>();
        startBTN.onClick.RemoveAllListeners();
        startBTN.onClick.AddListener(() => startAction?.Invoke());
        startBTN.onClick.AddListener(() => root.SetActive(false));
        _skip_btnPrefab.GetComponent<Button>().onClick.AddListener(() =>
        {
            scrollSnap.SetPageSkipToLast();
        });

        // scrollSnap Setup
        scrollSnap.Setup(
            pageSprite: tutorialSprites,
            imagePrefab: imagePrefab,
            btnPrefab: btnPrefab,
            _perv_btnPrefab: _perv_btnPrefab,
            _next_btnPrefab: _next_btnPrefab,
            unselectedSprite: unselectedPage,
            selectedSprite: selectedPage,
            scrollrectBG: scrollrectBG
            );
        scrollSnap.ScrollSnapRectSetup();
    }

    private void OnEnable()
    {
        _start_btn.SetActive(false);
    }
    private void Update()
    {
        current = scrollSnap._currentPage;
        count = scrollSnap._pageCount;
        //Debug.Log($"{current} {count}");
        if ((current+1) >= count)
        {
            _start_btn.SetActive(true);
            //_skip_btnPrefab.SetActive(false);
        }
        else
        {
            //_skip_btnPrefab.SetActive(true);
        }
    }
}
