using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;
using UnityEngine.AI;

public class RushTarget : MonoBehaviour, IMoveAI
{
    NavMeshAgent agent;

    [SerializeField] Transform target;
    [SerializeField] float visionRadius;
    [SerializeField] Vector2 desiredPositionStayTime;

    public AICore core { get; set; }

    private float distanceToTarget;
    private bool movingToDesiredPosition = false;
    private bool targetWasNoticed = false;
    private bool staying = false;

    void Start()
    {
        agent = GetComponentInParent<NavMeshAgent>();
    }


    public void AIUpdate()
    {
        distanceToTarget = Vector2.Distance(transform.position, target.position);
        staying = AIGeneral.AgentIsAtDestinationPoint(agent, 0.5f);
        targetWasNoticed = targetWasNoticed || AIGeneral.TargetIsVisible(transform.position, target, visionRadius, core.playerLayerMask | core.obstaclesLayerMask);

        if (targetWasNoticed && distanceToTarget <= visionRadius)
        {
            agent.destination = target.position;
        }
    }


    public void AIReset()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRadius);
    }
}
