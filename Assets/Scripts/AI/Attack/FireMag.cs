using Interfaces;
using System.Collections;
using UnityEngine;

public class FireMag : MonoBehaviour, IAttackAI
{
    public AICore core { get; set; }

    [Header("Parameters")]
    public GameObject damageZonePrefab;
    public float damageZoneRadius;
    public float subZoneRadius;
    public Vector2 subZonesSpawnRadius;

    public float attackRadius;
    public Vector2 attackBreak;
    public int damageZonesPerAttack;


    [Header("Calculated")]
    private float attackBreakTimer;
    private GameObject activeDamageZone = null;

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
        float distanceToTarget = Vector2.Distance(transform.position, core.target.position);
        if (distanceToTarget < attackRadius)
        {
            AIGeneral.LookAt(core.transform, core.target);
            attackBreakTimer -= Time.deltaTime;
        }
        core.canAttack = attackBreakTimer < 0;
        if (core.canAttack && distanceToTarget < attackRadius)
        {
            StartAttackAnimation();
        }
    }

    private void StartAttackAnimation()
    {
        core.animator.SetBool("Casting", true);
    }

    public void Attack()
    {
        Quaternion rotatiton = Quaternion.Euler(0, 0, Random.Range(0, 360));
        activeDamageZone = Instantiate(damageZonePrefab, core.target.position, rotatiton);
        activeDamageZone.transform.localScale = new Vector3(damageZoneRadius * 2, damageZoneRadius * 2, 1);
        StartCoroutine(SpellChannelingCoroutine());
        for (int i = 0; i < damageZonesPerAttack-1; i++)
        {
            float angle = Random.Range(0, 360);
            float distance = Random.Range(subZonesSpawnRadius.x, subZonesSpawnRadius.y);
            Vector2 subZonePos = AIGeneral.GetPositionByRaycast(core.target.position, distance, angle, subZoneRadius, core.obstaclesLayerMask);
            Instantiate(damageZonePrefab, subZonePos, Quaternion.identity).transform.localScale = new Vector3(subZoneRadius * 2, subZoneRadius * 2, 1);
        }
    }

    IEnumerator SpellChannelingCoroutine()
    {
        if (activeDamageZone != null)
        {   
            yield return new WaitForEndOfFrame();
            StartCoroutine(SpellChannelingCoroutine());
        }
        else
        {
            core.animator.SetBool("Casting", false);
            ResetAttackBreakTimer();
        }

    }

    private void OnDrawGizmosSelected()
    {
        if (core?.target == null) return;

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(core.target.position, subZonesSpawnRadius.y);
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawSphere(core.target.position, damageZoneRadius);
    }
}
