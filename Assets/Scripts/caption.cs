using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class caption : MonoBehaviour
{
    private string text;
    public TMP_Text targetText;
    private float delay = 0.125f;

    void Start()
    {
        text = targetText.text.ToString();
        targetText.text = " ";

        StartCoroutine(textPrint(delay));
    }

    IEnumerator textPrint(float d)
    {
        int count = 0;

        while (count != text.Length)
        {
            if (count < text.Length)
            {
                targetText.text += text[count].ToString();
                count++;
            }

            yield return new WaitForSeconds(delay);
        }
    }
}