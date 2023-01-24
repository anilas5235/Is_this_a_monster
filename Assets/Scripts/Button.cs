using TMPro;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] private Color _colorOff, _colorOn;
    public bool isUnlocked = true;
    private Button _myButton;

    private TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _myButton = GetComponent<Button>();
    }

    private void OnEnable()
    {
        if (gameObject.activeSelf) { ToggleColor(false); }
        _myButton.enabled = isUnlocked;
    }

    public void ToggleColor(bool value)
    {
        if (!isUnlocked) { return; }
        switch (value)
        {
            case true: _text.color = _colorOn; break;
            case false: _text.color = _colorOff; break;
        }
    }
}
