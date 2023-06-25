using UnityEngine;

public class CameraOrientation : MonoBehaviour
{
    private void Awake()
    {
        // ��������� �������������� ���������� ������
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

    private void Start()
    {
        // ���������� ��������������� �������� �� ����������� iOS
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Screen.autorotateToPortrait = false;
        }
    }
}
