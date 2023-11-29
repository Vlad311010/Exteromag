using Interfaces;
using Structs;
using System;
using System.Collections;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    Rigidbody2D rigidbody;

    [SerializeField] int maxHp;
    [SerializeField] AudioClip hitSound;

    [Header("Options")]
    [SerializeField] bool staggerable;
    [SerializeField] float staggerTime;

    private int currentHp;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        currentHp = maxHp;
    }

    public void ConsumeHp(int amount, Vector2 staggerDirectiom)
    {
        currentHp = Math.Clamp(currentHp - amount, 0, maxHp);
        GameEvents.current.HealthChange(currentHp, maxHp);
        if (currentHp <= 0)
            GetComponent<IDestroyable>().DestroyObject();
        else if (amount > 0 && staggerable)
        {
            StartCoroutine(Stagger(staggerTime, staggerDirectiom));
        }
    }



    IEnumerator Stagger(float time, Vector2 direction)
    {
        if (TryGetComponent(out CharacterLimitations limitations))
        {
            limitations.ActivateMovementContraint();
            rigidbody.velocity = direction * 350 * Time.fixedDeltaTime;
            yield return new WaitForSeconds(time);
            rigidbody.velocity = Vector2.zero;
            limitations.DisableMovementContraint();
        }
        else if (TryGetComponent(out AICore aiCore))
        {
            aiCore.OnHit();
            rigidbody.velocity = direction * 250 * Time.fixedDeltaTime;
            yield return new WaitForSeconds(time);
            rigidbody.velocity = Vector2.zero;
            // limitations.DisableMovementContraint();
        }

    }

}
