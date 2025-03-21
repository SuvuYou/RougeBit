using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

class MovableChain : MonoBehaviour
{
    [SerializeField] private UIWindow _uiWindow;
    [SerializeField] private MovableElementUI[] _elements;

    public UnityEvent OnChainComplete;

    public event Func<Task> OnBeforeMoveBack;

    private void Start()
    {
        Reset();
        
        _uiWindow.OnShowUI += () => Move();
        _uiWindow.OnHideUI += () => _onMoveBack();
    }

    private async Task _onMoveBack() 
    {
        if (OnBeforeMoveBack != null)
        {
            await OnBeforeMoveBack?.Invoke();
        }
        
        await MoveBack();
    }

    public void Move()
    {
        Sequence sequence = DOTween.Sequence();

        foreach (var element in _elements)
        {
            sequence.Append(element.Move());
        }

        sequence.SetUpdate(true).OnComplete(() => OnChainComplete?.Invoke());
    } 

    public Task MoveBack()
    {
        Sequence sequence = DOTween.Sequence();

        foreach (var element in _elements)
        {
            sequence.Append(element.MoveBack());
        }

        return sequence.OnComplete(() => OnChainComplete?.Invoke()).SetUpdate(true).AsyncWaitForCompletion();
    }

    public void Reset()
    {
        foreach (var element in _elements)
        {
            element.Reset();
        }
    }
}