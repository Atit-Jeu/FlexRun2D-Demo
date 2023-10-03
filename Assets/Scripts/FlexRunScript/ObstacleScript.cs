using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Obstacle
{
    public List<int> obstaclePointList;
    public List<Sprite> spritePoints;
    public float currentSpeed;
}


public class ObstacleScript : MonoBehaviour
{
    public Obstacle obstacle;
    [SerializeField] bool isRun;
    // Start is called before the first frame update
    public void SetupObstacle(Obstacle obstacleData)
    {
        this.obstacle = obstacleData;
        gameObject.SetActive(true);
        isRun = true;
        StartCoroutine(ABC());
    }
    public void AfterCheckPoint ()
    {
        //isRun = false;
        //Debug.Log("Destory Obj");
        Destroy(gameObject,0f);
    }
    public void SetObstacleToPlayer()
    {
        //return this.obstacle;
    }
    private void FixedUpdate()
    {
        //transform.position += new Vector3(0, 0, obstacle.currentSpeed);
    }
    public IEnumerator ABC()
    {
     
        while (isRun) {
            transform.position += new Vector3(0, obstacle.currentSpeed, obstacle.currentSpeed);
            yield return new WaitForFixedUpdate();
        }
        //Debug.Log("StopRun");
        //return null;
    }
}
