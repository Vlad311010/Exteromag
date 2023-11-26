using Interfaces;
using UnityEngine;
using UnityEngine.AI;

public class AICore : MonoBehaviour, IDestroyable
{
    [SerializeField] GameObject splash;
    [SerializeField] Color enemyColor;

    public NavMeshAgent agent;
    [SerializeField] IMoveAI moveAI;
    [SerializeField] IAttackAI attackAI;

    [SerializeField] bool moveAIActive = true;
    [SerializeField] bool attackAIActive = true;

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


    public void moveAISetActive(bool active)
    {
        if (active)
            moveAI.AIReset();

        moveAIActive = active;
    }

    public void DestroyObject()
    {
        Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(0, 359));
        SpriteRenderer splashSprite = Instantiate(splash, transform.position, rotation).GetComponent<SpriteRenderer>();
        splashSprite.color = enemyColor;
        Destroy(this.gameObject);
    }
}
