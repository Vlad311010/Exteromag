using Interfaces;
using Structs;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

public class MeleeAttack : MonoBehaviour, IAttackAI
{
    public AICore core { get; set; }

    [SerializeField] int damage;
    [SerializeField] float attackRadius;
    [SerializeField] float attackRange;
    [SerializeField] float attackCooldownTime;
    [SerializeField] float attackTick;
    [SerializeField] float tickPerAttack;
    [SerializeField] LayerMask attackLayerMask;
    // [SerializeField] GameObject attackTrail;
    [SerializeField] VisualEffect attackEffect;
    public bool stopAfterAttack = false;
    public float stopAfterAttackTime;

    Vector2 directionToTarget;
    private bool targetInAttackRange;


    public void AIUpdate()
    {
        
        targetInAttackRange = AIGeneral.IsInsideVisionCon(core.target.position, core.transform.position, core.transform.up, attackRange, attackRadius);
        if (targetInAttackRange && core.canAttack)
        {
            // Attack();
            StartAttackAnimation();
        }
    }

    private void StartAttackAnimation()
    {
        core.animator.SetBool("Bash", true);
    }

    public void Attack()
    {
        core.canAttack = false;
        attackEffect.Play();
        foreach (Collider2D collider in ObjectsToDamage())
        {
            directionToTarget = (collider.transform.position - transform.position).normalized;
            collider.GetComponent<IHealthSystem>().ConsumeHp(damage, directionToTarget);
        }
        StartCoroutine(Cooldown());
        StartCoroutine(TurnOffMovement());
        core.animator.SetBool("Bash", false);
    }
    private Collider2D[] ObjectsToDamage()
    {
        Collider2D[] possibleHits = Physics2D.OverlapCircleAll(transform.position, attackRadius, attackLayerMask);
        Collider2D[] hits = possibleHits
            .Where(hit => AIGeneral.IsInsideVisionCon(hit.transform.position, transform.position, core.transform.up, attackRange, attackRadius))
            .Where(hit => hit.TryGetComponent(out IHealthSystem _))
            .ToArray();

        return hits;

    } 

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(attackCooldownTime);
        core.canAttack = true;
    }

    IEnumerator TurnOffMovement()
    {
        core.moveAISetActive(false);
        yield return new WaitForSeconds(stopAfterAttackTime);
        core.moveAISetActive(true);
    }

    private void OnDrawGizmosSelected()
    {
        if (core == null) return;

        Gizmos.color = targetInAttackRange ? Color.red : Color.black;
        Extensions.DrawWireArc(core.transform.position, core.transform.up, attackRange, attackRadius);

    }

}
