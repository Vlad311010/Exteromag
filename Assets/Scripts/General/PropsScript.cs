using Interfaces;
using System.Collections.Generic;
using UnityEngine;

public class PropsScript : MonoBehaviour, IDestroyable
{
    Collider2D collider;
    
    [SerializeField] Animation animation;

    private void Start()
    {
        collider = GetComponent<Collider2D>();
        animation = GetComponent<Animation>();
    }

    public void DestroyObject()
    {
        animation.Play();
        collider.enabled = false;
    }


    
    public void DetachChildren()
    {
        animation.Stop();
        transform.DetachChildren();
    }
}
