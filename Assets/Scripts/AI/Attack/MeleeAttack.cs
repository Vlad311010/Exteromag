using Interfaces;
using Structs;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

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
    [SerializeField] GameObject attackTrail;

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
            StartCoroutine(Attack());
    }

    IEnumerator Attack()
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
    }

    private HealthSystem[] ObjectsToDamage()
    {
        Collider2D[] possibleHits = Physics2D.OverlapCircleAll(transform.position, attackRadius, attackLayerMask);
        HealthSystem[] hits = possibleHits
            .Where(hit => AIGeneral.IsInsideVisionCon(hit.transform.position, transform.position, core.transform.up, attackRange, attackRadius))
            .Where(hit => hit.TryGetComponent(out HealthSystem _))
            .Select(hit => hit.GetComponent<HealthSystem>())
            .ToArray();

        return hits;

    } 

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(attackCooldownTime);
        attackIsInCooldown = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (core == null) return;

        Gizmos.color = targetInAttackRange ? Color.red : Color.black;
        Extensions.DrawWireArc(core.transform.position, core.transform.up, attackRange, attackRadius);

    }

}
