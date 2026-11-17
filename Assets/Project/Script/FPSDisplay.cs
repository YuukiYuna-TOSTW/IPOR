using UnityEngine;
using TMPro;

public class FPSDisplay : MonoBehaviour
{
    public TextMeshProUGUI fpsText;
    private float deltaTime = 0.0f;

    void Update()
    {
        // Menghitung delta time untuk frame rate yang akurat
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;

        if (fpsText != null)
        {
            fpsText.text = string.Format("FPS: {0:0.0} ({1:0.0} ms)", fps, msec);
        }
    }
}