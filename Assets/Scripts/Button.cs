using System;
using TMPro;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public class Button : MonoBehaviour
{
    [SerializeField] private Color _colorOff, _colorOn;
    public bool isUnlocked = true;
    private UnityEngine.UI.Button _myButton;

    private TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _myButton = GetComponent<UnityEngine.UI.Button>();
    }

    private void OnEnable()
    {
        if (gameObject.activeSelf) { ToggleColor(false); }
    }

    private void FixedUpdate()
    {
        if (!_myButton.enabled)
        { _myButton.interactable = isUnlocked; }
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
