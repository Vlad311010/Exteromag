using Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class DashAttack : MonoBehaviour, IAttackAI
{
    public float attackRange;
    public Vector2 attackBreak;
    public Vector2 afterAttackBreak;
    public LayerMask layerMask;
    public float targetOvershoot;
    public float attackSpeed;
    public float attackAcceleration;



    public AICore core { get; set; }
    NavMeshAgent agent;
    Collider2D collider;
    Rigidbody2D rigidbody;


    private bool targetIsVisible = false;
    private bool attackState = false;
    private Vector2 direction;
    private Vector2 dashStartPoint = Vector2.zero;
    private float currentDashDistance;
    
    private float defaultMovementSpeed;
    private float attackPreparationMovementSpeed = 1;

    // private List<GameObject> damagedObjects = new List<GameObject>();

    private Coroutine cooldownCoroutine = null;

    private void Start()
    {
        agent = GetComponentInParent<NavMeshAgent>();
        collider = GetComponentInParent<Collider2D>();
        rigidbody = GetComponentInParent<Rigidbody2D>();
        currentDashDistance = targetOvershoot;
        defaultMovementSpeed = agent.speed;
    }

    public void AIUpdate()
    {
        targetIsVisible = AIGeneral.TargetIsVisible(transform.position, core.target, attackRange, core.playerLayerMask | core.obstaclesLayerMask);
        direction = (core.target.transform.position - transform.position).normalized;
        
        if (!attackState && core.target != null)
            AIGeneral.LookAt(core.transform, core.target);

        if (!attackState && core.canAttack && targetIsVisible)
        {
            StartAttackAnimation();
        }

        // if (cooldownCoroutine == null && attackState && Vector2.Distance(transform.position, core.target.position) > attackRange + 1f)
        if (cooldownCoroutine == null && attackState && (Vector2.Distance(transform.position, dashStartPoint) > currentDashDistance || rigidbody.velocity.magnitude < 2))
        {
            rigidbody.velocity = Vector2.zero;
            collider.isTrigger = false;
            core.damageOnCollision = false;
            attackState = false;
            core.canAttack = false;
            core.animator.SetBool("Dash", false);
            agent.speed = defaultMovementSpeed;
            cooldownCoroutine = StartCoroutine(CoolDown());
        }

    }

    private void StartAttackAnimation()
    {
        core.animator.SetBool("Dash", true);
        agent.speed = attackPreparationMovementSpeed;
    }

    public void Attack()
    {
        // damagedObjects.Clear();
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, targetOvershoot, core.obstaclesLayerMask);
        currentDashDistance = hit ? hit.distance - 1f : targetOvershoot;
        dashStartPoint = transform.position;
        collider.isTrigger = true;
        core.damageOnCollision = true;
        rigidbody.AddForce(direction * attackSpeed, ForceMode2D.Impulse);
        attackState = true;
        core.canAttack = false;
        GetComponentInParent<AICore>().moveAISetActive(false);
        core.agent.enabled = false;
    }

    IEnumerator CoolDown()
    { 
        
        float afterAttackBreakTimer = Random.Range(afterAttackBreak.x, afterAttackBreak.y);
        yield return new WaitForSeconds(afterAttackBreakTimer);
        core.agent.enabled = true;
        GetComponentInParent<AICore>().moveAISetActive(true);

        float attackCoolDownTimer = Random.Range(attackBreak.x, attackBreak.y);
        yield return new WaitForSeconds(attackCoolDownTimer);
        core.canAttack = true;
        attackState = false;
        cooldownCoroutine = null;
        
    }

    

    private void OnDrawGizmosSelected()
    {
        #if UNITY_EDITOR
            if (!EditorApplication.isPlaying) return;
        #endif

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.DrawLine(transform.position, core.target.position + (Vector3)direction * currentDashDistance);
    }
}
