using UnityEngine;

public class CharacterLimitations : MonoBehaviour
{
    CharacterMovement movement;


    [SerializeField] private float carryMovementSpeed;
    [SerializeField] private float carryJumpHeight;
    [SerializeField] private float carryJumpTimeToApex;

    private float defaultMovementSpeed;
    private float defaultJumpHeight;
    private float defaultJumpTimeToApex;

    void Start()
    {
        movement = GetComponent<CharacterMovement>();

        defaultMovementSpeed = movement.maxSpeed;
    }

    public void ActivateCarryLimitations()
    {
        movement.maxSpeed = carryMovementSpeed;
    }

    public void DisableLimitations()
    {
        movement.maxSpeed = defaultMovementSpeed;
    }
}
