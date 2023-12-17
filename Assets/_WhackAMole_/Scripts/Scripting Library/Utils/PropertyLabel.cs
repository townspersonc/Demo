using UnityEngine;

public abstract class PropertyLabel<T> : MonoBehaviour
{
    [SerializeField] private TMP_FormatedText _label;
    private Property<T> _prop;

    public void Init(Property<T> property)
    {
        UnSubscribe();

        _prop = property;
        _prop.OnChange += RefreshLabel;
        RefreshLabel(_prop.Value);
    }

    private void OnDisable()
    {
        UnSubscribe();
    }

    private void UnSubscribe()
    {
        if (_prop != null) _prop.OnChange -= RefreshLabel;
        _prop = null;
    }

    private void RefreshLabel(T value)
    {
        _label.SetText(value);
    }
}
