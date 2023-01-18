using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] private Color _colorOff, _colorOn;

    private TextMeshProUGUI _text;

    private void Start()
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void ToggleColor(bool value)
    {
        switch (value)
        {
            case true: _text.color = _colorOn; break;
            case false: _text.color = _colorOff; break;
        }
    }
}
