using Interfaces;
using Structs;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RangeAttackRuning : MonoBehaviour, IAttackAI
{
    public AICore core { get; set; }

    [Header("Parameters")]
    public SpellScriptableObject spell;
    public float attackRange;
    public Vector2 attackBreak;
    public Vector2Int castsPerAttack;
    public Vector2 castsBreak;
    [SerializeField] Transform target;


    [Header("Calculated")]
    private bool targetIsVisible;
    private Coroutine attackCoroutine = null;
    private float attackBreakTimer;

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
        targetIsVisible = AIGeneral.TargetIsVisible(transform.position, target, attackRange, core.playerLayerMask | core.obstaclesLayerMask);
        if (targetIsVisible)
        {
            attackBreakTimer -= Time.deltaTime;
        }
        else
        {
            ResetAttackBreakTimer();
        }

        if (targetIsVisible && attackBreakTimer < 0f && attackCoroutine == null)
        {
            attackCoroutine = StartCoroutine(AttackCoroutine());
        }
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
    }

    private void Cast()
    {
        Vector2 castDirection = (target.transform.position - transform.position).normalized;
        AimSnapshot aimSnapshot = new AimSnapshot(target.transform.position, castDirection);
        SpellCasting.Cast(transform, spell, aimSnapshot);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = targetIsVisible ? Color.green : Color.red;
        Gizmos.DrawLine(transform.position, target.position);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
