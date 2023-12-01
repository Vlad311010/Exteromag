using Interfaces;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AICore : MonoBehaviour, IDestroyable
{
    [SerializeField] GameObject splash;
    [SerializeField] Color enemyColor;

    public NavMeshAgent agent;
    public int threatLevel;
    [SerializeField] IMoveAI moveAI;
    [SerializeField] IAttackAI attackAI;
    [SerializeField] public LayerMask playerLayerMask;
    [SerializeField] public LayerMask obstaclesLayerMask;

    [SerializeField] bool moveAIActive = true;
    [SerializeField] bool attackAIActive = true;


    


    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateUpAxis = false;
        agent.updateRotation = false;


        moveAI = GetComponentInChildren<IMoveAI>();
        attackAI = GetComponentInChildren<IAttackAI>();
        moveAI.core = this;
        attackAI.core = this;
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
        {
            moveAI.AIReset();
            agent.isStopped = false;    
        }
        else
        {
            agent.isStopped = true;
        }

        moveAIActive = active;
    }

    public void DestroyObject()
    {
        Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(0, 359));
        SpriteRenderer splashSprite = Instantiate(splash, transform.position, rotation).GetComponent<SpriteRenderer>();
        splashSprite.color = enemyColor;
        Destroy(this.gameObject);
    }

    public void OnHit()
    {
    }
    
}
