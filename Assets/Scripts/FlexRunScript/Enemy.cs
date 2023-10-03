using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
//using static UnityEngine.RuleTile.TilingRuleOutput;

public class Enemy : MonoBehaviour
{
    public Transform spawn;
    public Transform target;
    //public Sprite[] allSprite;
    //public GameObject player;
    public AnimationCurve moveset;
    public AnimationCurve scaleRate;
    public float speedObj=0.1f;
    public float speedObjScale = 0.1f;
    public float t;
    public bool isChecked;
    private void Start()
    {
        //player = Player.instance.playerGameObject;
        
        //StartCoroutine(Example());
    }
    private void Update()
    {
        if(t < 1)
            t += Time.deltaTime * speedObj;
        transform.position = Vector3.Lerp(spawn.position, target.position, moveset.Evaluate(t));
        //if (Vector2.Distance(transform.position, target.position) <= 1)
        //{
        //    gameObject.SetActive(false);
        //}
        transform.localScale = Vector3.Lerp(transform.localScale, target.localScale, scaleRate.Evaluate(t));

    }

}
