using Interfaces;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AttackKeepDistance : MonoBehaviour//, IMoveAI
{
    NavMeshAgent agent;

    [SerializeField] Transform target;
    [SerializeField] float distance;
    [SerializeField] float followThreshold;
    [SerializeField] float goToTargetThreshold;
    [SerializeField] float desiredPositionSearchRadius;
    [SerializeField] Vector2 desiredPositionStayTime;

    private float distanceToTarget;
    private bool movingToDesiredPosition = false;

    private Coroutine stayingCoroutine = null;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        distanceToTarget = Vector2.Distance(transform.position, target.position);

        if (distanceToTarget >= distance + goToTargetThreshold)
        {
            agent.isStopped = false;
            agent.destination = target.position;
            movingToDesiredPosition = false;
            //stayingCoroutine = null;
        }
        else if (distanceToTarget >= distance + followThreshold && !movingToDesiredPosition)
        {
            agent.isStopped = false;
            movingToDesiredPosition = true;
            agent.destination = GetDesiredPosition();
            //stayingCoroutine = null;
        }

        if (!agent.isStopped && AIGeneral.AgentIsAtDestinationPoint(agent, 0.25f))
        {
            agent.isStopped = true;
            movingToDesiredPosition = false;
            //if (stayingCoroutine == null)
                //stayingCoroutine = StartCoroutine(StayTimer());

        }
    }

    private Vector2 GetDesiredPosition()
    {
        Vector2 directionFromTarget = (transform.position - target.transform.position).normalized;
        // Vector2 middle = (Vector2)target.transform.position + (directionFromTarget * (distance + followThreshold)) / 2;
        Vector2 middle = (Vector2)target.transform.position + (directionFromTarget * (distance + followThreshold - desiredPositionSearchRadius));
        return middle + AIGeneral.InsideCirlce(0, desiredPositionSearchRadius);
    }

    /*IEnumerator StayTimer()
    {
        float time = Random.Range(desiredPositionStayTime.x, desiredPositionStayTime.y);
        yield return new WaitForSeconds(time);
        agent.destination = GetDesiredPosition();
        agent.isStopped = false;
        movingToDesiredPosition = true;
    }*/

    private void OnDrawGizmosSelected()
    {
        Vector2 directionFromTarget = (transform.position - target.transform.position).normalized;
        // Vector2 middle =  (Vector2)target.transform.position + (directionFromTarget * (distance + followThreshold)) / 2;
        Vector2 middle = (Vector2)target.transform.position + (directionFromTarget * (distance + followThreshold - desiredPositionSearchRadius));
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(middle, desiredPositionSearchRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(target.transform.position, distance);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(target.transform.position, distance + followThreshold);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(target.transform.position, distance + goToTargetThreshold);
    }



}
