using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GesturesDetect : MonoBehaviour
{
    private Touch touch;
    //private Vector2 beginTouchPosition, endTouchPosition;
    public bool TestTouch;
    //public float calPercent;

    //public float currentFrame = 0;
    public float calMulitply = 1f;
    public float begin;

    public float speed = 1;
    public float normalTime;
    public Animator animator;
    public Animator animatorShadow;

    public void ShadowReplace()
    {
        var state = animator.GetCurrentAnimatorStateInfo(0);
        animatorShadow.Play(state.shortNameHash, 0, normalTime);
    }
    void Update()
    {
        if (FlexRunManager.Instance.isEndTutorial)
        {
            if (Input.touchCount > 0)
            {
                touch = Input.GetTouch(0);

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        begin = touch.position.x;

                        break;
                    case TouchPhase.Moved:
                        //Debug.Log(touch.deltaPosition);

                        //float cal =  FlexRunManager.Instance.keepFrame + touch.deltaPosition.x;
                        ////cal = Mathf.Clamp(cal, 0, 300f);
                        //FlexRunManager.Instance.SetAnimationFrame(cal * calMulitply);

                        var state = animator.GetCurrentAnimatorStateInfo(0);
                        var delta = touch.deltaPosition;
                        normalTime = Mathf.Clamp01(state.normalizedTime + delta.x * Time.deltaTime * speed);
                        animator.Play(state.shortNameHash, 0, normalTime);
                        animatorShadow.Play(state.shortNameHash, 0, normalTime);

                        break;
                    case TouchPhase.Stationary:

                        break;
                    case TouchPhase.Ended:
                        //AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

                        //float normalizedTime = stateInfo.normalizedTime;

                        //Debug.Log("Current animation frame: " + normalizedTime);

                        break;
                    case TouchPhase.Canceled:

                        break;
                    default:
                        break;
                }
            }
        }

    }
}
