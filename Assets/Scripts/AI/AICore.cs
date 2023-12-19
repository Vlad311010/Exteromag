using Interfaces;
using UnityEngine;
using UnityEngine.AI;

public class AICore : MonoBehaviour, IDestroyable
{
    AIEffects effects;
    public Animator animator;

    public NavMeshAgent agent;
    public int threatLevel;
    public Transform target;
    public IMoveAI moveAI { get; private set; }
    public IAttackAI attackAI { get; private set; }
    [SerializeField] public LayerMask playerLayerMask;
    [SerializeField] public LayerMask obstaclesLayerMask;

    [SerializeField] float visionRange = 50;
    
    [SerializeField] bool ignoreWalls = false;
    [SerializeField] bool moveAIActive = true;
    [SerializeField] bool attackAIActive = true;

    [HideInInspector] public bool canAttack = true;
    public bool damageOnCollision = false;
    private bool playerSpoted = false;


    void Awake()
    {
        effects = GetComponent<AIEffects>();
        animator = GetComponentInChildren<Animator>();
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

        if (!playerSpoted) return;

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

    /*private void OnDestroy()
    {
        GameEvents.current.EnemyDied();
    }*/

    public void SetTarget()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void SetGoToPoint(Vector2 goToPoint)
    {
        agent.destination = goToPoint;
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
