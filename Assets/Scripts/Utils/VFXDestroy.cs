using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class VFXDestroy : MonoBehaviour
{
    VisualEffect effect;
    private float minimalLifeTime = 0.5f;

    [SerializeField] float destroyTimer = 0;

    void Awake()
    {
        effect = GetComponent<VisualEffect>();
        if (destroyTimer <= 0)
            StartCoroutine(ParticlesCountCoroutine());
        else
            StartCoroutine(TimerCoroutine());

    }

    /*void Update()
    {
        
        if (minimalLifeTime <= 0 && effect.aliveParticleCount == 0)
        {
            Destroy(effect.gameObject);
        }
        else
        {
            minimalLifeTime -= Time.deltaTime;
        }
    }*/

    IEnumerator ParticlesCountCoroutine()
    {
        if (minimalLifeTime <= 0 && effect.aliveParticleCount == 0)
        {
            Destroy(effect.gameObject);
        }
        else
        {
            minimalLifeTime -= Time.deltaTime;
        }
        yield return new WaitForEndOfFrame();
        StartCoroutine(ParticlesCountCoroutine());
    }

    IEnumerator TimerCoroutine()
    {
        destroyTimer -= Time.deltaTime;
        if (destroyTimer <= 0)
        {
            Destroy(effect.gameObject);
        }
        yield return new WaitForEndOfFrame();
        StartCoroutine(TimerCoroutine());
    }
}
