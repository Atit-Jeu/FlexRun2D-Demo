using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class LivePointUI : MonoBehaviour
{
    [SerializeField] bool cheatLife;
    //[SerializeField] Image livePointImg;
    [SerializeField] Animator animatorCombo;
    [SerializeField] int currentLiftPoint = 3;
    [SerializeField] float currentScore;
    [SerializeField] int currentCombo;
    [SerializeField] int comboDegree = 4;
    [SerializeField] TMP_Text txtScore;
    [SerializeField] TMP_Text comboMulitply;
    [SerializeField] TMP_Text comboScore;
    [SerializeField] List<Image> spritesFront = new List<Image>();
    [SerializeField] List<Image> spritesBG = new List<Image>();
    public void UpdateScoreUI(int plusScore)
    {
        if (plusScore == 0 ) {

            LiftPointDown();
            
            return;
        }
        currentScore = plusScore;
        txtScore.text = $"{currentScore}";
        ComboPointPlusShow();
        //LiftPointDown();
    }
    public void SetupUI(Sprite livepointSpr, Sprite livepointBGSpr)
    {
        //livePointImg.sprite = livepointSpr;
        for (int i = 0; i < spritesFront.Count; i++) {
            spritesFront[i].sprite = livepointSpr;
        }
        for (int i = 0; i < spritesBG.Count; i++)
        {
            spritesBG[i].sprite = livepointBGSpr;
        }
    }
    public void ResetLiftPoint()
    {
        currentLiftPoint = 3;
        Debug.Log("currentLiftPoint " + currentLiftPoint);

        for (int i = 0; i < currentLiftPoint; i++)
        {
            spritesFront[i].gameObject.SetActive(true);
        }
      

    }
    public void LiftPointDown()
    {
        if(!cheatLife)
        {
            currentLiftPoint--;
            //Debug.Log(currentLiftPoint);


            spritesFront[currentLiftPoint].gameObject.SetActive(false);
            ComboPointNegativeShow();
            if (currentLiftPoint <= 0)
            {
                FlexRunManager.Instance.GameOver();
                //TODO: GameOver
                return;
            }
        }
     

    }
    public void ComboPointPlusShow()
    {
        currentCombo++;
        comboScore.text = "Combo x"+currentCombo.ToString();
        comboScore.GetComponent<RectTransform>().rotation = Quaternion.AngleAxis(Random.Range(-comboDegree, comboDegree), Vector3.forward);
        animatorCombo.SetTrigger("PlayCombo");
    }
    public void ShowComboMulitply(float combo)
    {
        comboMulitply.text = "x" + combo;
        comboMulitply.gameObject.SetActive(true);
    }
    public void HideComboMulitply()
    {
        comboMulitply.gameObject.SetActive(false);
    }
    public void ComboPointNegativeShow()
    {
        currentCombo = 0;
        comboScore.text = "Combo Failed";

        comboScore.GetComponent<RectTransform>().rotation = Quaternion.AngleAxis(Random.Range(-comboDegree, comboDegree), Vector3.forward);
        animatorCombo.SetTrigger("PlayCombo");
        //animatorCombo.SetTrigger("Combo");
        //comboScore.gameObject.SetActive(true);
        //animatorCombo.enabled = true;
    }
    public void ComboClose()
    {
        comboScore.gameObject.SetActive(false);
    }


}
