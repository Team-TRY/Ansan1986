using DG.Tweening;
using UnityEngine;

public class DotweenUIManager : MonoBehaviour
{
    [SerializeField] private float _transSpeed;

    public void OpenPanel(GameObject panel)
    {
        panel.transform.DOScale(1, _transSpeed);
    }
    public void ClosePanel(GameObject panel)
    {
        panel.transform.DOScale(0, _transSpeed).OnComplete(() => gameObject.SetActive(false));
    }

    public void MaxFade(GameObject panel)
    {
        panel.GetComponent<CanvasGroup>().DOFade(1, _transSpeed).SetUpdate(true);
    }
    public void MinFade(GameObject panel)
    {
        panel.GetComponent<CanvasGroup>().DOFade(0, _transSpeed).OnComplete(() => gameObject.SetActive(false)).SetUpdate(true);
    }

    public void MoveUp(GameObject panel)
    {
        panel.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 1000), _transSpeed).OnComplete(() => gameObject.SetActive(false)).SetUpdate(true);
    }
    public void MoveDown(GameObject panel)
    {
        panel.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 0), _transSpeed).SetUpdate(true);
    }
}