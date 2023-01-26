using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreenButton : MonoBehaviour
{
    [SerializeField] private Color _colorOff, _colorOn;
    public bool isUnlocked = true;
    private Button _myButton;

    private TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        if (gameObject.activeSelf) { ToggleColor(false); }
        StartCoroutine(CheckButton());
    }

    private IEnumerator CheckButton()
    {
        _myButton = GetComponent<Button>();
        yield return new WaitForSecondsRealtime(0.1f);
        _myButton.interactable = isUnlocked; 
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
