using UnityEngine;

public class AIAnimations : MonoBehaviour
{
    AICore core;
    void Start()
    {
        core = GetComponentInParent<AICore>();
    }

    public void Attack()
    {
        core.attackAI.Attack();
    }
}
