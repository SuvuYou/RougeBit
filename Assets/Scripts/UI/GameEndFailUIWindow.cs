using UnityEngine;

class GameEndFailUIWindow : UIWindow
{
    [SerializeField] private ButtonUI _tryAgainButton;
    [SerializeField] private ButtonUI _mainMenuButton;

    private void Start() 
    {
        // _tryAgainButton.SetupButton(GameManager.Instance.StartRound);
        _mainMenuButton.SetupButton(GameManager.Instance.EnterMainMenu);
    }
}