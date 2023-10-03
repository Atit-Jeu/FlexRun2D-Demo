using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class TestAniCode : MonoBehaviour
{
    public float speed;
        private void Update()
    {
        //    Using Scripting:

        //If you want to control the animation frames programmatically, you can use scripts to manipulate the animation's time.

        //Access the Animator/Animation Component: Depending on whether you are using an Animator Controller or an Animation component, you need to get a reference to it. You can use GetComponent<Animator>() or GetComponent<Animation>() on your GameObject.

        //Control Animation Time: You can control the animation time by changing the speed property of the Animator or Animation component. A speed of 1.0 means normal speed, 2.0 means double speed, and 0.5 means half speed. To pause the animation, you can set the speed to 0.

        //csharp
        // For Animator
        Animator animator = GetComponent<Animator>();
        animator.speed = speed; // Adjust the value to control animation speed
        

        //// For Animation
        //Animation animation = GetComponent<Animation>();
        //animation["YourAnimationClipName"].speed = 0.5f; // Adjust the value to control animation speed
        //                                                 //Control Animation Time Programmatically: You can also control the animation time directly using normalized time (0 to 1) to specify which frame of the animation to play. For example, to play the animation from the halfway point:

        ////csharp
        //// For Animator
        //Animator animator = GetComponent<Animator>();
        //float desiredNormalizedTime = 0.5f;
        //animator.Play("YourAnimationClipName", 0, desiredNormalizedTime);

        //// For Animation
        //Animation animation = GetComponent<Animation>();
        //float desiredNormalizedTime = 0.5f;
        //animation["YourAnimationClipName"].normalizedTime = desiredNormalizedTime;
        ////Remember to replace "YourAnimationClipName" with the actual name of your animation clip.

        ////Using these methods, you can control the specific animation frame and time in Unity either through the Animation window or by scripting it using C#.

    }
}
