using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlexRunSoundManager : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource playBG;
    public AudioSource playEffect;
    public AudioClip clickSFX;
    public AudioClip hitSFX;
    public AudioClip loseSFX;

    public void EffectClick()
    {
        playEffect.clip = clickSFX;
        playEffect.Play();
    }
    public void EffectEnd()
    {
        playEffect.clip = loseSFX;
        playEffect.Play();
    }
    public void EffectHit()
    {
        playEffect.clip = hitSFX;
        playEffect.Play();
    }
    public void PlayBGM()
    {
        playBG.clip = hitSFX;
        playBG.Play();
    }
}
