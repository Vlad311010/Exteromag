using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class VFXDestroy : MonoBehaviour
{
    VisualEffect effect;
    private float minimalLifeTime = 0.5f;

    [SerializeField] float destroyTimer = 0;

    [SerializeField] float shrinkPerFrame;
    [SerializeField] float shrinkLimit = 0.8f;

    void Awake()
    {
        effect = GetComponent<VisualEffect>();
        if (destroyTimer <= 0)
            StartCoroutine(ParticlesCountCoroutine());
        else
            StartCoroutine(TimerCoroutine());

    }

    IEnumerator ParticlesCountCoroutine()
    {
        if (minimalLifeTime <= 0 && effect.aliveParticleCount == 0)
        {
            InitDestroy();
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
            InitDestroy();
        }
        yield return new WaitForEndOfFrame();
        StartCoroutine(TimerCoroutine());
    }

    private void InitDestroy()
    {
        if (shrinkPerFrame <= 0)
            Destroy(gameObject);
        else
            StartCoroutine(Shrink());
    }

    IEnumerator Shrink()
    {
        transform.localScale = transform.localScale - Vector3.one * shrinkPerFrame * Time.deltaTime;
        yield return new WaitForEndOfFrame();
        if (transform.localScale.x < shrinkLimit || transform.localScale.y < shrinkLimit)
            Destroy(gameObject);
        else
            StartCoroutine(Shrink());
    }
}
