using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIResize : MonoBehaviour
{
    public float factor = 0.5f;
    RectTransform rectTransform;

    // Start is called before the first frame update
    void Awake()
    {
        factor = 0.5f;
        rectTransform = GetComponent<RectTransform>();
        if ((float)Screen.width / (float)Screen.height > 1)
        {
            Scale();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Scale();
    }

    void Scale()
    {
        bool isHorizontal = (float)Screen.width / (float)Screen.height > 1;
        if (isHorizontal)
        {
            rectTransform.localScale = new Vector3(factor, factor, 0);
        }
        else
        {
            rectTransform.localScale = new Vector3(1, 1, 0);
        }
    }
}
