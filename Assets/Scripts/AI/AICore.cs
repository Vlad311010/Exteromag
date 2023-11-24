using Interfaces;
using UnityEngine;
using UnityEngine.AI;

public class AICore : MonoBehaviour, IDestroyable
{
    public NavMeshAgent agent;
    [SerializeField] IMoveAI moveAI;
    [SerializeField] IAttackAI attackAI;

    public bool moveAIActive = true;
    public bool attackAIActive = true;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateUpAxis = false;
        agent.updateRotation = false;


        moveAI = GetComponentInChildren<IMoveAI>();
        attackAI = GetComponentInChildren<IAttackAI>();
    }

    void Update()
    {
        if (moveAIActive)
            moveAI.AIUpdate();
        if (attackAIActive)
            attackAI.AIUpdate();
    }

    public void SpawnProjectile(GameObject projectile, Vector2 direction, float speed, float acceleration, float deccelaeration, bool useDecceleratiion, float deccelerationStart = 0f)
    {
        // Instantiate(projectile);
    }

    /*public Coroutine CallCoroutine(IEnumerator coroutine)
    {
        return StartCoroutine(coroutine);
    }*/

    public void DestroyObject()
    {
        Destroy(this.gameObject);
    }
}
