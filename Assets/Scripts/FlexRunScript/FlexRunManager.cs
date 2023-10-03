using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;


[Serializable]
public class ComboSacling
{
    public int comboNumber;
    public float comboScoreMultiple;
}
public class FlexRunManager : MonoBehaviourEx<FlexRunManager>
{

    [Header("Slider")]
    public UnityEngine.UI.Slider slider;
    [SerializeField] Transform shadow;
    [Header("Player&Obstacle")]
    [SerializeField] GameObject player;

    [Header("Mechanic")]
    [SerializeField] public bool isEndTutorial;
    [SerializeField] int basecore = 10;
    [SerializeField] int currentScore = 0;
    [SerializeField] int comboScore = 0;
    [SerializeField] int comboLvUp = 0;
    [SerializeField] List<ComboSacling> comboSaclings;

    [Header("GameSpeed")]
    [SerializeField] float everyPatternSpeedUp = 5;
    [SerializeField] public float currentPatternCount = 0;
    [SerializeField] int SpeedLvMax = 20;
    [SerializeField] int currentSpeedLv = 0;
    [SerializeField] public float currentSpeed = -1;
    [SerializeField] float speedMultiple = 0.1f;

    [SerializeField] int nextLv = 0;
    float currentMulitpierScore = 1;

    [Header("ScriptComponent")]
    [SerializeField] LivePointUI lifeAndScoreUI;
    [SerializeField] ResultGame resultGame;
    [SerializeField] PlayerSpriteSheet playerSpriteSheet;
    [SerializeField] RespawnManager respawnPointEnemy;
    [SerializeField] TutorialController tutorialController;
    [SerializeField] FlexRunSoundManager soundManager;
    [SerializeField] GesturesDetect gesturesDetect;
    [Header("ButtonComponent")]
    [SerializeField] GameObject swipeToPlayPanel;

    public Animator animator;
    public Animator animatorShadow;
    public void EffectClick()
    {
        soundManager.EffectClick();
    }
    public void EffectEnd()
    {
        soundManager.EffectEnd();
    }
    public void EffectHit()
    {
        soundManager.EffectHit();
    }
    protected override void Awake()
    {
        base.Awake();
    }
    void Start()
    {
        Invoke("SetupGameByJson",0.1f);

    }
    public void SetupGameByJson()
    {
        tutorialController.Setup(() => {
            isEndTutorial = true;
            swipeToPlayPanel.SetActive(true);
        });
        animator.Play("Yoga", 0, 0 / (float)frameMulity);
    }
    public void PrepareReStartGame()
    {
        swipeToPlayPanel.SetActive(true);

    }
    Action action;
    public void StartGame()
    {
        FlexRunManager.Instance.EffectClick();
        swipeToPlayPanel.SetActive(false);
        Time.timeScale = 1f;
        respawnPointEnemy.StartWave();

    }

    public void RestartGame()
    {
        EffectClick();
        //Time.timeScale = 0f;
        //SceneManager.LoadScene(0);
        //currentScore = 0;
        //currentPatternCount = 0;
        //currentSpeed = 0.25f;
        //currentSpeedLv = 1;

        //lifeAndScoreUI.ResetLiftPoint();
        ////respawnPointEnemy.ResetGame();
        //PrepareReStartGame();
        //StopTime();
        ResumeTime();
        SceneManager.LoadScene(0);
        //StopTime();
    }
    public void BackToMainApp()
    {
        //TPDO: Unload Scene
        Destroy(gameObject);
    }

    public void GameOver()
    {
        //TODO: ResetAll
        EffectEnd();
        resultGame.ShowResult(currentScore);
        StopTime();
    }
    public void StopTime()
    {
        Debug.Log("StopTime");
        Time.timeScale = 0f;
    }
    public void ResumeTime()
    {
        Debug.Log("ResumeTime");
        Time.timeScale = 1f;

    }
    public void CheckPoint()
    {
        respawnPointEnemy.ShadowReplace();
        UpdateScore(true);
        UpdateLvSpeed();
    }
    bool hitWaiting;
    public void Hit()
    {
        if (!hitWaiting)
        {
            EffectHit();
            UpdateScore(false);
            StartCoroutine(WaitHit());
        }
    }
    IEnumerator WaitHit()
    {
        hitWaiting = true;      
        yield return new WaitForSeconds(0.3f);
        hitWaiting = false;
    }
    internal void Combo()
    {
        if (!hitWaiting)
        {
            EffectHit();
            UpdateScore(true);
            StartCoroutine(WaitHit());
        }
  
    }
    public void UpdateScore(bool isComboPass)
    {
        if (!isComboPass)
        {
            comboScore = 0;
            lifeAndScoreUI.UpdateScoreUI(comboScore);
            lifeAndScoreUI.HideComboMulitply();
            return;
        }

        comboScore++;
        if (comboScore <= comboSaclings[comboSaclings.Count - 1].comboNumber)
        {
            if (comboScore <= comboSaclings[nextLv].comboNumber)
            {
                //Debug.Log("Same");
                currentMulitpierScore = comboSaclings[nextLv].comboScoreMultiple;
            }
            else
            {
                //Debug.Log("Combo Multiple: " + nextLv);
                currentMulitpierScore = comboSaclings[nextLv].comboScoreMultiple;
                lifeAndScoreUI.ShowComboMulitply(comboSaclings[nextLv].comboScoreMultiple);

                nextLv++;
            }
        }

        var plusScore = basecore * currentMulitpierScore;
        //TODO: Add Text +Score
        currentScore += (int)plusScore;
        lifeAndScoreUI.UpdateScoreUI(currentScore);

    }
    public void UpdateLvSpeed()
    {
        currentPatternCount++;
        if (currentPatternCount >= everyPatternSpeedUp)
        {
            currentSpeedLv++;
            currentPatternCount = 0;

            if (currentSpeedLv <= SpeedLvMax)
            {
                currentSpeed += speedMultiple;
            }
        }
    }

    public float keepFrame;
    public float frameMulity = 300;
    public void SetAnimationFrame(float frame)
    {
        //Debug.Log($"{frame} : {frameMulity} = {frame / frameMulity}");
        animator.Play("Yoga", 0, frame / frameMulity);
        keepFrame = frame;
        respawnPointEnemy.dummyShadow.GetComponentInChildren<Animator>().Play("Yoga", 0, frame / frameMulity);
   
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (swipeToPlayPanel.activeSelf && isEndTutorial)
            {
                StartGame();
            }
                
        }

    }
}



