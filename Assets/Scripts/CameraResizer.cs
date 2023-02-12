using UnityEngine;
using Cinemachine;

public class CameraResizer : MonoBehaviour
{
    private float aspectRatio;
    private float targetAspectRatio;

    public float cameraSize = 10f;
    public float portraitAdjustFactor = 0.85f;
    public float landscapeAdjustFactor = 1.2f;

    private CinemachineVirtualCamera virtualCamera;

    private void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        aspectRatio = (float)Screen.width / (float)Screen.height;
        SetTargetAspectRatio();
        AdjustCameraSize();
    }

    private void Update()
    {
        aspectRatio = (float)Screen.width / (float)Screen.height;
        SetTargetAspectRatio();
        AdjustCameraSize();
    }

    private void SetTargetAspectRatio()
    {
        if (Screen.width > Screen.height)
        {
            targetAspectRatio = 9f / 16f;
        }
        else
        {
            targetAspectRatio = 16f / 9f;
        }
    }

    private void AdjustCameraSize()
    {
        if (aspectRatio < targetAspectRatio)
        {
            virtualCamera.m_Lens.OrthographicSize = cameraSize / portraitAdjustFactor;
        }
        else
        {
            virtualCamera.m_Lens.OrthographicSize = cameraSize / landscapeAdjustFactor;
        }
    }
}
