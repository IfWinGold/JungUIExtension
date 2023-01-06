using System.Collections;
using System.Collections.Generic;
using JungExtension.UI;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(ExtensionToggleSwitch))]
public class ToggleSwitchEditor : Editor
{
    public ExtensionToggleSwitch toggleSwitch;
    MonoScript script = null;

    private SerializedProperty _bgImage;
    private void OnEnable()
    {
        toggleSwitch = target as ExtensionToggleSwitch;
        if(!toggleSwitch.HasChild())
        {            
            toggleSwitch.CreateChilds();
        }                
    }
    public override void OnInspectorGUI()
    {
        using (new EditorGUI.DisabledScope(true))
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((MonoBehaviour)target), GetType(), false);

        toggleSwitch.m_SettingState = (ExtensionToggleSwitch.SettingState)EditorGUILayout.EnumPopup("SettingState", toggleSwitch.m_SettingState);
        toggleSwitch.m_UseChangeColor = EditorGUILayout.Toggle(new GUIContent("UsingChangeColor", "Switch 작동시 컬러값을 변경합니다."), toggleSwitch.m_UseChangeColor);

        switch (toggleSwitch.m_SettingState)
        {
            case ExtensionToggleSwitch.SettingState.DefaultSetting:
                {                    
                    //toggleSwitch.SetChangeDefaultSetting();
                }
                break;
            case ExtensionToggleSwitch.SettingState.ChangeSprite:
                {                    

                    GUILayout.Space(10);                                        

                    GUILayout.Space(10);
                    GUILayout.Label("배경이미지");                    
                    toggleSwitch.BackGroundOffSprite=(Sprite)EditorGUILayout.ObjectField("배경 Off",toggleSwitch.BackGroundOffSprite, typeof(Sprite), false, GUILayout.Height(EditorGUIUtility.singleLineHeight));
                    
                    toggleSwitch.BackGroundOnSprite = (Sprite)EditorGUILayout.ObjectField("배경 On", toggleSwitch.BackGroundOnSprite, typeof(Sprite), false, GUILayout.Height(EditorGUIUtility.singleLineHeight));
                    GUILayout.Space(10);


                    GUILayout.Label("핸들");                    
                    toggleSwitch.HandleOffSprite = (Sprite)EditorGUILayout.ObjectField("핸들 Off", toggleSwitch.HandleOffSprite, typeof(Sprite), false, GUILayout.Height(EditorGUIUtility.singleLineHeight));
                    
                    toggleSwitch.HandleOnSprite = (Sprite)EditorGUILayout.ObjectField("핸들 On", toggleSwitch.HandleOnSprite, typeof(Sprite), false, GUILayout.Height(EditorGUIUtility.singleLineHeight));


                    //toggleSwitch.SetChangeSpriteSetting();
                }
                break;        
        }

        if (toggleSwitch.m_UseChangeColor)
        {
            GUILayout.Space(10);
            GUILayout.Label("컬러");

            toggleSwitch.HandleOffColor = EditorGUILayout.ColorField("Handle컬러 Off", toggleSwitch.HandleOffColor, GUILayout.Height(EditorGUIUtility.singleLineHeight));

            toggleSwitch.HandleOnColor = EditorGUILayout.ColorField("Handle컬러On", toggleSwitch.HandleOnColor, GUILayout.Height(EditorGUIUtility.singleLineHeight));

            toggleSwitch.BackGroundOffColor = EditorGUILayout.ColorField("BG컬러 Off", toggleSwitch.BackGroundOffColor, GUILayout.Height(EditorGUIUtility.singleLineHeight));

            toggleSwitch.BackGroundOnColor = EditorGUILayout.ColorField("BG컬러On", toggleSwitch.BackGroundOnColor, GUILayout.Height(EditorGUIUtility.singleLineHeight));
        }


        if (GUILayout.Button("On"))
        {            
            toggleSwitch.SetActiveHandle(true);
        }
        if (GUILayout.Button("Off"))
        {            
            toggleSwitch.SetActiveHandle(false);
        }
        if (GUILayout.Button("Debug"))
        {
            toggleSwitch.OnClickDebugButton();
        }
        if (GUI.changed)
            EditorUtility.SetDirty(target);
    }
}
#endif