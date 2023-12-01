using Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Protect : MonoBehaviour, IMoveAI
{
    [SerializeField] Transform shieldGO;

    public Transform protectionTarget;
    [SerializeField] Transform target;
    [SerializeField] float innerRadius;
    [SerializeField] float outerRadius;
    [SerializeField] float rotationSpeed;
    [SerializeField] float visionRadius;
    [SerializeField] float protectionTargetSearchRadius;
    [SerializeField] float offset;

    public AICore core { get; set; }
    private NavMeshAgent agent;

    private Vector2 directionFromProtectedToTarget;
    private bool seeTarget = false;

    void Start()
    {
        agent = GetComponentInParent<NavMeshAgent>();
    }

    public void AIUpdate()
    {
        if (protectionTarget == null) SerchProtectionTarget();
        
        if (protectionTarget == null || shieldGO == null)
        {
            AIGeneral.LookAt(core.transform, target);
            agent.destination = target.position;
            return;
        }
        

        directionFromProtectedToTarget = (target.transform.position - protectionTarget.position).normalized;
        seeTarget = AIGeneral.TargetIsVisible(transform.position, target, visionRadius, core.playerLayerMask | core.obstaclesLayerMask);

        AIGeneral.LookAt(core.transform, protectionTarget, rotationSpeed, inverse: true);
        if (seeTarget)
        {
            agent.destination = GetDesiredPositionProtecting();
        }
        else
        {
            agent.destination = protectionTarget.position + Vector3.right * innerRadius;
        }
    }

    private Vector2 GetDesiredPositionProtecting()
    {
        float distance = Random.Range(innerRadius, outerRadius);
        Vector3 pos = Quaternion.Euler(0, 0, offset) * directionFromProtectedToTarget * distance;

        return protectionTarget.position + pos;
    }

    private void SerchProtectionTarget()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, protectionTargetSearchRadius, 1 << gameObject.layer);
        AICore[] enemies = colliders
            .Select(c => c.GetComponent<AICore>())
            .Where(aiCore => aiCore != null && aiCore.threatLevel >= 0)
            .OrderByDescending(aiCore => aiCore.threatLevel)
            .ToArray();
        protectionTarget = enemies.FirstOrDefault()?.transform;
    }

    public void AIReset()
    {
    }

    private void OnDrawGizmosSelected()
    {
        if (target == null || protectionTarget == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(protectionTarget.position, innerRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(protectionTarget.position, outerRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, visionRadius);
    }
}
