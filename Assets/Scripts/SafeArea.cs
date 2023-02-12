using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeArea : MonoBehaviour
{
    RectTransform rectTransform;
    public float scaleFactor = 0.5f;
    private void Awake()
    {
        RectTransform rt = GetComponent<RectTransform>();
        Rect safeArea = Screen.safeArea;
        Vector2 minAnchor = safeArea.position;
        Vector2 maxAnchor = minAnchor + safeArea.size;

        rectTransform = GetComponent<RectTransform>();

        minAnchor.x /= Screen.width;
        minAnchor.y /= Screen.height;
        maxAnchor.x /= Screen.width;
        maxAnchor.y /= Screen.height;

        rt.anchorMin = minAnchor;
        rt.anchorMax = maxAnchor;
    }

    public void ReduceOnHorizontal(bool isHorizontal) {
        if(isHorizontal) {
            rectTransform.localScale = new Vector2(rectTransform.localScale.x * scaleFactor, rectTransform.localScale.y * scaleFactor);
        } else {
            rectTransform.localScale = new Vector2(rectTransform.localScale.x, rectTransform.localScale.y);
        }
    }
}
