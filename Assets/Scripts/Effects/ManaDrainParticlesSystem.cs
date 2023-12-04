using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaDrainParticlesSystem : MonoBehaviour
{
    [SerializeField] Transform target;

    [SerializeField] float maxSpeed;
    [SerializeField] float acceleration;
    [SerializeField] float staleTime;
    [SerializeField] float terminatingDistanceToTarget;

    ParticleSystem ps;

    private bool moveToTarget = false;
    private Vector2 velocity = Vector2.zero;

    private void Start()
    {
        target = GameObject.FindWithTag("Player")?.transform;
        ps = GetComponent<ParticleSystem>();
        StartCoroutine(ChangeParticlesVelocity());
    }

    private void Update()
    {
        if (moveToTarget)
            Change();
    }

    private void Change()
    {
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[10];
        ps.GetParticles(particles, 10);
        for (int i = 0; i < particles.Length; i++)
        {
            Vector3 direction = target.position - particles[i].position;
            Vector3 desiredVelocity = direction * maxSpeed;
            float distance = Vector2.Distance(target.position, particles[i].position);
            float speedChange = acceleration * Time.deltaTime;
            
            velocity.x = Mathf.MoveTowards(particles[i].velocity.x, desiredVelocity.x, speedChange);
            velocity.y = Mathf.MoveTowards(particles[i].velocity.y, desiredVelocity.y, speedChange);
            particles[i].velocity = velocity;
            
            // speed up particles consumption
            if (desiredVelocity.magnitude - velocity.magnitude <= 1 || distance < terminatingDistanceToTarget)
                acceleration = 120;
        }
        ps.SetParticles(particles, 10);
        
    }

    IEnumerator ChangeParticlesVelocity()
    {
        yield return new WaitForSeconds(staleTime);
        moveToTarget = true;
    }
}
