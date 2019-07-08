using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "New Settings", menuName = "ButtonAssetSettings")]
public class ButtonsSettings : ScriptableObject
{
    public Color32 normalColor;
    public Color32 highlightColor;
    public Color32 pressedColor;
    public Color32 selectedColor;

    public float colorMultiplier = 1;
    public float transitionTime = 0.1f;
    public FontStyles highlightFontStyle;


}
