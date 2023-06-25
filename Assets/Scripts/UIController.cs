using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject _startMenu;
    [SerializeField] private GameObject _gameMenu;
    [SerializeField] private GameObject _betHUD;
    [SerializeField] private GameObject _freeGameButton;
    [SerializeField] private GameObject _helpWindow;
    [SerializeField] private GameObject _buttonBack;

    public bool inGame;

    public void HideStartMenu()
    {
        _startMenu.SetActive(false);
        _gameMenu.SetActive(true);
        _buttonBack.SetActive(true);
        inGame = true;
    }

    public void ActiveStartMenu()
    {
        _startMenu.SetActive(true);
        _gameMenu.SetActive(false);
        _buttonBack.SetActive(false);
    }

    public void ActiveFreeGame()
    {
        _betHUD.SetActive(false);
        _freeGameButton.SetActive(true);
    }

    public void ActiveStandartGame()
    {
        BetController.Instance.FreeSpin();
        _betHUD.SetActive(true);
        _freeGameButton.SetActive(false);
    }

    public void ActiveHelpWindow()
    {
        _helpWindow.SetActive(true);
        _buttonBack.SetActive(true);
        _startMenu.SetActive(false);
        _gameMenu.SetActive(false);

    }

    public void BackToStartMenu()
    {
        if (!BetController.Instance.animStart)
        {
            BetController.Instance.DestroySpinObjects();
            _startMenu.SetActive(true);
            _gameMenu.SetActive(false);
            _buttonBack.SetActive(false);
            inGame = false;
        }
    }
    public void CloseGame()
    {
        Application.Quit();
    }
}
