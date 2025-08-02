using UnityEngine;

public class camScript : MonoBehaviour
{
    void Start()
    {
        foreach (Canvas canvas in FindObjectsOfType<Canvas>())
        {
            if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
            {
                canvas.worldCamera = Camera.main;
            }
        }
    }
}
