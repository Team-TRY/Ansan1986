using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreBoard : MonoBehaviour
{
    static public int score;
    public TMP_Text tmp;
    void Start()
    {
        score = 0;
        score += (int)(RemainTime.rTime * 10);

        this.transform.Find("Star1").gameObject.SetActive(false);
        this.transform.Find("Star2").gameObject.SetActive(false);
        this.transform.Find("Star3").gameObject.SetActive(false);
        
        tmp.text= string.Format("Score : {0}",score);

        if(score >= 100) this.transform.Find("Star1").gameObject.SetActive(true);
        if(score >= 600) this.transform.Find("Star2").gameObject.SetActive(true);
        if(score >= 900) this.transform.Find("Star3").gameObject.SetActive(true);

    }


}
