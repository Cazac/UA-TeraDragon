using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using TMPro;

public class MenuButtonToggles : Selectable, IPointerClickHandler
{
    private TextMeshProUGUI _targetGraphic;
    private ColorBlock _myColor;

    public UnityEvent onClick;
    public ButtonsSettings customColorSettings;

    void Start()
    {
        //set the target graphic automatically, has to be 2nd child 
        _targetGraphic = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        this.targetGraphic = _targetGraphic;

        //set the colors from the scriptable object
        _myColor.normalColor = customColorSettings.normalColor;
        _myColor.highlightedColor = customColorSettings.highlightColor;
        _myColor.pressedColor = customColorSettings.pressedColor;
        _myColor.selectedColor = customColorSettings.selectedColor;
        _myColor.colorMultiplier = customColorSettings.colorMultiplier;
        _myColor.fadeDuration = customColorSettings.transitionTime;

        //finally set the colors to be used from the scriptable object
        this.colors = _myColor;
    }

    public void OnPointerClick(PointerEventData eventData) {
        onClick.Invoke();
    }
}
