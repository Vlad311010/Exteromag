using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

public class CharacterHealthSystem : MonoBehaviour, IHealthSystem
{
    Rigidbody2D rigidbody;
    CharacterLimitations limitations;

    [Header("Parameters")]
    [SerializeField] int maxHp;

    [SerializeField] Material hitMaterial;
    [SerializeField] Material defaultMaterial;
    [SerializeField] float hitHighlightTime;
    [SerializeField] AudioClip hitSound;

    [Header("Options")]
    [SerializeField] bool staggerable;
    [SerializeField] float staggerTime;

    private int currentHp;
    private SpriteRenderer[] sprites;
    private Coroutine hitHighlightCoroutine;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        limitations = GetComponent<CharacterLimitations>();
        sprites = GetComponentsInChildren<SpriteRenderer>();
        currentHp = maxHp;
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

            if (staggerable)
                StartCoroutine(Stagger(staggerTime, staggerDirectiom));
        }
    }


    IEnumerator Stagger(float time, Vector2 direction)
    {
        limitations.ActivateMovementContraint();
        rigidbody.velocity = direction * 350 * Time.fixedDeltaTime;
        yield return new WaitForSeconds(time);
        rigidbody.velocity = Vector2.zero;
        limitations.DisableMovementContraint();
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
            sprite.material = hitMaterial;
        }

        yield return new WaitForSeconds(hitHighlightTime);

        foreach (SpriteRenderer sprite in sprites)
        {
            sprite.material = defaultMaterial;
        }
    }
}
