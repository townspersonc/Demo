using System;
using UnityEngine;

public abstract class Window : MonoBehaviour
{
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

    public bool CanOpen => State == WindowState.Closed;
    public bool CanClose => State == WindowState.Open;

    public event Action<WindowState> OnStateChanged;

    public void Open(Payload_Base payload = null)
    {
        if(CanOpen) 
        { 
            State = WindowState.Open;
            gameObject.SetActive(true);
            OnOpen(payload);
        }
    }
    protected virtual void OnOpen(Payload_Base payload = null) { }
    public void Close()
    {
        if(CanClose)
        {
            gameObject.SetActive(false);
            State = WindowState.Closed;
            OnClose();
        }
    }
    protected virtual void OnClose() { }

    public enum WindowState
    {
        Closed,
        Open,
    }
}