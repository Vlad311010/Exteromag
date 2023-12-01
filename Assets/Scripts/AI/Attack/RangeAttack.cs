using Interfaces;
using Structs;
using UnityEngine;

public class RangeAttack : MonoBehaviour, IAttackAI
{
    public AICore core { get; set; }

    [Header("Parameters")]
    public SpellScriptableObject spell;
    public float attackRadius;
    public Vector2 attackBreak;
    [SerializeField] Transform target;


    [Header("Calculated")]
    private bool targetIsVisible;
    private float attackBreakTimer;
    private Coroutine attackCoroutine = null;


    private void Start()
    {
        ResetAttackBreakTimer();
    }

    private void ResetAttackBreakTimer()
    {
        attackBreakTimer = Random.Range(attackBreak.x, attackBreak.y);
    }

    public void AIUpdate()
    {
        targetIsVisible = AIGeneral.TargetIsVisible(transform.position, target, attackRadius, core.playerLayerMask | core.obstaclesLayerMask);
        if (targetIsVisible)
        {
            attackBreakTimer -= Time.deltaTime;
        }
        else
        {
            ResetAttackBreakTimer();
        }

        if (targetIsVisible && attackBreakTimer < 0f)
        {
            Attack();
        }
    }

    private void Attack()
    {
        Cast();
        ResetAttackBreakTimer();
    }


    private void Cast()
    {
        Vector2 castDirection = (target.transform.position - transform.position).normalized;
        AimSnapshot aimSnapshot = new AimSnapshot(target.transform.position, castDirection);
        SpellCasting.Cast(transform, spell, aimSnapshot);
    }

    void OnDrawGizmosSelected()
    {
        if (target == null) return; 

        Gizmos.color = targetIsVisible ? Color.green : Color.red;
        Gizmos.DrawLine(transform.position, target.position);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
