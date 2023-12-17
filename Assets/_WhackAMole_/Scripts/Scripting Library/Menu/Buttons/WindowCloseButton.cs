using UnityEngine;

public class WindowCloseButton : MenuButton
{
    [SerializeField] private Window _window = null;

    public virtual void Close() => Menu.Instance.Close(_window);
    protected override void OnButtonClick() => Close();

    private void Reset()
    {
        GetRefs();
    }
    protected override void GetRefs()
    {
        base.GetRefs();
        _window = GetComponentInParent<Window>(true);
    }
}
