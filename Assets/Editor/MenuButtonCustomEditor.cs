using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor(typeof(MenuButtonsBehaviour))]
public class MenuButtonCustomEditor : UnityEditor.UI.ToggleEditor
{
    public static MenuButtonsBehaviour button;

    //----------------//
    //CUSTOM INSPECTOR//
    //----------------//
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        button = (MenuButtonsBehaviour)target;

        //EditorGUILayout.BeginVertical("box");

        //RenderDefaultSettings();
        //RenderTransitionBox();

        //EditorGUILayout.EndVertical();

        RenderCustomVariables();
    }

    //---------------------//
    //TRANSITION BOX RENDER//
    //---------------------//
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

    public void RenderSpriteSwap()
    {
        EditorGUILayout.BeginVertical();

        button.mySprites.highlightedSprite = (Sprite)EditorGUILayout.ObjectField("Highlighted Sprite", button.mySprites.highlightedSprite, typeof(Sprite), true);
        button.mySprites.pressedSprite = (Sprite)EditorGUILayout.ObjectField("Pressed Sprite", button.mySprites.pressedSprite, typeof(Sprite), true);
        button.mySprites.selectedSprite = (Sprite)EditorGUILayout.ObjectField("Selected Sprite", button.mySprites.selectedSprite, typeof(Sprite), true);

        EditorGUILayout.EndVertical();
    }

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

    public void RenderDefaultSettings()
    {
        //label
        EditorGUILayout.LabelField("Toggle Settings", EditorStyles.boldLabel);

        button.interactable = EditorGUILayout.Toggle("Interactable", button.interactable);
        button.isOn = EditorGUILayout.Toggle("Is the button On?", button.isOn);
        button.targetGraphic = (Graphic)EditorGUILayout.ObjectField("Target Graphic", button.targetGraphic, typeof(Object), true);
        button.group = (ToggleGroup)EditorGUILayout.ObjectField("Toggle Group", button.group, typeof(Object), true);
    }

    public void RenderCustomVariables()
    {
        EditorGUILayout.BeginVertical("box");

        EditorGUILayout.LabelField("Custom Variables", EditorStyles.boldLabel);
        button.offsetSpeed = EditorGUILayout.Slider("Offset speed", button.offsetSpeed, 1, 5);
        button._targetPosition = EditorGUILayout.Vector3Field("Target Position Offset", button._targetPosition);

        EditorGUILayout.EndVertical();
    }
}
