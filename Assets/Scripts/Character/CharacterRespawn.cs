using Interfaces;
using UnityEngine;

public class CharacterRespawn : MonoBehaviour
{
    CharacterLimitations limitations;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        limitations = GetComponent<CharacterLimitations>();
    }

    public void Respawn()
    {
        limitations.ActivatePlayer();
        ResetStats();
    }

    public void ResetStats()
    {
        foreach (IResatable resatable in GetComponents<IResatable>())
        {
            resatable.ResetValues();
        }
    }
}
