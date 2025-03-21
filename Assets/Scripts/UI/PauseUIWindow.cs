using UnityEngine;

class PauseUIWindow : UIWindow
{
    [SerializeField] private ButtonUI _continueButton;
    [SerializeField] private ButtonUI _mainMenuButton;

    private void Start() 
    {
        _continueButton.SetupButton(GameManager.Instance.StartGame);
        _mainMenuButton.SetupButton(GameManager.Instance.EnterMainMenu);
    }
}