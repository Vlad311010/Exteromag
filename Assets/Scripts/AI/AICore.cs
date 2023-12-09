using Interfaces;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AICore : MonoBehaviour, IDestroyable
{
    AIEffects effects;

    public NavMeshAgent agent;
    public int threatLevel;
    public Transform target;
    [SerializeField] IMoveAI moveAI;
    [SerializeField] IAttackAI attackAI;
    [SerializeField] public LayerMask playerLayerMask;
    [SerializeField] public LayerMask obstaclesLayerMask;

    [SerializeField] bool moveAIActive = true;
    [SerializeField] bool attackAIActive = true;

    [HideInInspector] public bool canAttack = true;
    public bool damageOnCollision = false;


    void Awake()
    {
        effects = GetComponent<AIEffects>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateUpAxis = false;
        agent.updateRotation = false;


        UpdateAIComponents();

        SetTarget();
    }

    public void UpdateAIComponents()
    {
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
        // GameEvents.current.EnemyDied();
        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        GameEvents.current.EnemyDied();
    }

    public void SetTarget()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void OnHit()
    {
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (damageOnCollision && collider.TryGetComponent(out IHealthSystem healthSystem))
        {
            healthSystem.ConsumeHp(1, Vector2.zero);
        }
    }


}
