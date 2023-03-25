using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Pannel : MonoBehaviour
{
    public Vector2 size;
    public RectTransform contentRect;
    RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void Start()
    {
        size = rect.sizeDelta;
    }

    public void SetLastSibling()
    {
        rect.SetAsLastSibling();
    }

    public void SetParent(RectTransform parent)
    {
        rect.SetParent(parent);
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
