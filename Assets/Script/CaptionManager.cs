using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CaptionManager : MonoBehaviour
{
    public GameObject captionBox;
    public TMP_Text captionText;
    public int typingAnimationSec = 15; // 타이핑 애니메이션 시간

    public static CaptionManager instance;

    public Caption caption;
    public Button nextCapButton;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        StartCoroutine(CaptionManager.instance.ShowCaption(caption));
    }

    public IEnumerator TypeCaption(string caption)
    {
        captionText.text = "";
        foreach(var character in caption.ToCharArray())
        {
            captionText.text += character;
            yield return new WaitForSeconds(1f/typingAnimationSec);
        }
    }

    public IEnumerator ShowCaption(Caption caption)
    {
        foreach(var line in caption.lines)
        {
            yield return TypeCaption(line);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.D));
        }

    }

}