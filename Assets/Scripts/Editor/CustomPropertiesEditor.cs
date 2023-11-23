using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

public static class CustomEditorUtilsUtils
{
    public static Rect IncreaseHight(float offset, Rect position, ref float propertyHeight)
    {
        position.y += offset;
        propertyHeight += offset;
        return position;
    }

    public static Rect DefaultHeightStep(Rect position, ref float propertyHeight)
    {
        position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        propertyHeight += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        return position;
    }

    public static Rect DrawProperty(Rect position, SerializedProperty property, string propertyName, ref float propertyHeight)
    {
        position = CustomEditorUtilsUtils.DefaultHeightStep(position, ref propertyHeight);
        SerializedProperty propertyToDraw = property.FindPropertyRelative(propertyName);
        EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), propertyToDraw);
        return position;
    }

    public static void DrawProperties(SerializedProperty prop, bool drawChildren)
    {
        string lastPropPath = string.Empty;
        foreach (SerializedProperty p in prop)
        {
            if (p.isArray && p.propertyType == SerializedPropertyType.Generic)
            {
                EditorGUILayout.BeginHorizontal();
                p.isExpanded = EditorGUILayout.Foldout(p.isExpanded, p.displayName);
                EditorGUILayout.EndHorizontal();

                if (p.isExpanded)
                {
                    EditorGUI.indentLevel++;
                    DrawProperties(p, drawChildren);
                    EditorGUI.indentLevel--;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(lastPropPath) && p.propertyPath.Contains(lastPropPath)) { continue; } 
                lastPropPath = p.propertyPath;
                // EditorGUI.PropertyField(new Rect(1, 1, 100, 100), p);
                EditorGUILayout.PropertyField(p, drawChildren);
            }
        }
    }
}


public class AutomaticPropertyDrawer : PropertyDrawer
{
    protected float propertyHeight;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        propertyHeight = EditorGUIUtility.singleLineHeight;
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        EditorGUI.LabelField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), label, EditorStyles.boldLabel);

        CustomEditorUtilsUtils.DrawProperties(property, true);

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return this.propertyHeight;
    }
}


// Attributes
[CustomPropertyDrawer(typeof(ExplosionAttributeData))]
public class ExplosionAttributeDataEditor : AutomaticPropertyDrawer { }

[CustomPropertyDrawer(typeof(InstantAttributeData))]
public class InstantAttributeDataEditor : AutomaticPropertyDrawer { }

[CustomPropertyDrawer(typeof(BounceAttributeData))]
public class BounceAttributeDataDataEditor : AutomaticPropertyDrawer { }

[CustomPropertyDrawer(typeof(RotateAroundTargetAttributeData))]
public class RotateAroundTargetAttributeDataEditor : AutomaticPropertyDrawer { }

// Spawn atttributes
[CustomPropertyDrawer(typeof(DefaultSpawnAttributeData))]
public class DefaultSpawnAttributeDataEditor : AutomaticPropertyDrawer { }

[CustomPropertyDrawer(typeof(RandomOffsetAttributeData))]
public class RandomOffsetDataEditor : AutomaticPropertyDrawer { }

[CustomPropertyDrawer(typeof(ShotgunAttributeData))]
public class ShotgunSpawnAttributeDataEditor : AutomaticPropertyDrawer { }


