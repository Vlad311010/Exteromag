using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*if (collision.TryGetComponent(out ProjectileMovement projectileMovement))
        {
            projectileMovement.Freeze();
        }*/
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out ProjectileMovement projectileMovement))
        {
            projectileMovement.Freeze();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out ProjectileMovement projectileMovement))
        {
            projectileMovement.Unfreeze();
        }
    }
}
