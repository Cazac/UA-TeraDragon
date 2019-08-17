using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class MenuButtonsBehaviour : Toggle
{
    private RectTransform _thisTransform;

    //Vectors that I need
    private Vector3 _initPosition;
    public Vector3 _targetPosition = new Vector3(20, 0, 0);
    private Vector3 _finalPosition;

    public float offsetSpeed;
    public ColorBlock myColorBlock;
    public SpriteState mySprites;

    public UnityEvent onEvent;
    public UnityEvent myEvent;

    void Start()
    {
        _thisTransform = GetComponent<RectTransform>();
        _initPosition = _thisTransform.anchoredPosition3D;
        _finalPosition = _initPosition + _targetPosition;

        colors = myColorBlock;
        spriteState = mySprites;

    }

    void Update()
    {
        if (IsHighlighted())
        {
            _thisTransform.anchoredPosition3D = Vector3.Lerp(_thisTransform.anchoredPosition3D, _finalPosition, Time.deltaTime * offsetSpeed);
        }

        else if (IsPressed())
        {
            _thisTransform.anchoredPosition3D = Vector3.Lerp(_thisTransform.anchoredPosition3D, _finalPosition, Time.deltaTime * offsetSpeed);
        }

        else if (isOn)
        {
            interactable = false;
            _thisTransform.anchoredPosition3D = Vector3.Lerp(_thisTransform.anchoredPosition3D, _finalPosition, Time.deltaTime * offsetSpeed);
        }

        else
        {
            interactable = true;
            myColorBlock.normalColor = Color.white;
            _thisTransform.anchoredPosition3D = Vector3.Lerp(_thisTransform.anchoredPosition3D, _initPosition, Time.deltaTime * offsetSpeed);
        }
    }

}
