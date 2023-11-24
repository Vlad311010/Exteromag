using Interfaces;
using UnityEngine;
using UnityEngine.AI;

public class AICore : MonoBehaviour, IDestroyable
{
    public NavMeshAgent agent;
    [SerializeField] IMoveAI moveAI;
    [SerializeField] IAttackAI attackAI;

    [SerializeField] bool moveAIActive = true;
    [SerializeField] bool attackAIActive = true;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateUpAxis = false;
        agent.updateRotation = false;


        moveAI = GetComponentInChildren<IMoveAI>();
        attackAI = GetComponentInChildren<IAttackAI>();
    }

    void Update()
    {
        if (moveAIActive)
            moveAI.AIUpdate();
        if (attackAIActive)
            attackAI.AIUpdate();
    }


    public void moveAISetActive(bool active)
    {
        if (active)
            moveAI.AIReset();

        moveAIActive = active;
    }

    public void DestroyObject()
    {
        Destroy(this.gameObject);
    }
}
