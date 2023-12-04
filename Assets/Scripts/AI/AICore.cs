using Interfaces;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AICore : MonoBehaviour, IDestroyable
{
    AIEffects effects;

    public NavMeshAgent agent;
    public int threatLevel;
    [SerializeField] IMoveAI moveAI;
    [SerializeField] IAttackAI attackAI;
    [SerializeField] public LayerMask playerLayerMask;
    [SerializeField] public LayerMask obstaclesLayerMask;

    [SerializeField] bool moveAIActive = true;
    [SerializeField] bool attackAIActive = true;


    


    void Awake()
    {
        effects = GetComponent<AIEffects>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateUpAxis = false;
        agent.updateRotation = false;


        moveAI = GetComponentInChildren<IMoveAI>();
        attackAI = GetComponentInChildren<IAttackAI>();
        moveAI.core = this;
        attackAI.core = this;
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
        {
            moveAI.AIReset();
            agent.isStopped = false;    
        }
        else
        {
            agent.isStopped = true;
        }

        moveAIActive = active;
    }

    public void DestroyObject()
    {
        effects.OnDeath();
        GameEvents.current.EnemyDied();
        Destroy(this.gameObject);
    }

    public void OnHit()
    {
    }
    
}
