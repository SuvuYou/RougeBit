using System.Collections.Generic;
using UnityEngine;

class UIManager : Singlton<UIManager>
{
    [System.Serializable]
    [System.Flags]
    public enum UIWindows {
        None            = 0,
        Shop            = 1 << 0,
        MainMenu        = 1 << 1,
        Pause           = 1 << 2,
        GameEndSuccess  = 1 << 3,
        GameEndFail     = 1 << 4,
    }

    [SerializeField] private UglySerializableDictionary<UIWindows, UIWindow> _uiWindows = new();

    private UIWindows _activeWindows = UIWindows.None;

    private void Start() => _updateWindownsVisibility();

    private void _updateWindownsVisibility()
    {
        foreach (KeyValuePair<UIWindows, UIWindow> pair in _uiWindows.ToDictionary())
        {
            if (_activeWindows.HasFlag(pair.Key)) pair.Value.ShowUI(); 
            else pair.Value.HideUI();
        }
    }

    private void _addWindow(UIWindows window) 
    {
        _activeWindows |= window;
        _updateWindownsVisibility();
    }

    private void _removeWindow(UIWindows window) 
    {
        _activeWindows &= ~window;
        _updateWindownsVisibility();
    } 

    public void CloseAllWindows() 
    {
        _activeWindows = UIWindows.None;
        _updateWindownsVisibility();
    } 

    public void OpenWindow(UIWindows window) => _addWindow(window);
    public void CloseWindow(UIWindows window) => _removeWindow(window);

    // public void OpenShop() => _addWindow(UIWindows.Shop);

    // public void CloseShop() => _removeWindow(UIWindows.Shop);


}