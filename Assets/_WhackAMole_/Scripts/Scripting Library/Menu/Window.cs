using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public abstract class Window : MonoBehaviour
{
    [SerializeField] private Image _animRayCastBlock;

    private WindowState _state;
    public WindowState State
    {
        get => _state;
        private set
        {
            _state = value;
            OnStateChanged?.Invoke(_state);
        }
    }

    public bool CanOpen => State == WindowState.Closed || State == WindowState.Closing;
    public bool CanClose => State == WindowState.Open || State == WindowState.Opening;

    public abstract int OpenMiliSeconds { get; }
    public abstract int CloseMiliSeconds { get; }

    public event Action<WindowState> OnStateChanged;

    public async void Open(Payload_Base payload = null)
    {
        if (CanOpen)
        {
            OnOpenStart(payload);
            PerformOpenAnimation();
            await Task.Delay(OpenMiliSeconds);
            OnOpenFinish(payload);
        }
    }
    protected virtual void OnOpenStart(Payload_Base payload = null)
    {
        State = WindowState.Opening;
        gameObject.Enable();
        _animRayCastBlock.raycastTarget = true;
    }
    protected abstract void PerformOpenAnimation();
    protected virtual void OnOpenFinish(Payload_Base payload = null)
    {
        State = WindowState.Open;
        _animRayCastBlock.raycastTarget = false;
    }

    public async void Close()
    {
        if (CanClose)
        {
            OnCloseStart();
            PerformCloseAnimation();
            await Task.Delay(CloseMiliSeconds);
            OnCloseFinish();
        }
    }
    protected virtual void OnCloseStart()
    {
        State = WindowState.Closing;
        _animRayCastBlock.raycastTarget = true;
    }
    protected abstract void PerformCloseAnimation();
    protected virtual void OnCloseFinish()
    {
        State = WindowState.Closed;
        gameObject.Disable();
        _animRayCastBlock.raycastTarget = false;
    }

    public enum WindowState
    {
        Closed,
        Open,
        Closing,
        Opening
    }
}