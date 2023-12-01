using System.Collections;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D body;

    [Header("Movement Stats")]
    [Tooltip("Maximum movement speed")] public float maxSpeed = 10f;
    [Tooltip("How fast to stop after letting go")] public float maxDecceleration = 52f;
    [SerializeField][Tooltip("Friction to apply against movement on stick")] private float friction;
    
    

    [Header("Options")]
    [SerializeField] bool destroyOnStop = false;


    [Header("Parameters")]
    public Vector2 direction;
    private bool useDecceleration;
    private float deccelerationStartTimer;

    [Header("Calculations")]
    private Vector2 desiredVelocity;
    public Vector2 velocity;
    private bool deccelerating;
    private Coroutine deccelerationCoroutine;

    [Header("Freeze feature")]
    private bool freezed = false; 
    private Vector2 freezedVelocity; 

    private void Awake()
    {
        //Find the character's Rigidbody and ground detection script
        body = GetComponent<Rigidbody2D>();
    }

    public void Init(Vector2 direction, float speed, float deccelaeration, bool useDecceleratiion, float deccelerationStart = 0f)
    {
        this.direction = direction;
        maxSpeed = speed;
        maxDecceleration = deccelaeration;
        this.deccelerationStartTimer = deccelerationStart;
        this.useDecceleration = useDecceleratiion;
        if (this.useDecceleration)
            deccelerationCoroutine = StartCoroutine(DeccelerationTimer());
    }



    /*private void Update()
    {
        desiredVelocity = direction * Mathf.Max(maxSpeed - friction, 0f);
    }*/

    private void FixedUpdate()
    {
        if (freezed) return;

        desiredVelocity = direction * Mathf.Max(maxSpeed - friction, 0f);
        velocity = body.velocity;

        if (deccelerating)
        {
            runWithDecceleration();
        }
        else
        {
            runWithoutAcceleration();
        }

        if (destroyOnStop && velocity.magnitude < 0.1f)
        {
            GetComponent<SpellBase>().OnHit(Structs.CollisionData.Empty);
            Destroy(gameObject);
        }
    }

    private IEnumerator DeccelerationTimer()
    {

        // yield return new WaitForSeconds(deccelerationStartTimer);
        if (deccelerationStartTimer > 0)
        {
            deccelerationStartTimer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
            deccelerationCoroutine = StartCoroutine(DeccelerationTimer());
        }
        else
        {
            direction = Vector2.zero;
            deccelerating = true;
        }
    }

    private void runWithDecceleration()
    {
        float x = Mathf.Clamp(velocity.x - velocity.normalized.x * maxDecceleration, Mathf.Sign(velocity.x) < 0 ? -maxSpeed : 0, Mathf.Sign(velocity.x) < 0 ? 0 : maxSpeed);
        float y = Mathf.Clamp(velocity.y - velocity.normalized.y * maxDecceleration, Mathf.Sign(velocity.y) < 0 ? -maxSpeed : 0, Mathf.Sign(velocity.y) < 0 ? 0 : maxSpeed);
        body.velocity = new Vector2(x, y);
    }

    private void runWithoutAcceleration()
    {
        velocity = desiredVelocity;

        body.velocity = velocity;
    }

    public void Freeze()
    {
        freezedVelocity = body.velocity;
        body.velocity = Vector2.zero;
        freezed = true;
        if (deccelerationCoroutine != null)
        {
            StopCoroutine(deccelerationCoroutine);
        }
    }

    public void Unfreeze()
    {
        freezed = false;
        body.velocity = freezedVelocity;
        if (deccelerationStartTimer > 0 && useDecceleration)
            deccelerationCoroutine = StartCoroutine(DeccelerationTimer());
        
    }
}
