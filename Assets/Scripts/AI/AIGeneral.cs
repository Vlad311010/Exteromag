using UnityEngine;
using UnityEngine.AI;

public static class AIGeneral
{
    public static bool AgentIsAtDestinationPoint(NavMeshAgent agent, float threshold = 0.25f)
    {
        return agent.remainingDistance < threshold;
    }

    public static Vector2 InsideCirlce(float innerRadius, float outerRadius)
    {
        float angle = Random.Range(0, 359);
        Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.up;
        return direction * Random.Range(innerRadius, outerRadius);
    }

    public static Vector2 InsideCirlce(float innerRadius, float outerRadius, float alpha, float beta)
    {
        float angle = Random.Range(alpha, beta);
        Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.up;
        return direction * Random.Range(innerRadius, outerRadius);
    }

    public static bool IsVisible(Vector2 position, Transform target, float distance, LayerMask collisionLayerMask)
    {
        Vector2 direction = ((Vector2)target.position - position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, collisionLayerMask);
        return hit && hit.transform == target;
    }
}

