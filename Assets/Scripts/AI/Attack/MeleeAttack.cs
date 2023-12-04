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

    [SerializeField] Transform target;
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

    private NavMeshAgent agent;
    Vector2 directionToTarget;
    private bool targetInAttackRange;
    private bool attackIsInCooldown;

    void Start()
    {
        agent = GetComponentInParent<AICore>().agent;
    }

    public void AIUpdate()
    {
        // directionToTarget = (target.position - transform.position).normalized;
        // lookAngle = Extensions.NormalizeAngle(Extensions.GetAnglesFromDir(transform.position, directionToTarget)-90);
        targetInAttackRange = AIGeneral.IsInsideVisionCon(target.position, core.transform.position, core.transform.up, attackRange, attackRadius);
        // Debug.Log(lookAngle);
        if (targetInAttackRange && !attackIsInCooldown)
        {
            Attack();
            // StartCoroutine(Attack());
        }
    }

    void  Attack()
    {
        attackIsInCooldown = true;
        // Instantiate(attackTrail, transform.position + core.transform.up * attackRadius / 2, core.transform.rotation); // attack effect
        attackEffect.Play();
        foreach (Collider2D collider in ObjectsToDamage())
        {
            directionToTarget = (collider.transform.position - transform.position).normalized;
            collider.GetComponent<IHealthSystem>().ConsumeHp(damage, directionToTarget);
        }
        StartCoroutine(Cooldown());
        StartCoroutine(TurnOffMovement());
    }

    /*IEnumerator Attack()
    {
        attackIsInCooldown = true;
        List<HealthSystem> alreadyDamaged = new List<HealthSystem>();
        Instantiate(attackTrail, transform.position + core.transform.up * attackRadius/2, core.transform.rotation);
        for (int i = 0; i < tickPerAttack; i++)
        {
            yield return new WaitForSeconds(attackTick);
            
            foreach (HealthSystem hs in ObjectsToDamage())
            {
                if (!alreadyDamaged.Contains(hs))
                {
                    directionToTarget = (hs.transform.position - transform.position).normalized;
                    hs.ConsumeHp(damage, directionToTarget);
                    alreadyDamaged.Add(hs);
                }
            }
        }
        StartCoroutine(Cooldown());
    }*/

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
        attackIsInCooldown = false;
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
