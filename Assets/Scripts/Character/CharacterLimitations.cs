using UnityEngine;

public class CharacterLimitations : MonoBehaviour
{
    CharacterMovement movement;
    CharacterInteraction interaction;


    public bool movementConstraint = false;

    void Start()
    {
        movement = GetComponent<CharacterMovement>();
        interaction = GetComponent<CharacterInteraction>();
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

    public void DisableActions()
    {
        movement.enabled = false;
        interaction.enabled = false;
        interaction.control.gameplay.Disable();
        GetComponent<Rigidbody2D>().simulated = false;
    }

    public void ActivateActions()
    {
        movement.enabled = true;
        interaction.enabled = true;
        interaction.control.gameplay.Enable();
        GetComponent<Rigidbody2D>().simulated = true;
    }

    public void InfiniteMana(bool active)
    {
        if (active)
            GameEvents.current.onManaChange += RestoreMana;
        else
            GameEvents.current.onManaChange -= RestoreMana;
    }

    private void RestoreMana(int current, int max)
    {
        GameEvents.current.onManaChange -= RestoreMana;
        GetComponent<ManaPool>().Consume(-max);
        GameEvents.current.onManaChange += RestoreMana;
    }
}
