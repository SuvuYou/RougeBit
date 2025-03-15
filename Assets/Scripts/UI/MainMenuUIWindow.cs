using UnityEngine;

class MainMenuUIWindow : UIWindow
{
    [SerializeField] private ButtonUI _startButton;

    private void Start() => _startButton.SetupButton(GameManager.Instance.StartGame);
}