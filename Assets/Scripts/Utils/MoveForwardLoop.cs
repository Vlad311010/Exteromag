using UnityEngine;

public class MoveForwardLoop : MonoBehaviour
{
    ProjectileMovement movement;

    [SerializeField] Vector2 direction;
    [SerializeField] float speed;
    [SerializeField] float warpTime;

    private Vector2 startPos;
    private float warpTimer;

    private void Start()
    {
        movement = GetComponent<ProjectileMovement>();
        movement.Init(direction.normalized, speed, 0, false, 0);
        startPos = transform.position;
        warpTimer = warpTime;
    }

    private void Update()
    {
        warpTimer -= Time.deltaTime;
        if (warpTimer < 0)
        {
            transform.position = startPos;
            warpTimer = warpTime;
        }
    }




}
