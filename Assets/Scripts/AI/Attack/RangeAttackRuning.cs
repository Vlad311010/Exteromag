using Interfaces;
using Structs;
using System.Collections;
using UnityEngine;

public class RangeAttackRuning : MonoBehaviour, IAttackAI
{
    public AICore core { get; set; }

    [Header("Parameters")]
    public SpellScriptableObject spell;
    public float attackRange;
    public Vector2 attackBreak;
    public float multiAttackChance = 0.3f;
    public float multiAttackBreakTime = 0.15f;
    public Vector2Int castsPerAttack;
    public Vector2 castsBreak;


    [Header("Calculated")]
    private bool targetIsVisible;
    private Coroutine attackCoroutine = null;
    private float attackBreakTimer;
    private float currentMultiAttackChance;

    private void Start()
    {
        currentMultiAttackChance = multiAttackChance;
        ResetAttackBreakTimer();
    }

    private void ResetAttackBreakTimer()
    {
        if (Random.value < multiAttackChance)
        {
            attackBreakTimer = multiAttackBreakTime;
            currentMultiAttackChance = currentMultiAttackChance * 0.1f;
        }
        else
        {
            attackBreakTimer = Random.Range(attackBreak.x, attackBreak.y);
            currentMultiAttackChance = multiAttackChance;
        }
    }

    public void AIUpdate()
    {
        targetIsVisible = AIGeneral.TargetIsVisible(transform.position, core.target, attackRange, core.playerLayerMask | core.obstaclesLayerMask);
        AIGeneral.LookAt(core.transform, core.target);
        if (targetIsVisible)
        {
            attackBreakTimer -= Time.deltaTime;
        }
        /*else
        {
            ResetAttackBreakTimer();
        }*/

        core.canAttack = attackBreakTimer < 0f;
        if (targetIsVisible && core.canAttack && attackCoroutine == null)
        {
            // Attack();
            StartAttackAnimation();
        }
    }

    private void StartAttackAnimation()
    {
        core.animator.SetBool("Casting", true);
    }

    public void Attack()
    {
        attackCoroutine = StartCoroutine(AttackCoroutine());
    }

    IEnumerator AttackCoroutine()
    {
        int casts = Random.Range(castsPerAttack.x, castsPerAttack.y);
        for (int i = 0; i < casts; i++)
        {
            Cast();
            float timer = Random.Range(castsBreak.x, castsBreak.y);
            yield return new WaitForSeconds(timer);
        }
        attackCoroutine = null;
        ResetAttackBreakTimer();
        core.animator.SetBool("Casting", false);
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
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
