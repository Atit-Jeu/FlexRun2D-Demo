using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
 
    void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.tag == "Obstacle")
        {
            FlexRunManager.Instance.Hit();
            //Debug.Log("Obstacle : " + col.name);

        }
        if (col.gameObject.tag == "Panel")
        {
            //FlexRunManager.Instance.CheckPoint();
            //Debug.Log("Panel : " + col.name);
        }
        if (col.gameObject.tag == "Combo")
        {
            //Debug.Log("Combo : " + col.name);
            FlexRunManager.Instance.Combo();
        }
    }
}
