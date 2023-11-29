using Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Protect : MonoBehaviour, IMoveAI
{
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

    // private float distanceToTarget;
    private Vector2 directionFromProtectedToTarget;
    // private bool staying = false;
    private bool seeTarget = false;

    void Start()
    {
        agent = GetComponentInParent<NavMeshAgent>();
    }

    public void AIUpdate()
    {
        if (protectionTarget == null) SerchProtectionTarget();
        if (protectionTarget == null) return; // mb Change ai behavior

        // distanceToTarget = Vector2.Distance(transform.position, target.position);
        directionFromProtectedToTarget = (target.transform.position - protectionTarget.position).normalized;
        // staying = AIGeneral.AgentIsAtDestinationPoint(agent, 0.5f);
        seeTarget = AIGeneral.TargetIsVisible(transform.position, target, visionRadius, core.playerObstaclesLayerMask);

        if (seeTarget)
        {
            AIGeneral.LookAt(core.transform, protectionTarget, rotationSpeed, inverse:true);
            agent.destination = GetDesiredPosition();
        }
    }

    private Vector2 GetDesiredPosition()
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
