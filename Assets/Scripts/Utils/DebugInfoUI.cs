using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugInfoUI : MonoBehaviour
{
    bool showDebugInfo = true;

    private int currentMana;
    private int maxMana;

    private int currentHealth;
    private int maxHealth;

    private void OnEnable()
    {
        GameEvents.current.onManaChange += UpdateManaInfo;
        GameEvents.current.onHealthChange += UpdateHealthInfo;
    }

    private void OnDisable()
    {
        GameEvents.current.onManaChange -= UpdateManaInfo;
        GameEvents.current.onHealthChange -= UpdateHealthInfo;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) { showDebugInfo = !showDebugInfo; }
    }

    
    

    void OnGUI()
    {
        if (!showDebugInfo) return;

        

        float x = Screen.width - 200 - 20;
        GUILayout.BeginArea(new Rect(x, 20, 200, 200), new GUIStyle(GUI.skin.box));
        Status();
        GUILayout.EndArea();
    }

    void Status()
    {
        // GUILayout.Label("Selected spell: " + castScriptSnapshot.selectedSpell.name);
        GUILayout.Label(string.Format("Mana: {0}/{1}", currentMana, maxMana));
        GUILayout.Label(string.Format("LastTargetHp: {0}/{1}", currentHealth, maxHealth));
        // GUILayout.Label(string.Format("Shield: {0}/{1} | active:{2:on;2;OFF}", hpSystemSnapshot.currentShield, hpSystemSnapshot.maxShield, hpSystemSnapshot.shieldIsActive.GetHashCode()));
        // GUILayout.Label(string.Format("HP: {0}/{1}", hpSystemSnapshot.currentHP, hpSystemSnapshot.maxHP));
    }

    private void UpdateManaInfo(int current, int max)
    {
        currentMana = current;
        maxMana = max;
    }

    private void UpdateHealthInfo(int current, int max)
    {
        currentHealth = current;
        maxHealth = max;
    }
}
