using UnityEngine;

class MainMenuUIWindow : UIWindow
{
    [SerializeField] private ButtonUI _startButton;
    [SerializeField] private ButtonUI _exitButton;

    private void Start()
    {
        _startButton.SetupButton(GameManager.Instance.StartGame);
        _exitButton.SetupButton(ExitGame);
    }

    private void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}