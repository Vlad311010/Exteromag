using System.Collections;
using UnityEngine;

public class AIDisable : MonoBehaviour
{
    AICore core;

    private float destroyDelayTime = 2;

    void Start()
    {
        core = GetComponent<AICore>();
    }

    public void Disable()
    {
        foreach (SpriteRenderer sprite in GetComponentsInChildren<SpriteRenderer>())
        {
            Destroy(sprite);
        }
        foreach (Collider2D collider in GetComponentsInChildren<Collider2D>())
        {
            collider.enabled = false;
        }

        core.agent.enabled = false;
        GetComponent<Rigidbody2D>().simulated = false;
        GetComponent<Collider2D>().enabled = false;
        Animator animator = GetComponentInChildren<Animator>();
        animator.fireEvents = false;
        animator.StopPlayback();
        Destroy(GetComponentInChildren<AIAnimations>());
        Destroy(core.moveAI as Component);
        Destroy(core.attackAI as Component);
        Destroy(core.agent);
        Destroy(core);

        StopAllCoroutines();
        StartCoroutine(DelayedDestroy(destroyDelayTime));
    }

    IEnumerator DelayedDestroy(float time)
    {
        yield return new WaitForSeconds(time);
        GameEvents.current.EnemyDied();
        Destroy(this.gameObject);
    }

}
