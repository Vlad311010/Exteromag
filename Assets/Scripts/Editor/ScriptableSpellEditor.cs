using UnityEngine;
using UnityEditor;
using System.Linq;
using System;
using Enums;
using System.Collections;

[CustomEditor(typeof(SpellScriptableObject))]
public class ScriptableSpellEditor : Editor
{
    private SpellScriptableObject spellSO;

    private void OnEnable()
    {
        spellSO = (SpellScriptableObject)target;
    }

    private string SpellAttributeToPropertyName(string attr)
    {
        string formatedAttr = Char.ToLower(attr[0]) + attr.Substring(1);
        return formatedAttr + "Attribute";
    }

    public override void OnInspectorGUI()
    {
        string[] excludedAttributes = Enum.GetNames(typeof(SpellAttribute)).Select(attr => SpellAttributeToPropertyName(attr)).ToArray();
        string[] excludedSpawnAttributes = Enum.GetNames(typeof(SpellSpawnAttribute)).Select(attr => SpellAttributeToPropertyName(attr)).ToArray();
        string[] excludedFields = new string[] { "attributes", "spawnAttributes", "targetSnapDistance", "castOnTarget" };


        serializedObject.Update();
        EditorGUI.BeginChangeCheck();

        // SerializedProperty castsOnTarget = serializedObject.FindProperty("castOnTarget");


        DrawPropertiesExcluding(serializedObject, excludedAttributes.Concat(excludedSpawnAttributes).Concat(excludedFields).ToArray());

        /* EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.PropertyField(castsOnTarget);
        }
        EditorGUILayout.EndHorizontal();

        if (castsOnTarget.boolValue)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("targetSnapDistance"));
        }*/

        // spell attributes
        EditorGUILayout.PropertyField(serializedObject.FindProperty("attributes")); // draw attributes list
        for (int i = 0; i < spellSO.attributes.Count; i++) // draw selected attributes data
        {
            string attributeName = Enum.GetName(typeof(SpellAttribute), spellSO.attributes[i]);
            EditorGUILayout.PropertyField(serializedObject.FindProperty(SpellAttributeToPropertyName(attributeName)));
        }

        // spawn attributes
        EditorGUILayout.PropertyField(serializedObject.FindProperty("spawnAttributes")); // draw attributes list
        for (int i = 0; i < spellSO.spawnAttributes.Count; i++) // draw selected attributes data
        {
            string attributeName = Enum.GetName(typeof(SpellSpawnAttribute), spellSO.spawnAttributes[i]);
            EditorGUILayout.PropertyField(serializedObject.FindProperty(SpellAttributeToPropertyName(attributeName)));
        }


        if (EditorGUI.EndChangeCheck())
            serializedObject.ApplyModifiedProperties();
    }
}