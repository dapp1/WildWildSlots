using UnityEngine;

public class CameraOrientation : MonoBehaviour
{
    private void Awake()
    {
        // Установка горизонтальной ориентации экрана
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

    private void Start()
    {
        // Блокировка автоматического поворота на устройствах iOS
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Screen.autorotateToPortrait = false;
        }
    }
}
