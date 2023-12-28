using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CharacterStats", order = 3)]
public class CharacterStatsSO : ScriptableObject
{
    public int hp;
    public int mana;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        hp = player.GetComponent<CharacterHealthSystem>().MaxHealth;
        mana = player.GetComponent<ManaPool>().MaxMp;
    }

}
