using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FixedAspectRatio : MonoBehaviour
{
    public float targetAspect = 16.0f / 9.0f; // Set your desired aspect ratio here

    void Start()
    {
        AdjustCamera();
    }

    void AdjustCamera()
    {
        Camera camera = GetComponent<Camera>();

        // Determine the game window's current aspect ratio
        float windowAspect = (float)Screen.width / (float)Screen.height;

        // Calculate the scale height that is needed to maintain the aspect ratio
        float scaleHeight = windowAspect / targetAspect;

        // If the scale height is less than the current height, add letterboxing
        if (scaleHeight < 1.0f)
        {
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;

            camera.rect = rect;
        }
        else // Add pillarboxing
        {
            float scaleWidth = 1.0f / scaleHeight;

            Rect rect = camera.rect;

            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }
    }

    void OnPreCull()
    {
        // Ensure the background color is applied to the letterbox or pillarbox areas
        GL.Clear(true, true, Color.black);
    }
}
