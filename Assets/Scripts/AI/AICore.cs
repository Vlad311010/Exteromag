using Interfaces;
using UnityEngine;
using UnityEngine.AI;

public class AICore : MonoBehaviour, IDestroyable
{
    AIEffects effects;
    public Animator animator { get; private set; }
    AIDisable disable;

    public NavMeshAgent agent;
    public int threatLevel;
    public Transform target;

    public IMoveAI moveAI { get; private set; }
    public IAttackAI attackAI { get; private set; }
    [SerializeField] public LayerMask playerLayerMask;
    [SerializeField] public LayerMask obstaclesLayerMask;

    [SerializeField] float visionRange = 50;
    
    [SerializeField] bool ignoreWalls = false;
    [SerializeField] public bool moveAIActive = true;
    [SerializeField] public bool attackAIActive = true;

    [HideInInspector] public bool canAttack = true;
    public bool damageOnCollision = false;
    private bool playerSpoted = false;

    public Vector2 movementDirection { get; private set; }

    void Awake()
    {
        effects = GetComponent<AIEffects>();
        animator = GetComponentInChildren<Animator>();
        disable = GetComponent<AIDisable>();
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
        LayerMask visionLayerMask = ignoreWalls ? playerLayerMask : playerLayerMask | obstaclesLayerMask;
        playerSpoted = playerSpoted || AIGeneral.TargetIsVisible(transform.position, target, visionRange, visionLayerMask);
        movementDirection = (transform.position - agent.nextPosition).normalized;

        if (!playerSpoted)
        {
            AIGeneral.LookAt(transform, agent.nextPosition);
            return;
        }

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
        GameEvents.current.EnemyKilled();
        // GameEvents.current.EnemyDied();
        disable.Disable();
    }

    public void SetTarget()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void OnDamageRecive()
    {
        playerSpoted = true;
    }

    public void SetGoToPoint(Vector2 goToPoint)
    {
        agent.destination = goToPoint;
        AIGeneral.LookAt(transform, goToPoint);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (damageOnCollision && collider.TryGetComponent(out IHealthSystem healthSystem))
        {
            healthSystem.ConsumeHp(1, Vector2.zero);
        }
    }


}
