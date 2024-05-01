using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICredits : MonoBehaviour
{
    [SerializeField] private Button btnBack;
    [SerializeField] AudioClip clickSound;

    private void Start()
    {
        btnBack.onClick.AddListener(Back_Credits);
    }

    void Back_Credits()
    {
        if(clickSound != null)
        {
            SoundManager.Instance.SFXPlay("btnClick", clickSound);
        }
        gameObject.SetActive(false);
    }
}
