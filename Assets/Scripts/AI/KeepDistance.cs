using Interfaces;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class KeepDistance : MonoBehaviour, IMoveAI
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
    private bool staying = false;

    private Coroutine stayingCoroutine = null;

    private void Start()
    {
        agent = GetComponentInParent<AICore>().agent;
    }

    public void AIUpdate()
    {
        distanceToTarget = Vector2.Distance(transform.position, target.position);
        staying = AIGeneral.AgentIsAtDestinationPoint(agent, 0.5f);

        if (distanceToTarget >= distance + goToTargetThreshold)
        {
            agent.destination = target.position;
            movingToDesiredPosition = false;
        }
        else if (distanceToTarget >= distance + followThreshold && !movingToDesiredPosition)
        {
            movingToDesiredPosition = true;
            agent.destination = GetDesiredPosition();
        }

        if (staying && stayingCoroutine == null)
        {
            stayingCoroutine = StartCoroutine(StayTimer());
        }
        else if (!staying && stayingCoroutine != null)
        {
            StopCoroutine(stayingCoroutine);
            stayingCoroutine = null;
        }
    }

    private Vector2 GetDesiredPosition()
    {
        movingToDesiredPosition = true;

        Vector2 directionFromTarget = (transform.position - target.transform.position).normalized;
        // Vector2 middle = (Vector2)target.transform.position + (directionFromTarget * (distance + followThreshold)) / 2;
        Vector2 middle = (Vector2)target.transform.position + (directionFromTarget * (distance + followThreshold - desiredPositionSearchRadius));
        return middle + AIGeneral.InsideCirlce(0, desiredPositionSearchRadius);
    }

    IEnumerator StayTimer()
    {
        float time = Random.Range(desiredPositionStayTime.x, desiredPositionStayTime.y);
        yield return new WaitForSeconds(time);
        agent.destination = GetDesiredPosition();

    }

    private void OnDrawGizmosSelected()
    {
        if (target == null) return;

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
