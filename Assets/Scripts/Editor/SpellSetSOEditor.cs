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
        
        if (GUILayout.Button(string.Format("Langage Switch ({0})", Translator.selectedLangage.ToString())))
        {
            if (Translator.selectedLangage == SystemLanguage.English)
                Translator.selectedLangage = SystemLanguage.Ukrainian;
            else
                Translator.selectedLangage = SystemLanguage.English;
        }


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
        {
            // node.description = EditorGUILayout.TextField(node.description);
            if (node.translation == null || node.translation.Length == 0)
                node.translation = new Translator.Translation[] { new Translator.Translation(SystemLanguage.English), new Translator.Translation(SystemLanguage.Ukrainian) };

            int translationIndex = node.GetDescriptionTranslationIndex(Translator.selectedLangage);             
            node.translation[translationIndex].text = EditorGUILayout.TextField(node.translation[translationIndex].text);
        }
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

        /*for (int i = 0; i < node.translation.Length; i++)
        {
            string translationText = EditorGUILayout.TextField(node.translation[i].text);
            node.translation[i] = new Translator.Translation(node.translation[i], translationText);
        }*/

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
