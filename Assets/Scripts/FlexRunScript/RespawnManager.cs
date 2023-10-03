using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RespawnManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject obstaclePool;
    [SerializeField] GameObject dummyWallPrefab;
    [SerializeField] GameObject dummyObstacle;
    [SerializeField] public GameObject dummyShadow;
    [SerializeField] GameObject cloneObj;
    [SerializeField] Transform RespawnPrefab;
    [SerializeField] public int countShadowForShow;
    [SerializeField] List<WallEnableCollider> obstacleList = new List<WallEnableCollider>();
    private void Start()
    {
        var obs = obstaclePool.GetComponentsInChildren<WallEnableCollider>();
        obstacleList = obs.ToList();
        foreach (var item in obstacleList)
        {
            item.gameObject.SetActive(false);
        }
        Application.targetFrameRate = 60;

    }
    GameObject shadowObj;
    public List<Animator> shadows = new List<Animator>();
    public float speedInvoke;
    public void StartWave()
    {

        InvokeRepeating("RespawnWave",0, speedInvoke);
      
    }
    bool firstRun;
    public void RespawnWave ()
    {
            if (!dummyObstacle)
            {
                int randomCount = Random.Range(0, obstacleList.Count);
                cloneObj = Instantiate(obstacleList[randomCount].gameObject, RespawnPrefab);
                cloneObj.name = FlexRunManager.Instance.currentPatternCount + " :" + cloneObj.name;
                cloneObj.gameObject.transform.localScale = Vector3.zero;
                //Debug.Log("obj:" + cloneObj.name);
            }
            else
            {
                cloneObj = Instantiate(dummyObstacle, RespawnPrefab);
                cloneObj.gameObject.transform.localScale = Vector3.zero;
                //Debug.Log("Dummy:" + cloneObj.name);
            }
        cloneObj.SetActive(true);
        var shadowAni = cloneObj.GetComponentInChildren<Animator>();
        shadows.Add(shadowAni);

        if (!firstRun)
        {
            firstRun = true;
            dummyShadow.gameObject.SetActive(true);
            dummyShadow.transform.SetParent(shadows[countShadowForShow].transform, false);
            dummyShadow.GetComponentInChildren<Animator>().Play("Yoga", 0, FlexRunManager.Instance.keepFrame / (float)FlexRunManager.Instance.frameMulity);
        }

    }
    public void ShadowReplace()
    {

        countShadowForShow++;
        dummyShadow.transform.SetParent(shadows[countShadowForShow].transform, false);
        //FlexRunManager.Instance.ges.ShadowReplace();
        //dummyShadow.GetComponentInChildren<Animator>().Play("Yoga", 0,normalTime);

    }


}
