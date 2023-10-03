using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class WallEnableCollider : MonoBehaviour
{
    public float speed;
    public Animator animator;
    public Animator wallShadowRandom;
    public List<BoxCollider> colliders = new List<BoxCollider>();
    // Start is called before the first frame update
    void Awake()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }
    public void Start()
    {
        speed = FlexRunManager.Instance.currentSpeed;
        animator.speed = speed;
        if(wallShadowRandom)
        {
            //var state = wallShadowRandom.GetCurrentAnimatorStateInfo(0);
            int frameRandom = Random.Range(0, 300);
            //wallShadowRandom.Play(state.shortNameHash, 0, frameRandom);
            wallShadowRandom.GetComponentInChildren<Animator>().Play("Yoga", 0, frameRandom / FlexRunManager.Instance.frameMulity);
        }
    }
    public void SentEndableAllCollider()
    {
        if (colliders.Count <= 0) {
            var a = gameObject.GetComponentsInChildren<BoxCollider>();
            colliders = a.ToList();
        }
        foreach (var box in colliders) 
        {
            box.enabled = true;
        }
    }
    public void SentDisableAllCollider()
    {
        if (colliders.Count <= 0)
        { 
            var a = gameObject.GetComponentsInChildren<BoxCollider>();
            colliders = a.ToList();
        }
        foreach (var box in colliders)
        {
            box.enabled = false;
        }
    }
    public void DestoryObj()
    {
        FlexRunManager.Instance.CheckPoint();
        Destroy(gameObject);
    }
}
