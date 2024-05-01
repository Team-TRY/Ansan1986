using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private Button btnStartGame;
    [SerializeField] private Button btnSettings;
    [SerializeField] private Button btnCredits;
    [SerializeField] private Button btnQuitGame;

    [SerializeField] AudioClip clickSound;

    private void Start()
    {
        //Action buttonClickAction = () => SoundManager.Instance.SFXPlay("btnClick", clickSound);

        // btnStartGame.onClick.AddListener(() => { buttonClickAction(); OpenPanel_StartGame(); });
        // btnSettings.onClick.AddListener(() => { buttonClickAction(); OpenPanel_Settings(); });
        // btnCredits.onClick.AddListener(() => { buttonClickAction(); OpenPanel_Credits(); });
        // btnQuitGame.onClick.AddListener(() => { buttonClickAction(); OpenPopup_QuitGame(); });
        
        btnStartGame.onClick.AddListener(() => { OpenPanel_StartGame(); });
        btnSettings.onClick.AddListener(() => { OpenPanel_Settings(); });
        btnCredits.onClick.AddListener(() => { OpenPanel_Credits(); });
        btnQuitGame.onClick.AddListener(() => { OpenPopup_QuitGame(); });
    }
    public void OpenPanel_StartGame()
    {
        
    }

    void OpenPanel_Settings()
    {
        UIManager.Instance.OpenUI<UISettings>();
    }

    void OpenPanel_Credits()
    {
        UIManager.Instance.OpenUI<UICredits>();
    }

    void OpenPopup_QuitGame()
    {
        
    }
}
