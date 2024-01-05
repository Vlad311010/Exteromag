using Interfaces;
using System.Collections.Generic;
using UnityEngine;

public class PropsScript : MonoBehaviour, IDestroyable
{
    Collider2D collider;
    Animation animation;
    AIEffects effects;
    

    private void Start()
    {
        collider = GetComponent<Collider2D>();
        animation = GetComponent<Animation>();
        effects = GetComponent<AIEffects>();
    }

    public void DestroyObject()
    {
        animation.Play();
        collider.enabled = false;
        effects.OnDeath();
    }


    
    public void DetachChildren()
    {
        animation.Stop();
        transform.DetachChildren();
    }
}
