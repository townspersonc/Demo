using UnityEngine;
using UnityEngine.UI;

public abstract class MenuButton : MonoBehaviour
{
    [SerializeField] private Button _button = null;

    private void OnEnable()
    {
        _button.onClick.AddListener(OnButtonClick);
    }
    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnButtonClick);
    }

    protected abstract void OnButtonClick();

    private void Reset()
    {
        GetRefs();
    }
    protected virtual void GetRefs()
    {
        _button = GetComponent<Button>();
    }
}
