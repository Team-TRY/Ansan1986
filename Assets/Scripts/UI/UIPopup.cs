using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIPopup : MonoBehaviour
{
    [SerializeField] private TMP_Text txtTitle;
    [SerializeField] private TMP_Text txtContent;
    
    [SerializeField] private Button btnBack;
    [SerializeField] private Button btnConfirm;
    [SerializeField] private Button btnCancel;
    [SerializeField] AudioClip clickSound;
    [SerializeField] AudioClip popupSound;

    private Action OnConfirm;

    void Start()
    {
        AddButtonListener(btnBack, Close);
        AddButtonListener(btnCancel, Close);
        AddButtonListener(btnConfirm, Confirm);
    }

    void AddButtonListener(Button button, UnityAction action)
    {
        if (button != null)
        {
            button.onClick.AddListener(action);
        }
    }


    public void SetPopup(string title, string content, Action onConfirm = null)
    {
        //SoundManager.Instance.SFXPlay("popup", popupSound);
        txtTitle.text = title;
        txtContent.text = content;

        OnConfirm = onConfirm;
    }
    
    void Confirm()
    {
        //SoundManager.Instance.SFXPlay("btnClick", clickSound);
        if (OnConfirm != null)
        {
            OnConfirm();
            OnConfirm = null;
        }

        Close();
    }

    void Close()
    {
        //SoundManager.Instance.SFXPlay("btnClick", clickSound);
        gameObject.SetActive(false);
    }
}
