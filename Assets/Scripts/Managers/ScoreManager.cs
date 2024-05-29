using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreManager : MonoBehaviour
{

    static public int score = 1500;

    public enum ScoreCategory
    {
        routing, //차도 준수
        speed, // 속도 준수
        stop, // 정차위치 준수
        turnSignal, // 깜빡이 준수
        time, // 타임어택 성공시
        passanger, // 승객 승하차시
        accident // 교통사고시
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Scoring(ScoreCategory CategoryName)
    {
        if(CategoryName == ScoreCategory.routing) score += 10;
        else if (CategoryName == ScoreCategory.speed) score += 10;
        else if (CategoryName == ScoreCategory.stop) score += 300;
        else if (CategoryName == ScoreCategory.turnSignal) score += 150;
        else if (CategoryName == ScoreCategory.time) score += 700;
        else if (CategoryName == ScoreCategory.passanger) score += 300;
        else if (CategoryName == ScoreCategory.accident) score -= 200;
    
    }
        
}
