using UnityEngine;

public class CameraResizer : MonoBehaviour
{
    SafeArea safe;
    PlayerMove player;

    private void Update()
    {
        float aspectRatio = Screen.width / Screen.height;
        Camera camera = GetComponent<Camera>();

        if (aspectRatio >= 1)
        {
            // 가로모드
            camera.orthographicSize = 7;
            safe.ReduceOnHorizontal(true);
        }
        else
        {
            // 세로모드
            camera.orthographicSize = 9;
            safe.ReduceOnHorizontal(false);
        }
    }
}
