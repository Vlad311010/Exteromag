using Interfaces;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class IdleWander : MonoBehaviour, IMoveAI
{
    public AICore core { get; set; }
    public Transform target { get; set; }

    NavMeshAgent agent;

    [SerializeField] Vector2 wanderBreak;
    [SerializeField] float wanderInnerRadius;
    [SerializeField] float wanderOuterRadius;
    [SerializeField] bool rootedToOriginalPosition;

    private Vector2 originalPosition;
    private bool staying = false;


    void Start()
    {
        agent = GetComponentInParent<AICore>().agent;
        originalPosition = transform.position;

        if (agent.destination == null)
            agent.destination = transform.position;
        // StartCoroutine(Wait());
    }

    public void AIReset() { }

    public void AIUpdate()
    {
        if (!staying && AIGeneral.AgentIsAtDestinationPoint(agent, 0.25f))
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
        staying = true;
        yield return new WaitForSeconds(Random.Range(wanderBreak.x, wanderBreak.y));
        staying = false;
        SetWanderPosition();
    }

    private Vector2 NewDestination()
    {
        Vector2 origin = rootedToOriginalPosition ? originalPosition : agent.transform.position;
        Vector2 destination = origin + AIGeneral.InsideCirlce(wanderInnerRadius, wanderOuterRadius);
        if (!AIGeneral.PointIsReachable(agent, destination, out NavMeshPath path) && path.corners.Length > 0)
        {
            destination = path.corners.Last();
        }

        return destination;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, wanderInnerRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, wanderOuterRadius);
    }

}
