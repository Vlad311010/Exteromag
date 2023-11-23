using UnityEngine;

public class CharacterLookDirection : MonoBehaviour
{
    public Transform target;    

    void Update()
    {
        Vector3 playerDir = (target.transform.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, playerDir);
    }
}
