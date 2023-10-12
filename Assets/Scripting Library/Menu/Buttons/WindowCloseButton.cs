using UnityEngine;

public class WindowCloseButton : MenuButton
{
    [SerializeField] private Window _window = null;

    public virtual void Close() => _menu.Close(_window);
    protected override void OnButtonClick() => Close();

    protected override void GetRefs()
    {
        base.GetRefs();
        _window = GetComponentInParent<Window>(true);
    }
}
