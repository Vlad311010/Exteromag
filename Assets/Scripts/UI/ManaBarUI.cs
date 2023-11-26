using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaBarUI : MonoBehaviour
{
    [SerializeField] SpriteRenderer currentAmount;

    void Start()
    {
        GameEvents.current.onManaChange += UpdateManaBar;
    }

    private void UpdateManaBar(int current, int max)
    {
        float percent = (float)current / max;
        currentAmount.size = new Vector2(percent, 1);
    }
}
