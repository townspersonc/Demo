using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Menu : Singleton<Menu>
{
    [SerializeField] private List<Window> _windows;
    [SerializeField] private Window _startupWindow;

    public event Action<Window> OnWindowOpen;
    public event Action<Window> OnWindowClose;

    private void Start()
    {
        CloseAll();
        if (_startupWindow != null) Open(_startupWindow);
    }

    public Window Open<T>(Payload_Base payload = null) where T : Window
    {
        var window = GetWindow<T>();
        Open(window, payload);
        return window;
    }
    public void Open(Window window, Payload_Base payload = null)
    {
        if (window.CanOpen)
        {
            window.Open(payload);
            OnWindowOpen?.Invoke(window);
        }
    }

    public Window Close<T>() where T : Window
    {
        var window = GetWindow<T>();
        Close(window);
        return window;
    }
    public void Close(Window window)
    {
        if (window.CanClose)
        {
            window.Close();
            OnWindowClose?.Invoke(window);
        }
    }
    public void CloseAll()
    {
        _windows.ForEach(w => Close(w));
    }
    public void CloseAllExcept<T>() where T : Window
    {
        var window = GetWindow<T>();
        foreach (Window w in _windows)
        {
            if (window != w)
            {
                Close(w);
            }
        }
    }

    public T GetWindow<T>() where T : Window
    {
        foreach (var w in _windows)
        {
            if (w is T result)
            {
                return result;
            }
        }

        Debug.LogError($"Window of type {nameof(T)} not found.");
        return null;
    }

    private void Reset()
    {
        FindWindows();
    }
    private void FindWindows()
    {
        _windows = FindObjectsOfType<Window>(true).ToList();
    }
}

