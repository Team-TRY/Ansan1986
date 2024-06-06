using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Speedometer : MonoBehaviour
{
    public NavMeshAgent target; // NavMeshAgent를 타겟으로 설정

    public float maxSpeed = 0.0f; 

    public float minSpeedArrowAngle;
    public float maxSpeedArrowAngle;

    [Header("UI")]
    public TMP_Text speedLabel; 
    public RectTransform arrow;

    private float speed = 0.0f;

    private void Update()
    {
        if (target != null)
        {
            speed = target.velocity.magnitude * 3.6f; // NavMeshAgent의 속도를 사용하여 km/h로 변환

            if (speedLabel != null)
                speedLabel.text = ((int)speed) + " km/h";
            if (arrow != null)
                arrow.localEulerAngles =
                    new Vector3(0, 0, Mathf.Lerp(minSpeedArrowAngle, maxSpeedArrowAngle, speed / maxSpeed));
        }
    }
}