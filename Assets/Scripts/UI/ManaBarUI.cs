using System.Collections;
using UnityEngine;

public class ManaBarUI : MonoBehaviour
{
    [SerializeField] SpriteRenderer currentAmount;
    [SerializeField] SpriteRenderer recentlyUsedAmount;
    [SerializeField] float updatingDelay = 0.2f;

    float updatingDelayTimer;
    private Coroutine spriteUpdateCoroutine = null;
    private Coroutine updatingDelayCoroutine = null;

    void Start()
    {
        GameEvents.current.onManaChange += UpdateManaBar;
        recentlyUsedAmount.size = currentAmount.size;
        updatingDelayTimer = updatingDelay;
    }

    private void OnDestroy()
    {
        GameEvents.current.onManaChange -= UpdateManaBar;
    }

    private void UpdateManaBar(int current, int max)
    {
        float percent = (float)current / max;
        currentAmount.size = new Vector2(percent, 1);

        if (spriteUpdateCoroutine != null)
            StopCoroutine(spriteUpdateCoroutine);

        updatingDelayTimer = updatingDelay;
        if (updatingDelayCoroutine == null)
            updatingDelayCoroutine = StartCoroutine(TriggerContinuousUpdate());
    }

    IEnumerator TriggerContinuousUpdate()
    {
        if (updatingDelayTimer >= 0)
        {
            updatingDelayTimer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
            updatingDelayCoroutine = StartCoroutine(TriggerContinuousUpdate());
        }
        else
        {
            updatingDelayCoroutine = null;

            if (spriteUpdateCoroutine != null)
                StopCoroutine(spriteUpdateCoroutine);
            spriteUpdateCoroutine = StartCoroutine(UpdateSpriteContinuous(recentlyUsedAmount, currentAmount.size.x, 0.2f));
        }
    }

    IEnumerator UpdateSpriteContinuous(SpriteRenderer sprite, float desiredValue, float changeSpeed)
    {
        float amount = sprite.size.x;
        while (amount != desiredValue)
        {
            amount = Mathf.MoveTowards(amount, desiredValue, changeSpeed * Time.deltaTime);
            sprite.size = new Vector2(amount, 1);
            yield return new WaitForEndOfFrame();
        }
    }
}
