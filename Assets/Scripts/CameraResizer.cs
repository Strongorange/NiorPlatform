using UnityEngine;

public class CameraResizer : MonoBehaviour
{
    private void Update()
    {
        float aspectRatio = Screen.width / Screen.height;
        Camera camera = GetComponent<Camera>();

        if (aspectRatio >= 1)
        {
            // 가로모드
            camera.orthographicSize = 5;
        }
        else
        {
            // 세로모드
            camera.orthographicSize = 10;
        }
    }
}
