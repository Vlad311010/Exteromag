using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(CharacterLimitations))]
public class CharacterLimitationsEditor : Editor
{
    private CharacterLimitations _target;
    private bool infiniteManaSwitch = false;
    private void OnEnable()
    {
        _target = (CharacterLimitations)target;
    }

    public override void OnInspectorGUI()
    {

        DrawDefaultInspector();

        if (GUILayout.Button("Infinite mana"))
        {
            infiniteManaSwitch = !infiniteManaSwitch;
            _target.InfiniteMana(infiniteManaSwitch);
        }
    }

}
