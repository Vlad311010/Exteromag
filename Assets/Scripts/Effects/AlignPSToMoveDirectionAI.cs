using UnityEngine;

public class AlignPSToMoveDirectionAI : MonoBehaviour
{
    AICore movement;

    [SerializeField] float angleOffset;

    private Vector2 direction;

    void Start()
    {
        movement = GetComponentInParent<AICore>();
    }

    private void Update()
    {
        direction = movement.movementDirection == Vector2.zero ? direction : movement.movementDirection * -1;
        Quaternion lookDirection = Quaternion.LookRotation(new Vector3(0, 0, 1), direction);
        transform.rotation = Quaternion.Euler(0, 0, lookDirection.eulerAngles.z + 90 + angleOffset);
    }
}
