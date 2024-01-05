using UnityEngine;

public class AlignPSToMoveDirectionCharacter : MonoBehaviour
{
    CharacterMovement movement;
    
    [SerializeField] float angleOffset;

    private Vector2 direction;

    void Start()
    {
        movement = GetComponentInParent<CharacterMovement>();
    }

    private void Update()
    {
        direction = movement.direction == Vector2.zero ? direction : movement.direction * -1;
        Quaternion lookDirection = Quaternion.LookRotation(new Vector3(0, 0, 1), direction);
        transform.rotation = Quaternion.Euler(0, 0, lookDirection.eulerAngles.z + 90 + angleOffset);
    }

}
