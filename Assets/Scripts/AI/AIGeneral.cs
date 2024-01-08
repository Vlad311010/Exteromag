using UnityEngine;
using UnityEngine.AI;

public static class AIGeneral
{
    public static bool AgentIsAtDestinationPoint(NavMeshAgent agent, float threshold = 0.25f)
    {
        return agent.remainingDistance < threshold;
    }

    public static void LookAt(Transform self, Transform target)
    {
        Vector3 playerDir = (target.position - self.position).normalized;
        self.rotation = Quaternion.LookRotation(Vector3.forward, playerDir);
    }

    public static void LookAt(Transform self, Vector3 target)
    {
        Vector3 playerDir = (target - self.position).normalized;
        self.rotation = Quaternion.LookRotation(Vector3.forward, playerDir);
    }

    public static void LookAt(Transform self, Transform target, float step, bool inverse=true)
    {
        Vector3 playerDir = (target.position - self.position).normalized;
        Quaternion desiredRotation = Quaternion.LookRotation(Vector3.forward, playerDir);
        if (inverse)
            desiredRotation = Quaternion.Euler(desiredRotation.eulerAngles.x, desiredRotation.eulerAngles.y, desiredRotation.eulerAngles.z - 180);
        self.rotation = Quaternion.RotateTowards(self.rotation, desiredRotation, step);
    }

    public static bool IsInsideVisionCon(Vector2 targetPoint, Vector2 position, Vector2 direction, float range, float radius)
    {
        Vector2 directionToTarger = (targetPoint - position).normalized;
        float lookAngle = (Extensions.GetAnglesFromDir(position, direction));
        float targetAngle = (Extensions.GetAnglesFromDir(position, directionToTarger));
        float rangeLeft = (lookAngle + range / 2);
        float rangeRight = (lookAngle - range / 2); 
        float rangeMin = Mathf.Min(rangeLeft, rangeRight);
        float rangeMax= Mathf.Max(rangeLeft, rangeRight);

        // Debug.Log(lookAngle + " | " + targetAngle + " | " + rangeMin + " | " + rangeMax + " | " + (rangeMin <= targetAngle) + " | " + (targetAngle <= rangeMax));
        return (Vector2.Distance(position, targetPoint) <= radius && rangeMin <= targetAngle && targetAngle <= rangeMax);
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

    public static Vector2 InsideCirlce(float innerRadius, float outerRadius, Vector2 direction, float angle)
    {
        angle = Random.Range(0, angle);
        direction = Quaternion.Euler(0, 0, angle) * direction;
        return direction * Random.Range(innerRadius, outerRadius);
    }

    public static Vector2 GetPositionByRaycast(Vector2 position, float distance, float angle, float hitOffset, LayerMask collisionLayerMask)
    {
        Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.up;
        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, collisionLayerMask);
        if (hit)
        {
            return position + direction * hit.distance - direction * hitOffset;
        }
        else
            return position + direction * distance;
    }

    public static bool TargetIsVisible(Vector2 position, Transform target, float distance, LayerMask collisionLayerMask)
    {
        Vector2 direction = ((Vector2)target.position - position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, collisionLayerMask);
        return hit && hit.transform == target;
    }

    public static bool PointIsReachable(NavMeshAgent agent, Vector3 point, out NavMeshPath path)
    {
        path = new NavMeshPath();
        return agent.CalculatePath(point, path) && path.status == NavMeshPathStatus.PathComplete;

    }
}

