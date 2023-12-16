using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;
using UnityEngine.AI;

public class RushTarget : MonoBehaviour, IMoveAI
{
    NavMeshAgent agent;

    // [SerializeField] float visionRadius;

    public AICore core { get; set; }

    private float distanceToTarget;
    private bool targetWasNoticed = false;

    void Start()
    {
        agent = GetComponentInParent<NavMeshAgent>();
    }


    public void AIUpdate()
    {
        // distanceToTarget = Vector2.Distance(transform.position, core.target.position);
        // targetWasNoticed = targetWasNoticed || AIGeneral.TargetIsVisible(transform.position, core.target, visionRadius, core.playerLayerMask | core.obstaclesLayerMask);

        // if (targetWasNoticed && distanceToTarget <= visionRadius)
        // if (distanceToTarget <= visionRadius)
        {
            if (core.canAttack)
                agent.destination = core.target.position;
            else
            {
                Vector2 directionToTarget = (transform.position - core.target.position).normalized;
                agent.destination = (Vector2)core.target.position + directionToTarget * 1.8f;
            }
        }
    }


    public void AIReset()
    {

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        // Gizmos.DrawWireSphere(transform.position, visionRadius);
    }
}
