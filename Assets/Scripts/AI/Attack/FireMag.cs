using Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMag : MonoBehaviour, IAttackAI
{
    public AICore core { get; set; }

    [Header("Parameters")]
    public GameObject damageZone;
    public float damageZoneRadius;
    public float subZoneRadius;
    public Vector2 subZonesSpawnRadius;

    public float attackRadius;
    public Vector2 attackBreak;
    public int damageZonesPerAttack;
    [SerializeField] Transform target;


    [Header("Calculated")]
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
        float distanceToTarget = Vector2.Distance(transform.position, target.position);

        attackBreakTimer -= Time.deltaTime;
        if (attackBreakTimer < 0 &&  distanceToTarget < attackRadius)
        {
            ResetAttackBreakTimer();
            Attack();
        }
    }

    private void Attack()
    {
        Instantiate(damageZone, target.position, Quaternion.identity).transform.localScale = new Vector3(damageZoneRadius*2, damageZoneRadius * 2, 1);
        for (int i = 0; i < damageZonesPerAttack-1; i++)
        {
            float angle = Random.Range(0, 360);
            float distance = Random.Range(subZonesSpawnRadius.x, subZonesSpawnRadius.y);
            Vector2 subZonePos = AIGeneral.GetPositionByRaycast(target.position, distance, angle, subZoneRadius, core.obstaclesLayerMask);
            Instantiate(damageZone, subZonePos, Quaternion.identity).transform.localScale = new Vector3(subZoneRadius * 2, subZoneRadius * 2, 1);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (target == null) return;

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(target.position, subZonesSpawnRadius.y);
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawSphere(target.position, damageZoneRadius);
    }
}
