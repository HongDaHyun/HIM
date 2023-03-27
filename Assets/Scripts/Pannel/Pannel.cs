using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PannelState { Small, Edit }

public abstract class Pannel : MonoBehaviour
{
    public Vector2 size, position;
    public RectTransform contentRect;
    public PannelState pannelState = PannelState.Small;
    RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void Start()
    {
        size = rect.sizeDelta;
        position = rect.anchoredPosition;
    }

    public virtual void EditMode()
    {
        pannelState = PannelState.Edit;
        StopAllCoroutines();
        rect.SetAsLastSibling();
        rect.anchoredPosition = new Vector2(0, 0);
        StartCoroutine(ScaleUp(new Vector2(UIManager.Instance.screenWidth - 100, UIManager.Instance.screenHeight - 560)));
    }

    public void DeEditMode()
    {
        pannelState = PannelState.Small;
        StopAllCoroutines();
        rect.anchoredPosition = position;
        StartCoroutine(ScaleDown(size));
    }

    public IEnumerator ScaleUp(Vector2 maxSize)
    {
        float loss = 1f;
        while (rect.sizeDelta.x < maxSize.x - loss || rect.sizeDelta.y < maxSize.y - loss)
        {
            rect.sizeDelta = Vector2.Lerp(rect.sizeDelta, maxSize, 10f * Time.deltaTime);
            yield return null;
        }
        rect.sizeDelta = maxSize;
    }

    public IEnumerator ScaleDown(Vector2 minSize)
    {
        float loss = 1f;
        while (rect.sizeDelta.x > minSize.x + loss || rect.sizeDelta.y > minSize.y + loss)
        {
            rect.sizeDelta = Vector2.Lerp(rect.sizeDelta, minSize, 10f * Time.deltaTime);
            yield return null;
        }
        rect.sizeDelta = minSize;
    }
}
