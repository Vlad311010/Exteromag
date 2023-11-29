using UnityEngine;

public class CharacterLimitations : MonoBehaviour
{
    CharacterMovement movement;


    public bool movementConstraint = false;

    void Start()
    {
        movement = GetComponent<CharacterMovement>();
    }

    public void ActivateMovementContraint()
    {
        movementConstraint = true;
        movement.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        movement.velocity = Vector2.zero;
        // movement.desiredVelocity = Vector2.zero;
    }

    public void DisableMovementContraint()
    {
        movementConstraint = false;
    }
}
