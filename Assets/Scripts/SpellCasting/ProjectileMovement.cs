using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D body;

    [Header("Movement Stats")]
    [Tooltip("Maximum movement speed")] public float maxSpeed = 10f;
    [Tooltip("How fast to reach max speed")] public float maxAcceleration = 52f;
    [Tooltip("How fast to stop after letting go")] public float maxDecceleration = 52f;
    [SerializeField][Tooltip("Friction to apply against movement on stick")] private float friction;
    
    private float deccelerationStart;

    [Header("Options")]
    [SerializeField] bool useAcceleration = true;


    [Header("Calculations")]
    public Vector2 direction;
    private Vector2 desiredVelocity;
    public Vector2 velocity;
    private float maxSpeedChange;
    private bool deccelerating;

    private void Awake()
    {
        //Find the character's Rigidbody and ground detection script
        body = GetComponent<Rigidbody2D>();
    }

    public void Init(Vector2 direction, float speed, float acceleration, float deccelaeration, bool useDecceleratiion, float deccelerationStart = 0f)
    {
        this.direction = direction;
        maxSpeed = speed;
        maxAcceleration = acceleration;
        maxDecceleration = deccelaeration;
        this.deccelerationStart = deccelerationStart;
        if (useDecceleratiion)
            StartCoroutine(DeccelerationTimer());
    }



    private void Update()
    {
        desiredVelocity = direction * Mathf.Max(maxSpeed - friction, 0f);
    }

    private void FixedUpdate()
    {
        velocity = body.velocity;

        if (useAcceleration)
        {
            runWithAcceleration();
        }
        else
        {
            runWithoutAcceleration();
        }
    }

    private IEnumerator DeccelerationTimer()
    {
        yield return new WaitForSeconds(deccelerationStart);
        direction = Vector2.zero;
        deccelerating = true;
    }

    private void runWithAcceleration()
    {
        //Set our acceleration, deceleration, and turn speed stats, based on whether we're on the ground on in the air
        float decceleration = deccelerating ? maxDecceleration : maxAcceleration;

        maxSpeedChange = decceleration * Time.deltaTime;

        //Move our velocity towards the desired velocity, at the rate of the number calculated above
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        velocity.y = Mathf.MoveTowards(velocity.y, desiredVelocity.y, maxSpeedChange);

        //Update the Rigidbody with this new velocity
        body.velocity = velocity;

    }

    private void runWithoutAcceleration()
    {
        velocity = desiredVelocity;

        body.velocity = velocity;
    }
}
