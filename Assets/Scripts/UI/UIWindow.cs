using UnityEngine;

class UIWindow : MonoBehaviour
{
    [SerializeField] protected GameObject _uiContainer;

    public void ShowUI() => _uiContainer.SetActive(true);

    public void HideUI() => _uiContainer.SetActive(false);
}