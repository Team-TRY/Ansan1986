using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreBoard : MonoBehaviour
{
    public TMP_Text tmp;
    private void OnEnable()
    {
        tmp.text= string.Format("Score : {0}",ScoreManager.score);
        if(ScoreManager.score >= 500) this.transform.Find("Star1").gameObject.SetActive(true);
        if(ScoreManager.score >= 700) this.transform.Find("Star2").gameObject.SetActive(true);
        if(ScoreManager.score >= 1000) this.transform.Find("Star3").gameObject.SetActive(true);

    }

    public void ReplayButton()
    {
        this.gameObject.SetActive(false);
        this.transform.Find("Star1").gameObject.SetActive(false);
        this.transform.Find("Star3").gameObject.SetActive(false);
        this.transform.Find("Star3").gameObject.SetActive(false);
        tmp.text= "Score : 0";
        ScoreManager.score = 0;

    }
}
