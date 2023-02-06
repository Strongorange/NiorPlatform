using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResizer : MonoBehaviour
{
    private Camera camera;

    private void Start()
    {
        camera = GetComponent<Camera>();

        float aspectRatio = (float)Screen.width / (float)Screen.height;
        float cameraSize = camera.orthographicSize;

        if (aspectRatio < 1)
        {
            cameraSize = 10;
        }

        camera.orthographicSize = cameraSize;
    }
}
