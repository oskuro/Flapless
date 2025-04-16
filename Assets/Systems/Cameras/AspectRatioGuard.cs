using UnityEngine;

public class AspectRatioGuard : MonoBehaviour
{
    void Start()
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

}
