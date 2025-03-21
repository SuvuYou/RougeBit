using System;
using System.Threading.Tasks;
using UnityEngine;

class UIWindow : MonoBehaviour
{
    [SerializeField] protected GameObject _uiContainer;

    public event Func<Task> OnHideUI;
    public event Action OnShowUI;

    private bool _lastContainerVisibility;

    public void ShowUI() 
    {
        _lastContainerVisibility = true;

        _uiContainer.SetActive(_lastContainerVisibility);

        OnShowUI?.Invoke();
    }

    public async void HideUI() 
    {
        _lastContainerVisibility = false;

        Task task = OnHideUI?.Invoke();

        if (task != null) 
        {
            await task;
        }

        _uiContainer.SetActive(_lastContainerVisibility);
    }
}