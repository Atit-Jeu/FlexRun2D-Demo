using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] Button btnResume;
    [SerializeField] Button btnRestart;
    [SerializeField] Button btnOpenPauseMenu;
    [SerializeField] GameObject objPauseMenu;
    // Start is called before the first frame update
    void Start()
    {
        btnOpenPauseMenu.onClick.AddListener(() => { 
            objPauseMenu.SetActive(true);
            FlexRunManager.Instance.StopTime();
            FlexRunManager.Instance.EffectClick();
        });
        btnResume.onClick.AddListener(() => {
            objPauseMenu.SetActive(false);
            FlexRunManager.Instance.ResumeTime();
            FlexRunManager.Instance.EffectClick();

        });
        btnRestart.onClick.AddListener(() => {
            objPauseMenu.SetActive(false);
            FlexRunManager.Instance.RestartGame();
            FlexRunManager.Instance.EffectClick();
        });
    }
}
