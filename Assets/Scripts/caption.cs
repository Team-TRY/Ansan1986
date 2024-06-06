using System.Collections;
using TMPro;
using UnityEngine;

public class Caption : MonoBehaviour
{
    [SerializeField] private TMP_Text targetText;
    [SerializeField] private float delay = 0.125f;
    [SerializeField] private float displayDuration = 2.0f; // 텍스트가 유지될 시간
    [SerializeField] private string[] textList; // 출력할 텍스트 배열

    private int currentIndex = 0;

    private void Start()
    {
        if (targetText != null && textList.Length > 0)
        {
            StartCoroutine(TextSequence());
        }
        else
        {
            Debug.LogError("Target Text or textList is not assigned properly.");
        }
    }

    private IEnumerator TextSequence()
    {
        while (currentIndex < textList.Length)
        {
            yield return StartCoroutine(TextPrint(textList[currentIndex]));
            currentIndex++;
        }
    }

    private IEnumerator TextPrint(string text)
    {
        targetText.text = string.Empty;

        for (int count = 0; count < text.Length; count++)
        {
            targetText.text += text[count];
            yield return new WaitForSeconds(delay);
        }

        // 텍스트가 다 출력된 후 displayDuration만큼 대기
        yield return new WaitForSeconds(displayDuration);

        // 텍스트 지우기
        targetText.text = string.Empty;

        // 다음 텍스트 출력 전 잠시 대기
        yield return new WaitForSeconds(1.0f); // 필요에 따라 조정 가능
    }
}