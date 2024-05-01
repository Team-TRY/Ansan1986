using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFader : MonoBehaviour
{
    [SerializeField] private Color fadeColor;
    [SerializeField] private AnimationCurve fadeCurve;
    
    public float fadeDuration = 2;
    public string colorPropertyName = "_Color";
    
    private Renderer rend;
    
    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.enabled = false;

        FadeIn();
    }

    private void FadeIn()
    {
        Fade(1, 0);
    }
    
    public void FadeOut()
    {
        Fade(0, 1);
    }

    private void Fade(float alphaIn, float alphaOut)
    {
        StartCoroutine(FadeRoutine(alphaIn,alphaOut));
    }

    private IEnumerator FadeRoutine(float alphaIn,float alphaOut)
    {
        rend.enabled = true;

        float timer = 0;
        while(timer <= fadeDuration)
        {
            Color newColor = fadeColor;
            newColor.a = Mathf.Lerp(alphaIn, alphaOut, fadeCurve.Evaluate(timer / fadeDuration));

            rend.material.SetColor(colorPropertyName, newColor);

            timer += Time.deltaTime;
            yield return null;
        }

        Color endColor = fadeColor;
        endColor.a = alphaOut;
        rend.material.SetColor(colorPropertyName, endColor);

        if(alphaOut == 0)
            rend.enabled = false;
    }
}