using Interfaces;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class IdleWander : MonoBehaviour, IMoveAI
{
    NavMeshAgent agent;

    [SerializeField] Vector2 wanderBreak;
    [SerializeField] float wanderInnerRadius;
    [SerializeField] float wanderOuterRadius;
    [SerializeField] bool rootedToOriginalPosition;

    private Vector2 originalPosition;

    void Start()
    {
        agent = GetComponentInParent<AICore>().agent;
        originalPosition = agent.transform.position;
        StartCoroutine(Wait());
    }

    public void AIUpdate()
    {
        if (!agent.isStopped && AIGeneral.AgentIsAtDestinationPoint(agent, 0.25f))
        {
            StartCoroutine(Wait());
        }
    }

    private void SetWanderPosition()
    {
        agent.destination = NewDestination();
    }

    IEnumerator Wait()
    {
        agent.isStopped = true;
        yield return new WaitForSeconds(Random.Range(wanderBreak.x, wanderBreak.y));
        agent.isStopped = false;
        SetWanderPosition();


    }

    private Vector2 NewDestination()
    {
        Vector2 origin = rootedToOriginalPosition ? originalPosition : agent.transform.position;
        return origin + AIGeneral.InsideCirlce(wanderInnerRadius, wanderOuterRadius);
    }


}
