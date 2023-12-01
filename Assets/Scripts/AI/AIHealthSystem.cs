using Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHealthSystem : MonoBehaviour, IHealthSystem
{

    [SerializeField] int maxHp;

    [SerializeField] Material hitMaterial;
    [SerializeField] Material defaultMaterial;
    [SerializeField] float hitHighlightTime;
    [SerializeField] AudioClip hitSound;

    private int currentHp;
    private SpriteRenderer[] sprites;
    private Coroutine hitHighlightCoroutine;


    void Start()
    {
        currentHp = maxHp;
        sprites = GetComponentsInChildren<SpriteRenderer>();
    }

    public void ConsumeHp(int amount, Vector2 staggerDirectiom)
    {
        currentHp = System.Math.Clamp(currentHp - amount, 0, maxHp);
        GameEvents.current.HealthChange(currentHp, maxHp);
        if (currentHp <= 0)
            GetComponent<IDestroyable>().DestroyObject();
        else if (amount > 0)
        {
            OnHit();
        }
    }

    private void OnHit()
    {
        if (hitHighlightCoroutine != null)
        {
            StopCoroutine(hitHighlightCoroutine);
        }
        hitHighlightCoroutine = StartCoroutine(HitHighlight());
    }

    IEnumerator HitHighlight()
    {
        foreach (SpriteRenderer sprite in sprites)
        {
            if (sprite != null)
                sprite.material = hitMaterial;
        }

        yield return new WaitForSeconds(hitHighlightTime);

        foreach (SpriteRenderer sprite in sprites)
        {
            if (sprite != null)
                sprite.material = defaultMaterial;
        }
    }


}
