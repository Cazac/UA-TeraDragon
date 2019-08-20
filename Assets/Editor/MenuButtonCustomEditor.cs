using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.Events;


[CustomEditor(typeof(MenuButtonsBehaviour))]
public class MenuButtonCustomEditor : UnityEditor.UI.ToggleEditor
{
    public static MenuButtonsBehaviour button;
    public float something = 1;


    //----------------//
    //CUSTOM INSPECTOR//
    //----------------//
    public override void OnInspectorGUI()
    {
        button = (MenuButtonsBehaviour)target;
        //base.OnInspectorGUI();



        RenderDefaultSettings();
        RenderTransitionBox();
        RenderCustomVariables();
        RenderUnityEvent();
    }

    /// <summary>
    /// Renders the color block.
    /// </summary>
    public void RenderColorBlock()
    {

        EditorGUILayout.BeginVertical();

        button.myColorBlock.colorMultiplier = EditorGUILayout.Slider("Color Multiplier", button.myColorBlock.colorMultiplier, 1, 5);
        button.myColorBlock.fadeDuration = EditorGUILayout.Slider("Fade Duration", button.myColorBlock.fadeDuration, 0, 2);

        button.myColorBlock.normalColor = EditorGUILayout.ColorField("Normal Color", button.myColorBlock.normalColor);
        button.myColorBlock.highlightedColor = EditorGUILayout.ColorField("Highlighted Color", button.myColorBlock.highlightedColor);
        button.myColorBlock.pressedColor = EditorGUILayout.ColorField("Pressed Color", button.myColorBlock.pressedColor);
        button.myColorBlock.selectedColor = EditorGUILayout.ColorField("Selected Color", button.myColorBlock.selectedColor);
        button.myColorBlock.disabledColor = EditorGUILayout.ColorField("Disabled Color", button.myColorBlock.disabledColor);

        EditorGUILayout.EndVertical();

    }

    /// <summary>
    /// Renders the sprite swap.
    /// </summary>
    public void RenderSpriteSwap()
    {
        EditorGUILayout.BeginVertical();

        button.mySprites.highlightedSprite = (Sprite)EditorGUILayout.ObjectField("Highlighted Sprite", button.mySprites.highlightedSprite, typeof(Sprite), true);
        button.mySprites.pressedSprite = (Sprite)EditorGUILayout.ObjectField("Pressed Sprite", button.mySprites.pressedSprite, typeof(Sprite), true);
        button.mySprites.selectedSprite = (Sprite)EditorGUILayout.ObjectField("Selected Sprite", button.mySprites.selectedSprite, typeof(Sprite), true);
        button.mySprites.disabledSprite = (Sprite)EditorGUILayout.ObjectField("Disabled Sprite", button.mySprites.disabledSprite, typeof(Sprite), true);


        EditorGUILayout.EndVertical();
    }

    /// <summary>
    /// Renders the transition box.
    /// </summary>
    public void RenderTransitionBox()
    {
        EditorGUILayout.BeginVertical("box");
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Transition Type", EditorStyles.boldLabel);
            button.transition = (Selectable.Transition)EditorGUILayout.EnumPopup(button.transition);
            EditorGUILayout.EndHorizontal();

            switch (button.transition)
            {

                case Selectable.Transition.ColorTint:
                    EditorGUILayout.Space();
                    RenderColorBlock();
                    break;

                case Selectable.Transition.SpriteSwap:
                    EditorGUILayout.Space();
                    RenderSpriteSwap();
                    break;
            }
        }
        EditorGUILayout.EndVertical();
    }

    /// <summary>
    /// Renders the default settings.
    /// </summary>
    public void RenderDefaultSettings()
    {
        //label
        EditorGUILayout.BeginVertical("box");

        EditorGUILayout.LabelField("Toggle Settings", EditorStyles.boldLabel);

        button.interactable = EditorGUILayout.Toggle("Interactable", button.interactable);
        button.isOn = EditorGUILayout.Toggle("Is the button On?", button.isOn);
        button.targetGraphic = (Graphic)EditorGUILayout.ObjectField("Target Graphic", button.targetGraphic, typeof(Object), true);
        button.group = (ToggleGroup)EditorGUILayout.ObjectField("Toggle Group", button.group, typeof(Object), true);

        EditorGUILayout.EndVertical();
    }

    /// <summary>
    /// Renders the custom variables.
    /// </summary>
    public void RenderCustomVariables()
    {
        EditorGUILayout.BeginVertical("box");

        EditorGUILayout.LabelField("Custom Variables", EditorStyles.boldLabel);
        button.offsetSpeed = EditorGUILayout.Slider("Offset speed", button.offsetSpeed, 1, 20);
        button._targetPosition = EditorGUILayout.Vector3Field("Target Position Offset", button._targetPosition);

        EditorGUILayout.EndVertical();
    }

    /// <summary>
    /// Renders the unity event.
    /// </summary>
    public void RenderUnityEvent() 
    {
        EditorGUILayout.BeginVertical("box");

        this.serializedObject.Update();
        EditorGUILayout.PropertyField(this.serializedObject.FindProperty("onValueChanged"), true);
        this.serializedObject.ApplyModifiedProperties();

        EditorGUILayout.EndVertical();
    }
}
