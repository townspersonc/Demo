using TMPro;
using UnityEngine;

public class TMP_FormatedText : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField, TextArea] private string _format = string.Empty;

    public string Text => _text.text;

    public void SetText(params object[] arguments)
    {
        FormatCheck();

        _text.text = string.Format(_format, arguments);
    }

    private void FormatCheck()
    {
        if (_format == string.Empty) _format = _text.text;
    }

    private void Reset()
    {
        _text = GetComponent<TMP_Text>();
        if (_text != null) SetFromatFromText();
    }
    [ContextMenu(nameof(SetFromatFromText))]
    private void SetFromatFromText()
    {
        _format = _text.text;
    }
}
