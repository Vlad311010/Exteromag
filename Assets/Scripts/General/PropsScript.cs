using Interfaces;
using UnityEngine;

public class PropsScript : MonoBehaviour, IDestroyable
{
    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
