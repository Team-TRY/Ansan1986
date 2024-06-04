using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RemainTime : MonoBehaviour
{

    public TMP_Text rTimeText;
    public static float rTime = 65f;
    private float sec;
    private float min;
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
        sec = rTime % 60;
        min = rTime / 60;
        rTimeText.text = string.Format("{0:D2}:{1:D2}",(int)min, (int)sec );
    }
}
