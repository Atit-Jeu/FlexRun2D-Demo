using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultGame : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text showTotalScore;
    [SerializeField] GameObject showNewHighScore;
    [SerializeField] GameObject panelResult;
    [SerializeField] Button btnRestart;
    [SerializeField] Button btnLeaveGame;
    // Start is called before the first frame update
    private void Start()
    {
        btnRestart.onClick.AddListener(RestartGame);
        btnLeaveGame.onClick.AddListener(LeaveGame);
    }
    public void ShowResult (int lastScore)
    {
        panelResult.SetActive (true);
        showTotalScore.text = lastScore.ToString();
        //TODO: PlayerScoreFrom Project
        //if(showNewHighScore > playerHighScore)
        //{
        //    showNewHighScore.SetActive(true);
        //}
        //else
        //{
        //    showNewHighScore.SetActive(false);
        //}
        //showNewHighScore.SetActive(true);
    }
    public void RestartGame()
    {
        FlexRunManager.Instance.EffectClick();
        panelResult.SetActive (false);
        FlexRunManager.Instance.RestartGame();
    }
    public void LeaveGame()
    {
        FlexRunManager.Instance.EffectClick();
        Application.Quit();
        //TODO: Go to main app
    }
}
