using Interfaces;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class KeepDistance : MonoBehaviour, IMoveAI
{
    NavMeshAgent agent;

    [SerializeField] Transform target;
    [SerializeField] float innerRadius;
    [SerializeField] float desiredPositionRadius;
    [SerializeField] float goToTargetThreshold;
    [SerializeField] float desiredPositionSearchAngle;
    [SerializeField] Vector2 desiredPositionStayTime;

    public AICore core { get; set; }

    private float distanceToTarget;
    private Vector2 directionFromTarget;
    private bool movingToDesiredPosition = false;
    private bool staying = false;

    private Coroutine stayingCoroutine = null;

    private void Start()
    {
        agent = GetComponentInParent<AICore>().agent;
    }

    public void AIReset()
    {
        agent.destination = transform.position;
        movingToDesiredPosition = false;

        if (stayingCoroutine != null)
        {
            StopCoroutine(stayingCoroutine);
            stayingCoroutine = null;
        }
    }

    public void AIUpdate()
    {
        distanceToTarget = Vector2.Distance(transform.position, target.position);
        directionFromTarget = (transform.position - target.transform.position).normalized;
        staying = AIGeneral.AgentIsAtDestinationPoint(agent, 0.5f);

        if (distanceToTarget >= goToTargetThreshold)
        {
            agent.destination = target.position;
            movingToDesiredPosition = false;
        }
        else if (distanceToTarget < goToTargetThreshold && !movingToDesiredPosition)
        {
            movingToDesiredPosition = true;
            agent.destination = GetDesiredPosition();
        }
        else if (distanceToTarget < innerRadius)
        {
            agent.destination = target.position + (Vector3)directionFromTarget * goToTargetThreshold;
            movingToDesiredPosition = false;
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

        Vector3[] a = new Vector3[100];
        for (int i = 0; i < 100; i++)
        {
            Vector3 pos = AIGeneral.InsideCirlce(innerRadius, desiredPositionRadius, Quaternion.Euler(0, 0, desiredPositionSearchAngle / 2) * directionFromTarget, -desiredPositionSearchAngle);
            a[i] = pos;
        }
        a = a.OrderByDescending(x => RatePosition(x)).ToArray();


        return target.position + a[0];
    }

    private float RatePosition(Vector2 position)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, -directionFromTarget, goToTargetThreshold);
        if (hit && hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            return 1;
        }
        return 0;
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

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(target.position, innerRadius);
        Gizmos.color = Color.yellow;
        Extensions.DrawWireArc(target.position, directionFromTarget, desiredPositionSearchAngle, desiredPositionRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(target.position, goToTargetThreshold);

    }



}
