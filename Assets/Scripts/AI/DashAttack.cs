using Interfaces;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class DashAttack : MonoBehaviour, IAttackAI
{
    public float attackRange;
    public Vector2 attackBreak;
    public LayerMask layerMask;
    public float targetOvershoot;
    public float attackSpeed;
    public float attackAcceleration;

    //TODO: make attack preparation part. (moving slightli back of target). Add target lock 



    [SerializeField] Transform target;

    NavMeshAgent agent;
    Collider2D collider;
    Rigidbody2D rigidbody;


    private float defaultSpeed;
    private float defaultAcceleration;

    private bool targetIsVisible = false;
    private bool attackState = false;
    private bool cooldown = false;
    private Vector2 direction;
    private Vector2 afterAttackPos;

    private Coroutine cooldownCoroutine = null;

    private void Start()
    {
        agent = GetComponentInParent<NavMeshAgent>();
        collider = GetComponentInParent<Collider2D>();
        rigidbody = GetComponentInParent<Rigidbody2D>();

        defaultSpeed = agent.speed;
        defaultAcceleration = agent.acceleration;
    }

    public void AIUpdate()
    {
        targetIsVisible = AIGeneral.IsVisible(transform.position, target, attackRange, layerMask);
        direction = (target.transform.position - transform.position).normalized;

        if (!attackState && !cooldown && targetIsVisible)
        {
            Attack();
        }

        // if (cooldownCoroutine == null && attackState && Vector2.Distance(transform.position, afterAttackPos) < 0.5f)
        if (cooldownCoroutine == null && attackState && Vector2.Distance(transform.position, target.position) > attackRange + 1f)
        {
            rigidbody.velocity = Vector2.zero;
            attackState = false;
            cooldown = true;
            cooldownCoroutine = StartCoroutine(CoolDown());
        }

    }

    private void Attack()
    {
        afterAttackPos = (Vector2)target.transform.position + direction * targetOvershoot; ;
        collider.enabled = false;
        rigidbody.AddForce(direction * attackSpeed, ForceMode2D.Impulse);
        attackState = true;
        cooldown = true;
    }

    IEnumerator CoolDown()
    {
        float timer = Random.Range(attackBreak.x, attackBreak.y);
        yield return new WaitForSeconds(timer);
        cooldown = false;
        cooldownCoroutine = null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.DrawLine(transform.position, target.position + (Vector3)direction * targetOvershoot);
    }
}
