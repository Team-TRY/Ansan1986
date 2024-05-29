using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RemainTime : MonoBehaviour
{

    public TMP_Text rTimeText;
    public static float rTime = 5f;
    // Start is called before the first frame update
    void Start()
    {
        rTimeText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        rTime -= Time.deltaTime;
        if(rTime<0) rTime = 0;
        rTimeText.text = "Time: " + Mathf.Round(rTime);
    }
}
