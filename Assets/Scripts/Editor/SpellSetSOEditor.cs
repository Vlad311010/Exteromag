using UnityEditor;
using UnityEngine;
using static SpellSetSO;

[CustomEditor(typeof(SpellSetSO))]
public class SpellSetSOEditor : Editor
{
    private SpellSetSO _target;
    const int pixelsPerIndentLevel = 20;
    private bool showDescription;

    private void OnEnable()
    {
        _target = (SpellSetSO)target;
    }

    public override void OnInspectorGUI()
    {
        showDescription = GUILayout.Toggle(showDescription, "Show Description");

        SpellUpgradeNode root = _target.root;
        Display(root);

        if (GUILayout.Button("Clear"))
        {
            root.children.Clear();
            root.spell = null;
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(_target);
            AssetDatabase.SaveAssets();
        }
        
    }

    private void Display(SpellUpgradeNode node, int indentLevel = 0)
    {
        indentLevel++;

        GUILayout.BeginHorizontal();
        if (showDescription)
            node.description = EditorGUILayout.TextField(node.description);
        else
            node.spell = (SpellScriptableObject)EditorGUILayout.ObjectField(node.spell, typeof(SpellScriptableObject));

        if (GUILayout.Button("+", GUILayout.MaxWidth(30)))
        {
            node.Add(new SpellUpgradeNode());
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Space(pixelsPerIndentLevel * indentLevel);
        GUILayout.BeginVertical();
        foreach (SpellUpgradeNode child in node.children)
        {
            Display(child, indentLevel);
        }

        GUILayout.EndVertical();
        GUILayout.EndHorizontal();

        // horizontal view
        /*GUILayout.BeginHorizontal();
        node.spell = (SpellScriptableObject)EditorGUILayout.ObjectField(node.spell, typeof(SpellScriptableObject));
        if (GUILayout.Button("+", GUILayout.MaxWidth(30)))
        {
            node.Add(new SpellUpgradeNode());
        }
        GUILayout.Space(pixelsPerIndentLevel * indentLevel);
        GUILayout.BeginVertical();
        foreach (SpellUpgradeNode child in node.children)
        {
            Display(child, indentLevel);
        }

        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        */
    }
}
