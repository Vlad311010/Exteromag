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
        targetIsVisible = AIGeneral.TargetIsVisible(transform.position, core.target, attackRadius, core.playerLayerMask | core.obstaclesLayerMask);
        if (targetIsVisible)
        {
            attackBreakTimer -= Time.deltaTime;
            AIGeneral.LookAt(core.transform, core.target.transform);
        }
        else
        {
            ResetAttackBreakTimer();
        }

        core.canAttack = attackBreakTimer < 0f;
        if (targetIsVisible && core.canAttack)
        {
            StartAttackAnimation();
        }
    }

    public void StartAttackAnimation()
    {
        core.animator.SetBool("Casting", true);
    }

    public void Attack()
    {
        core.animator.SetBool("Casting", false);
        Cast();
        ResetAttackBreakTimer();
    }


    private void Cast()
    {
        Vector2 castDirection = (core.target.transform.position - transform.position).normalized;
        AimSnapshot aimSnapshot = new AimSnapshot(core.target.transform.position, castDirection);
        SpellCasting.Cast(transform, spell, aimSnapshot);
    }

    void OnDrawGizmosSelected()
    {
        if (core == null) return; 

        Gizmos.color = targetIsVisible ? Color.green : Color.red;
        Gizmos.DrawLine(transform.position, core.target.position);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
