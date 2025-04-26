using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class AspectRatioGuard : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _cineCamera;
    [SerializeField] private float targetWidth = 10f; // Desired world units across
    void Start()
    {
        //SetAspectRation();
        // AdjustCameraWidth();
        StartCoroutine(AdjustNextFrame());
    }

    private static void SetAspectRation()
    {
        float targetAspect = 9f / 16f;
        float windowAspect = (float)Screen.width / Screen.height;
        float scaleHeight = windowAspect / targetAspect;

        Camera camera = Camera.main;

        if (scaleHeight < 1f)
        {
            Rect rect = camera.rect;

            rect.width = 1f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1f - scaleHeight) / 2f;

            camera.rect = rect;
        }
        else
        {
            float scaleWidth = 1f / scaleHeight;

            Rect rect = camera.rect;

            rect.width = scaleWidth;
            rect.height = 1f;
            rect.x = (1f - scaleWidth) / 2f;
            rect.y = 0;

            camera.rect = rect;
        }
    }

    void AdjustCameraWidth()
    {
        if (_cineCamera == null)
        {
            Debug.LogWarning("No CinemachineCamera assigned!");
            return;
        }

        float screenAspect = (float)Screen.width / (float)Screen.height;
        float orthoSize = (targetWidth / screenAspect) * 0.5f;

        var lens = _cineCamera.Lens;
        lens.OrthographicSize = orthoSize;
        _cineCamera.Lens = lens;
    }

    private IEnumerator AdjustNextFrame()
    {
        yield return null; // wait one frame
        AdjustCameraWidth();
    }
}
