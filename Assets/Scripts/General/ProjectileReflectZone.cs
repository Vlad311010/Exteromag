using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileReflectZone : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out ProjectileMovement projectileMovement))
        {
            // projectileMovement.direction
        }
    }
    
}
