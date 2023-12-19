using Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Protect : MonoBehaviour, IMoveAI
{
    [SerializeField] Transform shieldGO;

    public Transform protectionTarget;
    [SerializeField] float innerRadius;
    [SerializeField] float outerRadius;
    [SerializeField] float rotationSpeed;
    [SerializeField] float visionRadius;
    [SerializeField] float protectionTargetSearchRadius;
    public float offset;

    public AICore core { get; set; }
    private NavMeshAgent agent;

    private Vector2 directionFromProtectedToTarget;
    private Vector2 lastDirectionFromProtectedToTarget = Vector3.right;
    private bool seeTarget = false;

    void Start()
    {
        agent = GetComponentInParent<NavMeshAgent>();
    }

    public void AIUpdate()
    {
        if (protectionTarget == null) SearchProtectionTarget();
        
        if (protectionTarget == null || shieldGO == null)
        {
            AIGeneral.LookAt(core.transform, core.target);
            agent.destination = core.target.position;
            return;
        }
        

        directionFromProtectedToTarget = (core.target.transform.position - protectionTarget.position).normalized;
        seeTarget = AIGeneral.TargetIsVisible(transform.position, core.target, visionRadius, core.playerLayerMask | core.obstaclesLayerMask);

        AIGeneral.LookAt(core.transform, protectionTarget, rotationSpeed, inverse: true);
        if (seeTarget)
        {
            agent.destination = GetDesiredPositionProtecting();
            lastDirectionFromProtectedToTarget = directionFromProtectedToTarget;
        }
        else
        {
            agent.destination = protectionTarget.position + Quaternion.Euler(0, 0, offset) * lastDirectionFromProtectedToTarget * innerRadius;
        }
    }

    private Vector2 GetDesiredPositionProtecting()
    {
        float distance = Random.Range(innerRadius, outerRadius);
        Vector3 pos = Quaternion.Euler(0, 0, offset) * directionFromProtectedToTarget * distance;

        return protectionTarget.position + pos;
    }

    private void SearchProtectionTarget()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, protectionTargetSearchRadius, 1 << gameObject.layer);
        AICore[] enemies = colliders
            .Select(c => c.GetComponent<AICore>())
            .Where(aiCore => aiCore != null && aiCore.threatLevel >= 0)
            .OrderByDescending(aiCore => aiCore.threatLevel)
            .ThenBy(aiCore => Vector2.Distance(transform.position, aiCore.transform.position))
            .ToArray();
        
        protectionTarget = enemies.FirstOrDefault()?.transform;
        if (protectionTarget == null) return;

        UnderProtection underProtection;
        if (!protectionTarget.TryGetComponent(out underProtection))
        {
            underProtection = protectionTarget.AddComponent<UnderProtection>();
        }
        underProtection.AddProtector(this);
    }

    public void AIReset()
    {
    }

    private void OnDestroy()
    {
        GameEvents.current.ProtectorDied(this);
    }

    private void OnDrawGizmosSelected()
    {
        if (core == null || protectionTarget == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(protectionTarget.position, innerRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(protectionTarget.position, outerRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, visionRadius);
    }
}
