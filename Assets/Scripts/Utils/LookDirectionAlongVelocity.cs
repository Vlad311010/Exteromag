using UnityEngine;

public class LookDirectionAlongVelocity : MonoBehaviour
{
    Rigidbody2D rigidbody;
    
    public float minimalSpeed = 4;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (rigidbody.velocity.magnitude < minimalSpeed)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else 
        {
            Quaternion lookDirection = Quaternion.LookRotation(new Vector3(0, 0, 1), rigidbody.velocity);
            transform.rotation = Quaternion.Euler(0, 0, lookDirection.eulerAngles.z + 90);
        }
    }
}
